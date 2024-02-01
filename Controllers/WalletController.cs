using System.Net;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using WalletApp.Backend.Repositories.Interfaces;
using WalletApp.Backend.Services.Interfaces;
using WalletApp.Backend.Views;

namespace WalletApp.Backend.Controllers;

[ApiController]
[Route("[controller]")]
public class WalletController : ControllerBase
{
    private readonly ILogger<WalletController> _logger;
    private readonly IAccountRepository _accountRepository;
    private readonly IAuthorizerUserRepository _authUserRepository;
    private readonly IImageRepository _imageRepository;
    private readonly IStatusRepository _statusRepository;
    private readonly ITransactionRepository _transactionRepository;
    private readonly ITypeRepository _typeRepository;
    private readonly IDailyPointsService _pointsService;
    private readonly IImageService _imageService;

    public WalletController(ILogger<WalletController> logger,
        IAccountRepository accountRepository,
        IAuthorizerUserRepository authUserRepository,
        IImageRepository imageRepository,
        IStatusRepository statusRepository,
        ITransactionRepository transactionRepository,
        ITypeRepository typeRepository,
        IDailyPointsService pointsService,
        IImageService imageService)
    {
        _logger = logger;
        _accountRepository = accountRepository;
        _authUserRepository = authUserRepository;
        _imageRepository = imageRepository;
        _statusRepository = statusRepository;
        _transactionRepository = transactionRepository;
        _typeRepository = typeRepository;
        _pointsService = pointsService;
        _imageService = imageService;
    }

    // Type
    [HttpGet("type")]
    public async Task<IActionResult> GetAllTypesAsync()
    {
        return Ok(await _typeRepository.GetAllType());
    }

    [HttpPost("type")]
    public async Task<IActionResult> AddTypeAsync(AddTypeView request)
    {
        var res = await _typeRepository.AddType(request);
        return Ok(new
        {
            Id = res
        });
    }

    [HttpDelete("type/{id:int}")]
    public async Task<IActionResult> DeleteTypeAsync([FromRoute] int id)
    {
        var res = await _typeRepository.DeleteType(id);

        if (res == null)
        {
            return BadRequest("Неможливо видалити не існуючий тип");
        }

        return Ok(new
        {
            Id = res
        });
    }

    // Status
    [HttpGet("status")]
    public async Task<IActionResult> GetAllStatusAsync()
    {
        return Ok(await _statusRepository.GetAllStatus());
    }

    [HttpPost("status")]
    public async Task<IActionResult> AddStatusAsync(AddStatusView request)
    {
        var res = await _statusRepository.AddStatus(request);
        return Ok(new
        {
            Id = res
        });
    }

    [HttpDelete("status/{id:int}")]
    public async Task<IActionResult> AddStatusAsync([FromRoute] int id)
    {
        var res = await _statusRepository.DeleteStatus(id);
        if (res == null)
        {
            return BadRequest("Неможливо видалити не існуючий статус");
        }

        return Ok(new
        {
            Id = res
        });
    }

    // AuthUser
    [HttpGet("authUser/{accId:int}")]
    public async Task<IActionResult> GetAllUserByAccountAsync([FromRoute] int accId)
    {
        return Ok(await _authUserRepository.GetAllUserByAccount(accId));
    }

    [HttpPost("authUser")]
    public async Task<IActionResult> AddUserToAccount(AddAuthUserView request)
    {
        var res = await _authUserRepository.AddUserToAccount(request);
        if (res == null)
        {
            return BadRequest("Неможливо додати юзера до не існуючого аккаунта");
        }
        return Ok(new
        {
            Id = res
        });
    }

    [HttpDelete("authUser/{id:int}")]
    public async Task<IActionResult> DeleteUserFromAccountAsync([FromRoute] int id)
    {
        var res = await _authUserRepository.DeleteUserFromAccount(id);
        if (res == null)
        {
            return BadRequest("Неможливо видалити не існуючого юзера");
        }

        return Ok(new
        {
            Id = res
        });
    }

    // Image
    [HttpGet("image")]
    public async Task<IActionResult> GetAllImagesAsync()
    {
        return Ok(await _imageRepository.GetAllImages());
    }

    [HttpGet("image/{id:int}")]
    public async Task<IActionResult> GetImageAsync([FromRoute] int id)
    {
        var res = await _imageRepository.GetImageById(id);
        if (res == null)
        {
            return BadRequest("Невдалося отримати картинку");
        }

        var image = _imageService.GetFile(res.Name);
        if (image == null)
        {
            return BadRequest("Невдалося отримати картинку");
        }
        
        return File(image, res.ContentType);
    }

    [HttpPost("image")]
    public async Task<IActionResult> AddImageAsync()
    {
        var res = await _imageRepository.AddImage();
        if (res == null)
        {
            return BadRequest("Невдалося згенерувати картинку");
        }

        return Ok(new
        {
            Id = res
        });
    }

    [HttpDelete("image/{id:int}")]
    public async Task<IActionResult> DeleteImageAsync([FromRoute] int id)
    {
        var res = await _imageRepository.DeleteImage(id);
        if (res == null)
        {
            return BadRequest("Невдалося видалити картинку");
        }

        return Ok(new
        {
            Id = res
        });
    }

    //Account
    [HttpGet("account")]
    public async Task<IActionResult> GetAllAccountsAsync()
    {
        return Ok(await _accountRepository.GetAllAccounts());
    }

    [HttpGet("account/{id:int}")]
    public async Task<IActionResult> GetAccountAsync([FromRoute] int id)
    {
        var res = await _accountRepository.GetAccount(id);
        if (res == null)
        {
            return BadRequest("Неможливо отримати не існуючий аккаунт");
        }

        return Ok(res);
    }

    [HttpPost("account")]
    public async Task<IActionResult> AddAccountAsync(AddUserView request)
    {
        var res = await _accountRepository.AddAccount(request);
        return Ok(new
        {
            Id = res
        });
    }

    [HttpDelete("account/{id:int}")]
    public async Task<IActionResult> DeleteAccountAsync([FromRoute] int id)
    {
        var res = await _accountRepository.DeleteAccount(id);
        if (res == null)
        {
            return BadRequest("Неможливо видалити не існуючий аккаунт");
        }

        return Ok(new
        {
            Id = res
        });
    }

    //Transaction
    [HttpGet("transaction/{accId:int}/last")]
    public async Task<IActionResult> GetLastTenTransactionsAsync([FromRoute] int accId)
    {
        return Ok(await _transactionRepository.GetLastTenTransactions(accId));
    }

    [HttpPost("transaction")]
    public async Task<IActionResult> AddTransactionAsync(AddTransactionView request)
    {
        var res = await _transactionRepository.AddTransaction(request);
        if (res == null)
        {
            return BadRequest("Невдалося додати транзакцію, перевірте корректність данних");
        }

        return Ok(new
        {
            Id = res
        });
    }

    [HttpGet("transaction/{id:int}/detail")]
    public async Task<IActionResult> GetTransactionDetailAsync([FromRoute] int id)
    {
        var res = await _transactionRepository.GetTransactionDetail(id);
        if (res == null)
        {
            return BadRequest("Невдалося отримати деталі транзакції");
        }

        return Ok(res);
    }

    [HttpGet("transactions/{accId:int}/list")]
    public async Task<IActionResult> GetTransactionsListAsync([FromRoute] int accId)
    {
        var accRes = await _accountRepository.GetAccount(accId);
        if (accRes == null)
        {
            return BadRequest("Неможливо отримати транзакції з не існуючого аккаунт");
        }

        var transactionRes = await _transactionRepository.GetLastTenTransactions(accId);
        return Ok(new
        {
            CardBalance = accRes.Balance,
            Available = accRes.Limit,
            NoPaymentDue =
                $"You’ve paid your {DateTime.Now.ToString("MMMM", System.Globalization.CultureInfo.InvariantCulture)} balance.",
            DailyPoints = _pointsService.PointToString(accRes.Points),
            LatestTransactions = transactionRes
        });
    }
}