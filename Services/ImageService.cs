using System.Drawing;
using System.Net.Mime;
using WalletApp.Backend.Services.Interfaces;

namespace WalletApp.Backend.Services;

public class ImageService : IImageService
{
    private readonly Random _random;
    private readonly string _imageFolder;


    public ImageService(IConfiguration configuration)
    {
        _random = new Random();
        _imageFolder = configuration.GetSection("ImageFolder:Path").Value!;
    }

    public string? GenerateImage()
    {
        try
        {
            var randomColor = GetDarkRandomColor();
            var image = new Bitmap(40, 40);

            using var graphics = Graphics.FromImage(image);
            graphics.Clear(randomColor);

            using var transactionIcon = SystemIcons.Information;
            graphics.DrawIcon(transactionIcon, new Rectangle(0, 0, image.Width, image.Height));

            var fileName = Guid.NewGuid() + ".jpeg";
            image.Save(_imageFolder + fileName, System.Drawing.Imaging.ImageFormat.Jpeg);

            return fileName;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
    }

    public byte[]? GetFile(string fileName)
    {
        var path = _imageFolder + fileName;
        if (File.Exists(path))
        {
            using var stream = File.OpenRead(path);
            using var ms = new MemoryStream();

            stream.CopyTo(ms);
            return ms.ToArray();
        }
        else
        {
            return null;
        }
    }

    public bool DeleteImage(string fileName)
    {
        var path = _imageFolder + fileName;
        try
        {
            File.Delete(path);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return false;
        }
    }

    private Color GetDarkRandomColor()
    {
        var red = _random.Next(0, 128);
        var green = _random.Next(0, 128);
        var blue = _random.Next(0, 128);

        return Color.FromArgb(red, green, blue);
    }
}