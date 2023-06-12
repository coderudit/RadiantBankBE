using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RadiantBank.Domain.Models;
using RadiantBank.Domain.Repository;

namespace RadiantBank.Infrastructure.RadiantBankDB.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ILogger<UserRepository> _logger;
    private readonly IRadiantBankDbContext _dbContext;
    public UserRepository(ILogger<UserRepository> logger, IRadiantBankDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }
    
    public async Task<bool> AddUserAsync(User user)
    {
        try
        {
            await _dbContext.Users.AddAsync(user).ConfigureAwait(false);
            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError("Unable to add user {UserId} due to: {ExMessage}", user.Id, ex.Message);
            return false;
        }
        return true;
    }

    public async Task<IEnumerable<User>> GetUsersAsync()
    {
        return await _dbContext.Users.ToListAsync().ConfigureAwait(false);;
    }

    public async Task<User?> GetUserByIdAsync(string id)
    {
        return await _dbContext.Users.Where(x=>x.Id.Equals(id))
            .Include(x=>x.Accounts)
            .FirstOrDefaultAsync().ConfigureAwait(false);
    }
}