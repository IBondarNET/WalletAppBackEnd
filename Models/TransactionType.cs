using System.ComponentModel.DataAnnotations;

namespace WalletApp.Backend.Models;

public class TransactionType
{
    public int Id { get; set; }
    public string Name { get; set; }
}