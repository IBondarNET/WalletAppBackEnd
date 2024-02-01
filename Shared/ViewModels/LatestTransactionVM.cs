namespace WalletApp.Backend.Shared.ViewModels;

public class LatestTransactionVM
{
    public int Id { get; set; }
    public string TotalWithType { get; set; }
    public string Name { get; set; }
    public string DescriptionWithStatus { get; set; }
    public string DateWithAuthUser { get; set; }
    public int ImageId { get; set; }
}