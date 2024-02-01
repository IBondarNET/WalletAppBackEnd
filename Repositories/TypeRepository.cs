using Microsoft.EntityFrameworkCore;
using WalletApp.Backend.Data;
using WalletApp.Backend.Models;
using WalletApp.Backend.Repositories.Interfaces;
using WalletApp.Backend.Views;

namespace WalletApp.Backend.Repositories;

public class TypeRepository : ITypeRepository
{
    private readonly WalletContext _walletContext;

    public TypeRepository(WalletContext walletContext)
    {
        _walletContext = walletContext;
    }

    public async Task<ICollection<TransactionType>> GetAllType()
    {
        return await _walletContext.Types.ToListAsync();
    }
    
    public async Task<int> AddType(AddTypeView request)
    {
        var res = await _walletContext.Types.AddAsync(new TransactionType()
        {
            Name = request.Name
        });
        await _walletContext.SaveChangesAsync();
        return res.Entity.Id;
    }

    public async Task<int?> DeleteType(int id)
    {
        var result = await _walletContext.Types
            .Where(x=>x.Id == id)
            .ExecuteDeleteAsync();
        return result == 0 ? null : id;
    }
}