using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RadiantBank.Domain.Models;
using RadiantBank.Domain.Repository;

namespace RadiantBank.Infrastructure.RadiantBankDB.Repositories;

public class AccountRepository : IAccountRepository
{
    private readonly ILogger<AccountRepository> _logger;
    private readonly IRadiantBankDbContext _dbContext;
    public AccountRepository(ILogger<AccountRepository> logger, IRadiantBankDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }
    
    public async Task<bool> AddAccountAsync(Account account)
    {
        var user = await _dbContext.Users.
            Where(x => x.Id.Equals(account.UserId)).
            FirstOrDefaultAsync();
        
        if (user == null)
            return false;
        
        user.Accounts.Add(account);
        
        try
        {
            await _dbContext.Accounts.AddAsync(account);
            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError("Unable to add account {AccountAccountNumber} for user: {AccountUserId} due to: {ExMessage}",
                account.AccountNumber, account.UserId, ex.Message);
            return false;
        }
        return true;
    }

    public async Task<bool> RemoveAccountAsync(string userId, string accountNumber)
    {
        var user = await _dbContext.Users.
            Where(x => x.Id.Equals(userId)).
            FirstOrDefaultAsync();
        
        var account = user?.Accounts.FirstOrDefault(x => x.AccountNumber.Equals(accountNumber));
        
        if (account == null)
            return false;

        account.IsActive = false;
        _dbContext.Accounts.Update(account);
        return true;
    }

    public async Task<IEnumerable<Account>> GetAccountsAsync(string userId)
    {
        return await _dbContext.Accounts.
            Where(x=>x.UserId == userId).
            ToListAsync();
    }

    public async Task<Account?> GetAccountByIdAsync(string userId, string accountNumber)
    {
        return await _dbContext.Accounts.
            Where(x=>x.UserId.Equals(userId)
                  && x.AccountNumber.Equals(accountNumber)
                  && x.IsActive)
            .Include(x=>x.TransactionHistory)
            .FirstOrDefaultAsync().ConfigureAwait(false);;
    }

    public async Task<bool> UpdateAccountAsync(Account account)
    {
        _dbContext.Accounts.Update(account);
        await _dbContext.SaveChangesAsync();
        return true;
    }
}