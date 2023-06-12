namespace RadiantBank.Application.DTOs;

public record AccountDTO
{
    public string AccountNumber { get; set; } = null!;
    public DateTime OpenDate { get; set; }
    public int IsActive { get; set; }
    public decimal TotalBalance { get; set; }
    public ICollection<TransactionDTO> TransactionHistory { get; set; } = new List<TransactionDTO>();
}