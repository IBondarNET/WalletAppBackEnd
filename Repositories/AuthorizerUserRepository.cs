using Microsoft.EntityFrameworkCore;
using WalletApp.Backend.Data;
using WalletApp.Backend.Models;
using WalletApp.Backend.Repositories.Interfaces;
using WalletApp.Backend.Views;

namespace WalletApp.Backend.Repositories;

public class AuthorizerUserRepository : IAuthorizerUserRepository
{
    private readonly WalletContext _walletContext;

    public AuthorizerUserRepository(WalletContext walletContext)
    {
        _walletContext = walletContext;
    }

    public async Task<ICollection<AuthorizedUser>> GetAllUserByAccount(int accId)
    {
        return await _walletContext.AuthorizedUsers
            .Where(a=>a.AccountId == accId).ToListAsync();
    }
    
    public async Task<int?> AddUserToAccount(AddAuthUserView request)
    {
        var acc = await _walletContext.Accounts.FirstOrDefaultAsync(x => x.Id == request.AccId);
        if (acc == null)
        {
            return null;
        }
        var res = await _walletContext.AuthorizedUsers.AddAsync(new AuthorizedUser()
        {
            AccountId = request.AccId,
            Name = request.Name
        });
         await _walletContext.SaveChangesAsync();
        return res.Entity.Id;
    }

    public async Task<int?> DeleteUserFromAccount(int id)
    {
        var result = await _walletContext.AuthorizedUsers
            .Where(x=>x.Id == id)
            .ExecuteDeleteAsync();
        return result == 0 ? null : id;
    }
}

