namespace WalletApp.Backend.Models;

public class AuthorizedUser
{
    public int Id { get; set; }
    public int AccountId { get; set; }
    public string Name { get; set; }
    
    public Account Account { get; set; }
}