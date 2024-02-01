using WalletApp.Backend.Models;
using WalletApp.Backend.Views;

namespace WalletApp.Backend.Repositories.Interfaces;

public interface IAuthorizerUserRepository
{
    Task<ICollection<AuthorizedUser>> GetAllUserByAccount(int accId);
    Task<int?> AddUserToAccount(AddAuthUserView request);
    Task<int?> DeleteUserFromAccount(int id);
}