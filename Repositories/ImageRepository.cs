using Microsoft.EntityFrameworkCore;
using WalletApp.Backend.Data;
using WalletApp.Backend.Models;
using WalletApp.Backend.Repositories.Interfaces;
using WalletApp.Backend.Services.Interfaces;

namespace WalletApp.Backend.Repositories;

public class ImageRepository : IImageRepository
{
    private readonly WalletContext _walletContext;
    private readonly IImageService _imageService;

    public ImageRepository(WalletContext walletContext, IImageService imageService)
    {
        _walletContext = walletContext;
        _imageService = imageService;
    }

    public async Task<ICollection<TransactionImage>> GetAllImages()
    {
        return await _walletContext.Images.ToListAsync();
    }

    public async Task<TransactionImage?> GetImageById(int id)
    {
        var res = await _walletContext.Images.FirstOrDefaultAsync(i => i.Id == id);

        return res ?? null;
    }
    
    public async Task<int?> AddImage()
    {
        var imageName =  _imageService.GenerateImage();
        if (imageName == null)
        {
            return null;
        }
        var res = await _walletContext.Images.AddAsync(new TransactionImage()
        {
            Name = imageName,
            ContentType = "image/jpeg"
        });
        await _walletContext.SaveChangesAsync();
        return res.Entity.Id;
    }

    public async Task<int?> DeleteImage(int id)
    {
        var image = await _walletContext.Images.FirstOrDefaultAsync(i => i.Id == id);
        
        if (image != null  &&  _imageService.DeleteImage(image.Name))
        { 
            _walletContext.Images.Remove(image);
        }

        var result = await _walletContext.SaveChangesAsync();
        return result == 0 ? null : id;
    }
}