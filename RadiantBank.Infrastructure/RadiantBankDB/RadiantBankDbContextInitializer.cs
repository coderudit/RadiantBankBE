using Microsoft.Extensions.Logging;
using RadiantBank.Domain.Models;
using RadiantBank.Domain.Repository;

namespace RadiantBank.Infrastructure.RadiantBankDB;

public class RadiantBankDbContextInitializer
{
    private readonly ILogger<RadiantBankDbContextInitializer> _logger;
    private readonly RadiantBankDbContext _dbContext;
    private readonly IUserRepository _userRepository;
    private readonly IAccountRepository _accountRepository;
    private readonly ITransactionRepository _transactionRepository;
    
    public RadiantBankDbContextInitializer(ILogger<RadiantBankDbContextInitializer> logger,
        RadiantBankDbContext context,
        IUserRepository userRepository,
        IAccountRepository accountRepository,
        ITransactionRepository transactionRepository)
    {
        _logger = logger;
        _dbContext = context;
        _userRepository = userRepository;
        _accountRepository = accountRepository;
        _transactionRepository = transactionRepository;
    }
    
    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    private async Task TrySeedAsync()
    {
        List<Task> dbSetupTasks = new();
        var user1 = new User
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Johnny Depp",
            Address = "1333 South Park Street, Halifax, NS",
            ContactNumber = "4562358907",
            EmailAddress = "johnnydepp@gmail.com"
        };
        dbSetupTasks.Add(_userRepository.AddUserAsync(user1));
        
        var account1 = new Account
        {
            AccountNumber = Guid.NewGuid().ToString(),
            OpenDate = DateTime.Now,
            IsActive = true,
            TotalBalance = 1000,
            UserId = user1.Id,
            User = user1
        };
        dbSetupTasks.Add(_accountRepository.AddAccountAsync(account1));
        
        var transaction1 = new Transaction
        {
            Id = Guid.NewGuid().ToString(),
            TypeId = 1,
            Time = DateTime.Now,
            PreviousBalance = 0,
            CurrentBalance = 1000,
            Comment = $"Initializing the account with 1000 at {DateTime.Now}",
            AccountNumber = account1.AccountNumber
        };
        dbSetupTasks.Add(_transactionRepository.AddTransactionAsync(transaction1));
        
        var transaction2 = new Transaction
        {
            Id = Guid.NewGuid().ToString(),
            TypeId = 1,
            Time = DateTime.Now,
            PreviousBalance = 0,
            CurrentBalance = 700,
            Comment = $"Depositing 700 in the account at {DateTime.Now}",
            AccountNumber = account1.AccountNumber
        };
        dbSetupTasks.Add(_transactionRepository.AddTransactionAsync(transaction2));
        
        var transaction3 = new Transaction
        {
            Id = Guid.NewGuid().ToString(),
            TypeId = 2,
            Time = DateTime.Now,
            PreviousBalance = 0,
            CurrentBalance = 700,
            Comment = $"Withdrawing 300 from the account at {DateTime.Now}",
            AccountNumber = account1.AccountNumber
        };
        dbSetupTasks.Add(_transactionRepository.AddTransactionAsync(transaction3));

        try
        {
            await Task.WhenAll(dbSetupTasks);
        }
        catch (AggregateException ex)
        {
            Console.WriteLine("Exception while setting up the Radiant Bank DB. ");
            foreach (var innerException in ex.InnerExceptions)
            {
                Console.WriteLine(innerException.Message);
            }
        }
    }
}