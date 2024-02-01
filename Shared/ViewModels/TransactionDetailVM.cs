namespace WalletApp.Backend.Shared.ViewModels;

public class TransactionDetailVM
{
    public decimal Total { get; set; }
    public string TypeName { get; set; }
    public string Name { get; set; }
    public string Date { get; set; }
    public string StatusName { get; set; }
    public string Description { get; set; }
}