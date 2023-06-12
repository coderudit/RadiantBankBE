using RadiantBank.Application.Services.Interfaces;
using RadiantBank.Domain.Models;
using RadiantBank.Domain.Repository;

namespace RadiantBank.Application.Services.Implementations;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<bool> AddUserAsync(User user)
    {
        return await _userRepository.AddUserAsync(user).ConfigureAwait(false);
    }

    public async Task<IEnumerable<User>> GetUsersAsync()
    {
        return await _userRepository.GetUsersAsync().ConfigureAwait(false);
    }
    
    public async Task<User?> GetUserByIdAsync(string userId)
    {
        return await _userRepository.GetUserByIdAsync(userId).ConfigureAwait(false);
    }
}