using RadiantBank.Application.Common.Mappings;

namespace RadiantBank.Application.DTOs;

public record TransactionDTO
{
    public string Id { get; set; } = null!;
    public string TransactionType { get; set; } = null!;
    public DateTime Time { get; set; }
    public decimal PreviousBalance { get; set; }
    public decimal CurrentBalance { get; set; }
}