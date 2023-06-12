using RadiantBank.Domain.Models;

namespace RadiantBank.Application.Services.Interfaces;

public interface IAccountService
{
    public Task<bool> AddAccountAsync(Account account, decimal initialAmount);
    public Task<bool> RemoveAccountAsync(string userId, string accountNumber);
    public Task<IEnumerable<Account>> GetAccountsAsync(string userId);
    public Task<Account?> GetAccountByIdAsync(string userId, string accountNumber);
    
    public Task<bool> DepositToAccountAsync(string userId, string accountNumber, decimal amount);
    
    public Task<bool> WithdrawFromAccountAsync(string userId, string accountNumber, decimal amount);
}