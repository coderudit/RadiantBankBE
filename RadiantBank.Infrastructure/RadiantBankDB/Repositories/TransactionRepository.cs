using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RadiantBank.Domain.Models;
using RadiantBank.Domain.Repository;

namespace RadiantBank.Infrastructure.RadiantBankDB.Repositories;

public class TransactionRepository : ITransactionRepository
{
    private readonly ILogger<TransactionRepository> _logger;
    private readonly IRadiantBankDbContext _dbContext;
    
    public TransactionRepository(ILogger<TransactionRepository> logger, IRadiantBankDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }
    
    public async Task<bool> AddTransactionAsync(Transaction transaction)
    {
        try
        {
            var account = await _dbContext.Accounts.Where(x => x.AccountNumber.Equals(transaction.AccountNumber))
                .FirstOrDefaultAsync().ConfigureAwait(false);
            if (account == null)
                return false;
            
            account.TransactionHistory.Add(transaction);
            await _dbContext.Transactions.AddAsync(transaction);
            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError("Unable to add transaction {TransactionId} for account {TransactionAccountNumber} due to: {ExMessage}",
                transaction.Id, transaction.AccountNumber, ex.Message);
            return false;
        }
        return true;
    }
}