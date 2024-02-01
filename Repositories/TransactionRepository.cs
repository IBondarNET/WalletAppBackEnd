using Microsoft.EntityFrameworkCore;
using WalletApp.Backend.Data;
using WalletApp.Backend.Models;
using WalletApp.Backend.Repositories.Interfaces;
using WalletApp.Backend.Shared.Constants;
using WalletApp.Backend.Shared.Enums;
using WalletApp.Backend.Shared.ViewModels;
using WalletApp.Backend.Views;


namespace WalletApp.Backend.Repositories;

public class TransactionRepository : ITransactionRepository
{
    private readonly WalletContext _walletContext;

    public TransactionRepository(WalletContext walletContext)
    {
        _walletContext = walletContext;
    }

    public async Task<ICollection<LatestTransactionVM>> GetLastTenTransactions(int accId)
    {
        var query = from tr in _walletContext.Transactions
            join tp in _walletContext.Types on tr.TypeId equals tp.Id
            join s in _walletContext.Statuses on tr.StatusId equals s.Id
            join athUser in _walletContext.AuthorizedUsers on tr.AuthorizedUserId equals athUser.Id into athUserD
            // join ic in _walletContext.Images on tr.ImageId equals ic.Id
            from authUser in athUserD.DefaultIfEmpty()
            where tr.AccountId == accId
            orderby tr.DateCreate
            select new LatestTransactionVM()
            {
                Id = tr.Id,
                TotalWithType = (tp.Name == ConstantType.Payment.ToString() ? "+$" : "$") + tr.Total,
                Name = tr.Name,
                DescriptionWithStatus = (s.Name == ConstantStatus.Accepted.ToString() ? string.Empty : s.Name + " - ")
                                        + tr.Description,
                DateWithAuthUser = (authUser == default ? string.Empty : authUser.Name + " - ")
                                   + (tr.DateCreate.Date < DateTime.Now.Date.AddDays(-6)
                                       ? tr.DateCreate.ToString("M/d/yy").Replace('.', '/')
                                       : tr.DateCreate.Date < DateTime.Now.Date.AddDays(-2)
                                           ? tr.DateCreate.DayOfWeek.ToString()
                                           : tr.DateCreate.Date == DateTime.Now.Date
                                               ? "Today"
                                               : "Yesterday"
                                   ),
                ImageId = tr.ImageId
            };
        return await query.Take(10).ToListAsync();
    }

    public async Task<TransactionDetailVM?> GetTransactionDetail(int id)
    {
        var query = from tr in _walletContext.Transactions
            join tp in _walletContext.Types on tr.TypeId equals tp.Id
            join s in _walletContext.Statuses on tr.StatusId equals s.Id
            where tr.Id == id
            select new TransactionDetailVM()
            {
                Total = tr.Total,
                TypeName = tp.Name,
                Name = tr.Name,
                Date = tr.DateCreate.ToString("M/d/yy, HH:mm").Replace('.', '/'),
                StatusName = s.Name,
                Description = tr.Description
            };
        return await query.FirstOrDefaultAsync();
    }

    public async Task<int?> AddTransaction(AddTransactionView request)
    {
        await using  var  transactionDb = await _walletContext.Database.BeginTransactionAsync();
        try
        {
            var query = from acc in _walletContext.Accounts
                join authUser in _walletContext.AuthorizedUsers on acc.Id equals authUser.AccountId into athUserD
                from ath in athUserD.DefaultIfEmpty() 
                where acc.Id == request.AccountId
                select new
                {
                    AuthUserId = ath.Id,
                    Balance = acc.Balance,
                    Limit = acc.Limit
                };
            var accRes = await query.ToListAsync();
            if (request.AuthorizedUserId != null && accRes.All(x => x.AuthUserId != request.AuthorizedUserId))
            {
                return null;
            }

            if (accRes.Count != 0)
            {
                var sum = request.TypeId == (int)ConstantType.Payment ? request.Total : -request.Total;
                var b = accRes[0].Balance + sum > Constants.MaxValueBalance;
                if (b || accRes[0].Balance + sum < Constants.MinValueBalance)
                {
                    return null;
                }

                await _walletContext.Accounts
                    .Where(acc => acc.Id == request.AccountId)
                    .ExecuteUpdateAsync(setter => setter
                        .SetProperty(acc => acc.Balance, accRes[0].Balance + sum)
                        .SetProperty(acc => acc.Limit, accRes[0].Limit - sum)
                    );
            }
        
            var transaction = await _walletContext.AddAsync(new Transaction()
            {
                AccountId = request.AccountId,
                TypeId = request.TypeId,
                Total = request.Total,
                Name = request.Name,
                Description = request.Description,
                StatusId = request.StatusId,
                AuthorizedUserId = request.AuthorizedUserId,
                ImageId = request.ImageId
            });
            
            await _walletContext.SaveChangesAsync();
            await transactionDb.CommitAsync();
            
            return transaction.Entity.Id;
        }
        catch (Exception e)
        {
            Console.WriteLine("BadSaveDb" + request.AccountId);
            await transactionDb.RollbackAsync();
            return null;
        }
    }
}