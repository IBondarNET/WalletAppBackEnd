using Microsoft.EntityFrameworkCore;
using WalletApp.Backend.Data;
using WalletApp.Backend.Models;
using WalletApp.Backend.Repositories.Interfaces;
using WalletApp.Backend.Services.Interfaces;
using WalletApp.Backend.Shared.Constants;
using WalletApp.Backend.Views;

namespace WalletApp.Backend.Repositories;

public class AccountRepository : IAccountRepository
{
    private readonly WalletContext _walletContext;
    private readonly IDailyPointsService _pointsService;
    private readonly Random _random;

    public AccountRepository(WalletContext walletContext, IDailyPointsService pointsService)
    {
        _walletContext = walletContext;
        _pointsService = pointsService;
        _random = new Random();
    }

    public async Task<int> AddAccount(AddUserView request)
    {
        var balance = _random.Next(0, 1500);
        var res = await _walletContext.Accounts.AddAsync(new Account()
        {
            Name = request.Name,
            Balance = balance,
            Limit = Constants.MaxValueBalance - balance,
            Points = _pointsService.GetPointOfThisDate(DateTime.Now)
        });
        await _walletContext.SaveChangesAsync();
        return res.Entity.Id;
    }

    public async Task<Account?> GetAccount(int id)
    {
        var points = _pointsService.GetPointOfThisDate(DateTime.Now);
        await _walletContext.Accounts
            .Where(x => x.Id == id)
            .ExecuteUpdateAsync(setter => setter
                .SetProperty(x => x.Points, points));
        
        var acc = await _walletContext.Accounts.FirstOrDefaultAsync(a => a.Id == id);
        
        return acc;
    }

    public async Task<ICollection<Account>> GetAllAccounts()
    {
        var points = _pointsService.GetPointOfThisDate(DateTime.Now);
        await _walletContext.Accounts
            .ExecuteUpdateAsync(setter => setter
                .SetProperty(x => x.Points, points));
        return await _walletContext.Accounts.ToListAsync();
    }

    public async Task<int?> DeleteAccount(int id)
    {
        var result = await _walletContext.Accounts
            .Where(x => x.Id == id)
            .ExecuteDeleteAsync();
        return result == 0 ? null : id;
    }
}