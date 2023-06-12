namespace RadiantBank.Domain.Models;

public class User
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Address { get; set; } = null!;
    public string ContactNumber { get; set; } = null!;
    public string? EmailAddress { get; set; }
    
    public ICollection<Account> Accounts { get; } = new List<Account>();
}