using WalletApp.Backend.Shared.Constants;

namespace WalletApp.Backend.Models;

public class Account
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Balance { get; set; }
    public decimal Limit { get; set; }
    public long Points { get; set; }
    public DateTime DateCreate { get; set; } = DateTime.Now;
    
    public List<AuthorizedUser>? AuthorizedUsers { get; set; }
}