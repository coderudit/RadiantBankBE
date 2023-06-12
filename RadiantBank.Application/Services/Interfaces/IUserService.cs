using RadiantBank.Domain.Models;

namespace RadiantBank.Application.Services.Interfaces;

public interface IUserService
{
    public Task<bool> AddUserAsync(User user);
    public Task<IEnumerable<User>> GetUsersAsync();
    public Task<User?> GetUserByIdAsync(string userId);
}