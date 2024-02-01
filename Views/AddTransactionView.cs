namespace WalletApp.Backend.Views;

public class AddTransactionView
{
    public int AccountId { get; set; }
    public int TypeId { get; set; }
    public decimal Total { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int StatusId { get; set; }
    public int? AuthorizedUserId { get; set; }
    public int ImageId { get; set; }
}
