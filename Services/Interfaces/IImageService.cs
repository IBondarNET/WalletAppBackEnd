namespace WalletApp.Backend.Services.Interfaces;

public interface IImageService
{
    string? GenerateImage();
    byte[]? GetFile(string fileName);
    bool DeleteImage(string fileName);
}