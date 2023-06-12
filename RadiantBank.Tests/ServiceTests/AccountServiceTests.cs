using Moq;
using NUnit.Framework;
using RadiantBank.Application.Services.Implementations;
using RadiantBank.Application.Services.Interfaces;
using RadiantBank.Domain.Models;
using RadiantBank.Domain.Repository;

namespace RadiantBank.Tests.ServiceTests;

[TestFixture]
public class AccountServiceTests
{
    private IAccountService _accountService;
    private Mock<IUserRepository> _userRepositoryMock;
    private Mock<IAccountRepository> _accountRepositoryMock;
    private Mock<ITransactionRepository> _transactionRepositoryMock;

    [SetUp]
    public void Setup()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _accountRepositoryMock = new Mock<IAccountRepository>();
        _transactionRepositoryMock = new Mock<ITransactionRepository>();
        _accountService = new AccountService(_userRepositoryMock.Object, 
            _accountRepositoryMock.Object, _transactionRepositoryMock.Object);
    }

    [Test]
    public async Task GetAccountsAsync_ReturnsListOfAccounts()
    {
        // Arrange
        var userId = Guid.NewGuid().ToString();
        var accounts = new List<Account>
        {
            new Account
            {
                AccountNumber = Guid.NewGuid().ToString(),
                TotalBalance = 1000,
                IsActive = true,
                OpenDate = DateTime.Now,
                UserId = userId
            },
            new Account
            {
                AccountNumber = Guid.NewGuid().ToString(),
                TotalBalance = 2000,
                IsActive = true,
                OpenDate = DateTime.Now,
                UserId = userId
            }
        };
        _accountRepositoryMock.Setup(repository => repository.GetAccountsAsync(userId)).ReturnsAsync(accounts);

        // Act
        var result = await _accountService.GetAccountsAsync(userId);

        // Assert
        Assert.AreEqual(accounts, result);
    }

    [Test]
    public async Task GetAccountAsync_ValidAccountNumber_ReturnsAccount()
    {
        // Arrange
        var userId = Guid.NewGuid().ToString();
        var accountNumber = Guid.NewGuid().ToString();
        var account = new Account
        {
            AccountNumber = accountNumber,
            TotalBalance = 2000,
            IsActive = true,
            OpenDate = DateTime.Now,
            UserId = userId
        };
        _accountRepositoryMock.Setup(repository => repository.GetAccountByIdAsync(userId, accountNumber))
            .ReturnsAsync(account);

        // Act
        var result = await _accountService.GetAccountByIdAsync(userId, accountNumber);

        // Assert
        Assert.AreEqual(account, result);
    }

    [Test]
    public async Task CreateAccountAsync_ValidAccount_ReturnsNewAccount()
    {
        // Arrange
        var userId = Guid.NewGuid().ToString();
        var accountNumber = Guid.NewGuid().ToString();
        var account = new Account {
            AccountNumber = accountNumber,
            TotalBalance = 2000,
            IsActive = true,
            OpenDate = DateTime.Now,
            UserId = userId 
        };
        _accountRepositoryMock.Setup(repository => repository.AddAccountAsync(It.IsAny<Account>()))
            .ReturnsAsync(true);

        // Act
        var result = await _accountService.AddAccountAsync(account, account.TotalBalance);

        // Assert
        Assert.IsTrue(result);
    }
    
    [Test]
        public async Task DepositToAccountAsync_ValidData_ReturnsTrue()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            var accountNumber = Guid.NewGuid().ToString();
            var account = new Account {
                AccountNumber = accountNumber,
                TotalBalance = 2000,
                IsActive = true,
                OpenDate = DateTime.Now,
                UserId = userId 
            };
            
            var amount = 100;
            _accountRepositoryMock
                .Setup(repository => repository.GetAccountByIdAsync(userId, accountNumber))
                .ReturnsAsync(account);
    
            // Act
            var result = await _accountService.DepositToAccountAsync(userId, accountNumber, amount);
    
            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task DepositToAccountAsync_InvalidAccountNumber_ReturnsFalse()
        {
            // Arrange
            var userId = "1";
            var accountNumber = Guid.NewGuid().ToString();
            
            var account = new Account {
                AccountNumber = accountNumber,
                TotalBalance = 2000,
                IsActive = true,
                OpenDate = DateTime.Now,
                UserId = userId 
            };
            var amount = 100;
            _accountRepositoryMock
                .Setup(repository => repository.GetAccountByIdAsync(userId, accountNumber))
                .ReturnsAsync(account);
            
            // Act
            var result = await _accountService.DepositToAccountAsync(userId, Guid.NewGuid().ToString(), amount);
        
            // Assert
            Assert.IsFalse(result);
        }
        
        [Test]
        public async Task DepositToAccountAsync_InvalidData_ReturnsFalse()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            var accountNumber = Guid.NewGuid().ToString();
            
            var account = new Account {
                AccountNumber = accountNumber,
                TotalBalance = 2000,
                IsActive = true,
                OpenDate = DateTime.Now,
                UserId = userId 
            };
            var amount1 = -100;
            var amount2 = 10001;
            _accountRepositoryMock
                .Setup(repository => repository.GetAccountByIdAsync(userId, accountNumber))
                .ReturnsAsync(account);
            
            // Act
            var result1 = await _accountService.DepositToAccountAsync(userId, accountNumber, amount1);
            var result2 = await _accountService.DepositToAccountAsync(userId, accountNumber, amount2);
            
            // Assert
            Assert.IsFalse(result1);
            Assert.IsFalse(result2);
        }
        
        [Test]
        public async Task WithdrawFromAccountAsync_ValidData_ReturnsTrue()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            var accountNumber = Guid.NewGuid().ToString();
            var account = new Account {
                AccountNumber = accountNumber,
                TotalBalance = 2000,
                IsActive = true,
                OpenDate = DateTime.Now,
                UserId = userId 
            };
            
            var amount = 100;
            _accountRepositoryMock
                .Setup(repository => repository.GetAccountByIdAsync(userId, accountNumber))
                .ReturnsAsync(account);
            
            // Act
            var result = await _accountService.WithdrawFromAccountAsync(userId, accountNumber, amount);
        
            // Assert
            Assert.IsTrue(result);
        }
        
        [Test]
        public async Task WithdrawFromAccountAsync_InvalidAccountNumber_ReturnsFalse()
        {
            var userId = "1";
            var accountNumber = Guid.NewGuid().ToString();
            
            var account = new Account {
                AccountNumber = accountNumber,
                TotalBalance = 2000,
                IsActive = true,
                OpenDate = DateTime.Now,
                UserId = userId 
            };
            var amount = 100;
            _accountRepositoryMock
                .Setup(repository => repository.GetAccountByIdAsync(userId, accountNumber))
                .ReturnsAsync(account);
        
            // Act
            var result = await _accountService.WithdrawFromAccountAsync(userId, Guid.NewGuid().ToString(), amount);
        
            // Assert
            Assert.IsFalse(result);
        }
        
        [Test]
        public async Task WithdrawFromAccountAsync_InsufficientBalance_ReturnsFalse()
        {
            // Arrange
            var userId = "1";
            var accountNumber = Guid.NewGuid().ToString();
            
            var account = new Account {
                AccountNumber = accountNumber,
                TotalBalance = 2000,
                IsActive = true,
                OpenDate = DateTime.Now,
                UserId = userId 
            };
            var amount = 2000;
            _accountRepositoryMock
                .Setup(repository => repository.GetAccountByIdAsync(userId, accountNumber))
                .ReturnsAsync(account);
        
            // Act
            var result = await _accountService.WithdrawFromAccountAsync(userId, Guid.NewGuid().ToString(), amount);
        
            // Assert
            Assert.IsFalse(result);
        }
        
        [Test]
        public async Task WithdrawFromAccountAsync_InvalidData_ReturnsFalse()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            var accountNumber = Guid.NewGuid().ToString();
            
            var account = new Account {
                AccountNumber = accountNumber,
                TotalBalance = 2000,
                IsActive = true,
                OpenDate = DateTime.Now,
                UserId = userId 
            };
            var negativeAmount = -100;
            var moreThan90PercentAmount = 1801;
            var lessThan100AfterWithdrawlAmount = 1901;
            _accountRepositoryMock
                .Setup(repository => repository.GetAccountByIdAsync(userId, accountNumber))
                .ReturnsAsync(account);
            
            // Act
            var result1 = await _accountService.WithdrawFromAccountAsync(userId, accountNumber, negativeAmount);
            var result2 = await _accountService.WithdrawFromAccountAsync(userId, accountNumber, moreThan90PercentAmount);
            var result3 = await _accountService.WithdrawFromAccountAsync(userId, accountNumber, lessThan100AfterWithdrawlAmount);
            
            // Assert
            Assert.IsFalse(result1);
            Assert.IsFalse(result2);
            Assert.IsFalse(result3);
        }
}