using System.ComponentModel.DataAnnotations;

namespace WalletApp.Backend.Models;

public class Transaction
{
    public int Id { get; set; } 
    public int AccountId { get; set; }
    public int TypeId { get; set; }
    public decimal Total { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime DateCreate { get; set; } = DateTime.Now;
    public int StatusId { get; set; }
    public int? AuthorizedUserId { get; set; }
    public int ImageId { get; set; }
    
    public Account Account { get; set; }
    public TransactionType TransactionType { get; set; }
    public Status Status { get; set; }
    public AuthorizedUser? AuthorizedUser { get; set; }
    public TransactionImage TransactionImage { get; set; }
    
}