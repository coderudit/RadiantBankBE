using RadiantBank.Domain.Models;

namespace RadiantBank.Domain.Repository;

public interface IUserRepository
{
    Task<bool> AddUserAsync(User user);
    Task<IEnumerable<User>> GetUsersAsync();
    Task<User?> GetUserByIdAsync(string id);
}