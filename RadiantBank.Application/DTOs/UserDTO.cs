using RadiantBank.Application.Common.Mappings;
using RadiantBank.Domain.Models;

namespace RadiantBank.Application.DTOs;

public class UserDTO
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Address { get; set; } = null!;
    public string ContactNumber { get; set; } = null!;
    public string? EmailAddress { get; set; }
    public ICollection<string> Accounts { get; set; } = new List<string>();
}