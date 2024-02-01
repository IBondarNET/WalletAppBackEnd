using WalletApp.Backend.Models;
using WalletApp.Backend.Views;

namespace WalletApp.Backend.Repositories.Interfaces;

public interface ITypeRepository
{
    Task<ICollection<TransactionType>> GetAllType();
    Task<int> AddType(AddTypeView request);
    Task<int?> DeleteType(int id);
}