using WalletApp.Backend.Shared.ViewModels;
using WalletApp.Backend.Views;

namespace WalletApp.Backend.Repositories.Interfaces;

public interface ITransactionRepository
{
    Task<ICollection<LatestTransactionVM>> GetLastTenTransactions(int accId);
    Task<TransactionDetailVM?> GetTransactionDetail(int id);
    Task<int?> AddTransaction(AddTransactionView request);
}