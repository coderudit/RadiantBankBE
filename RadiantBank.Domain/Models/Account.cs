using System.ComponentModel.DataAnnotations;

namespace RadiantBank.Domain.Models;

public class Account
{
    [Key]
    public string AccountNumber { get; set; } = null!;
    public DateTime OpenDate { get; set; }
    public bool IsActive { get; set; }
    public DateTime CloseDate { get; set; }
    public decimal TotalBalance { get; set; }
    public string UserId { get; set; }
    
    //Ensures foreign key constraint that Account does not exist without being associated to a user.
    public User User { get; set; } = null!;
    
    public ICollection<Transaction> TransactionHistory { get; set; } = new List<Transaction>();
}