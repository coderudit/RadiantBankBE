using Microsoft.EntityFrameworkCore;
using RadiantBank.Application.Services.Interfaces;
using RadiantBank.Domain.Enums;
using RadiantBank.Domain.Models;
using RadiantBank.Domain.Repository;

namespace RadiantBank.Application.Services.Implementations;

public class AccountService : IAccountService
{
    private readonly IUserRepository _userRepo;
    private readonly IAccountRepository _accountRepo;
    private readonly ITransactionRepository _transactionRepo;
    private const decimal MIN_BALANCE = 100;
    private const decimal MIN_PERCENT = (decimal)0.9;
    private const decimal MAX_AMOUNT = 10000;
    
    public AccountService(IUserRepository userRepo,
        IAccountRepository accountRepo,
        ITransactionRepository transactionRepo)
    {
        _userRepo = userRepo;
        _accountRepo = accountRepo;
        _transactionRepo = transactionRepo;
    }
    
    public async Task<bool> AddAccountAsync(Account account, decimal initialAmount)
    {
        var accountAdded = await _accountRepo.AddAccountAsync(account).ConfigureAwait(false);
        if (!accountAdded)
            return false;
        
        var isInitialDepositDone = await DepositToAccountAsync(account.UserId, account.AccountNumber, initialAmount).
            ConfigureAwait(false);
        if (!isInitialDepositDone)
        {
            //Log error
        }
        return true;
    }

    public async Task<bool> RemoveAccountAsync(string userId, string accountNumber)
    {
        return await _accountRepo.RemoveAccountAsync(userId, accountNumber).ConfigureAwait(false);
    }

    public async Task<IEnumerable<Account>> GetAccountsAsync(string userId)
    {
        return await _accountRepo.GetAccountsAsync(userId).ConfigureAwait(false);
    }

    public async Task<Account?> GetAccountByIdAsync(string userId, string accountNumber)
    {
        return await _accountRepo.GetAccountByIdAsync(userId, accountNumber).ConfigureAwait(false);
    }

    public async Task<bool> DepositToAccountAsync(string userId, string accountNumber, decimal amount)
    {
        var account = await GetAccountByIdAsync(userId, accountNumber).ConfigureAwait(false);
        if (account is not { IsActive: true } ||
            amount <= 0 ||
            amount > MAX_AMOUNT)
            return false;
        
        var previousBalance = account.TotalBalance;
        account.TotalBalance += amount;

        var transaction = new Transaction
        {
            Id = Guid.NewGuid().ToString(),
            AccountNumber = accountNumber,
            Comment = $"Depositing {amount} in the account at {DateTime.Now}",
            PreviousBalance = previousBalance,
            CurrentBalance = account.TotalBalance,
            Time = DateTime.Now,
            TypeId = (int)TransactionType.Deposit,
        };
        await _accountRepo.UpdateAccountAsync(account).ConfigureAwait(false);
        await _transactionRepo.AddTransactionAsync(transaction).ConfigureAwait(false);
        return true;
    }

    public async Task<bool> WithdrawFromAccountAsync(string userId, string accountNumber, decimal amount)
    {
        var account = await GetAccountByIdAsync(userId, accountNumber).ConfigureAwait(false);
        if (account is not { IsActive: true } ||
            amount < 0 ||
            amount > MIN_PERCENT * account.TotalBalance ||
            account.TotalBalance - amount < MIN_BALANCE)
            return false;

        var previousBalance = account.TotalBalance;
        account.TotalBalance -= amount;
        var transaction = new Transaction
        {
            Id = Guid.NewGuid().ToString(),
            AccountNumber = accountNumber,
            Comment = $"Withdrawing {amount} from the account at {DateTime.Now}",
            PreviousBalance = previousBalance,
            CurrentBalance = account.TotalBalance,
            Time = DateTime.Now,
            TypeId = (int)TransactionType.Withdraw,
        };
        await _accountRepo.UpdateAccountAsync(account).ConfigureAwait(false);
        await _transactionRepo.AddTransactionAsync(transaction).ConfigureAwait(false);
        return true;
    }
}