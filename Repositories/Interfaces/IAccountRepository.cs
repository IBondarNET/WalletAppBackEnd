using WalletApp.Backend.Models;
using WalletApp.Backend.Views;

namespace WalletApp.Backend.Repositories.Interfaces;

public interface IAccountRepository
{
    Task<int> AddAccount(AddUserView request);
    Task<Account?> GetAccount(int id);
    Task<ICollection<Account>> GetAllAccounts();
    Task<int?> DeleteAccount(int id);
}