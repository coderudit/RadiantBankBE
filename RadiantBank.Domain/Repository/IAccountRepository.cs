using RadiantBank.Domain.Models;

namespace RadiantBank.Domain.Repository;

public interface IAccountRepository
{
    Task<bool> AddAccountAsync(Account account);
    Task<bool> RemoveAccountAsync(string userId, string accountNumber);
    Task<IEnumerable<Account>> GetAccountsAsync(string userId);
    Task<Account?> GetAccountByIdAsync(string userId, string accountNumber);
    Task<bool> UpdateAccountAsync(Account account);
}