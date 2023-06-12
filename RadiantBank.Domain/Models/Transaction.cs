using System.ComponentModel.DataAnnotations.Schema;

namespace RadiantBank.Domain.Models;

public class Transaction
{
    public string Id { get; set; }
    public short TypeId { get; set; }
    public DateTime Time { get; set; }
    public decimal PreviousBalance { get; set; }
    public decimal CurrentBalance { get; set; }
    public string Comment { get; set; }
    //Ensures foreign key constraint that Transaction does not exist without being associated to a account.
    [ForeignKey("AccountNumber")]
    public string AccountNumber { get; set; } = null!;
}