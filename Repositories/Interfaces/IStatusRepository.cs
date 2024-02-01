using WalletApp.Backend.Models;
using WalletApp.Backend.Views;

namespace WalletApp.Backend.Repositories.Interfaces;

public interface IStatusRepository
{
    Task<ICollection<Status>> GetAllStatus();
    Task<int> AddStatus(AddStatusView request);
    Task<int?> DeleteStatus(int id);
}