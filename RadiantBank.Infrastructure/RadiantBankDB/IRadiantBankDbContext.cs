using Microsoft.EntityFrameworkCore;
using RadiantBank.Domain.Models;

namespace RadiantBank.Infrastructure.RadiantBankDB;

public interface IRadiantBankDbContext
{
    DbSet<User> Users { get; set; }
    DbSet<Account> Accounts { get; set; }
    DbSet<Transaction> Transactions { get; set; }
    Task<int> SaveChangesAsync();
}