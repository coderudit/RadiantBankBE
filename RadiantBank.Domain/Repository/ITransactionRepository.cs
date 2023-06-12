using RadiantBank.Domain.Models;

namespace RadiantBank.Domain.Repository;

public interface ITransactionRepository
{
    Task<bool> AddTransactionAsync(Transaction transaction);
}