using Microsoft.EntityFrameworkCore;
using WalletApp.Backend.Data;
using WalletApp.Backend.Models;
using WalletApp.Backend.Repositories.Interfaces;
using WalletApp.Backend.Views;

namespace WalletApp.Backend.Repositories;

public class StatusRepository : IStatusRepository
{
    private readonly WalletContext _walletContext;

    public StatusRepository(WalletContext walletContext)
    {
        _walletContext = walletContext;
    }

    public async Task<ICollection<Status>> GetAllStatus()
    {
        return await _walletContext.Statuses.ToListAsync();
    }
    
    public async Task<int> AddStatus(AddStatusView request)
    {
        var res = await _walletContext.Statuses.AddAsync(new Status()
        {
            Name = request.Name
        });
        await _walletContext.SaveChangesAsync();
        return res.Entity.Id;
    }

    public async Task<int?> DeleteStatus(int id)
    {
        var result = await _walletContext.Statuses
            .Where(x=>x.Id == id)
            .ExecuteDeleteAsync();
        return result == 0 ? null : id;
    }
}

