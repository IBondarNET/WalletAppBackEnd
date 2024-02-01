using WalletApp.Backend.Models;

namespace WalletApp.Backend.Repositories.Interfaces;

public interface IImageRepository
{
    Task<ICollection<TransactionImage>> GetAllImages();
    Task<TransactionImage?> GetImageById(int id);
    Task<int?> AddImage();
    Task<int?> DeleteImage(int id);
}