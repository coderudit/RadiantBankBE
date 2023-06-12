using MediatR;
using Microsoft.EntityFrameworkCore;
using RadiantBank.Domain.Models;

namespace RadiantBank.Infrastructure.RadiantBankDB;

public class RadiantBankDbContext : DbContext, IRadiantBankDbContext
{
    private readonly IMediator _mediator;
    public RadiantBankDbContext(
        DbContextOptions<RadiantBankDbContext> options,
        IMediator mediator)
    {
        _mediator = mediator;
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseInMemoryDatabase(databaseName: "RadiantBankDB");
    }

    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<Account> Accounts { get; set; }
    public virtual DbSet<Transaction> Transactions { get; set; }

    public async Task<int> SaveChangesAsync()
    {
        return await base.SaveChangesAsync();
    }
}