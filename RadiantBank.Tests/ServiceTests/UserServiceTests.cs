using Moq;
using NUnit.Framework;
using RadiantBank.Application.Services.Implementations;
using RadiantBank.Application.Services.Interfaces;
using RadiantBank.Domain.Models;
using RadiantBank.Domain.Repository;

namespace RadiantBank.Tests.ServiceTests;

[TestFixture]
public class UserServiceTests
{
    private IUserService _userService;
    private Mock<IUserRepository> _userRepositoryMock;

    [SetUp]
    public void Setup()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _userService = new UserService(_userRepositoryMock.Object);
    }

    [Test]
    public async Task GetUsersAsync_ReturnsListOfUsers()
    {
        // Arrange
        var users = new List<User>
        {
            new User
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Johnny Depp",
                Address = "1333 South Park Street, Halifax, NS",
                ContactNumber = "4562358907",
                EmailAddress = "johnnydepp@gmail.com"
            },
            new User
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Emily Depp",
                Address = "1133 South Park Street, Halifax, NS",
                ContactNumber = "45697731907",
                EmailAddress = "emilydepp@gmail.com"
            }
        };
        _userRepositoryMock
            .Setup(repository => repository.GetUsersAsync()).ReturnsAsync(users);

        // Act
        var result = await _userService.GetUsersAsync();

        // Assert
        Assert.AreEqual(users, result);
    }

    [Test]
    public async Task GetUserAsync_ValidId_ReturnsUser()
    {
        var id = Guid.NewGuid().ToString();
        // Arrange
        var user = new User
        {
            Id = id,
            Name = "Johnny Depp",
            Address = "1333 South Park Street, Halifax, NS",
            ContactNumber = "4562358907",
            EmailAddress = "johnnydepp@gmail.com"
        };
        _userRepositoryMock.Setup(repository => repository.GetUserByIdAsync(It.IsAny<string>())).ReturnsAsync(user);

        // Act
        var result = await _userService.GetUserByIdAsync(id);

        // Assert
        Assert.AreEqual(user, result);
    }

    [Test]
    public async Task CreateUserAsync_ValidUser_ReturnsNewUser()
    {
        // Arrange
        var newUser = new User
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Johnny Depp",
            Address = "1333 South Park Street, Halifax, NS",
            ContactNumber = "4562358907",
            EmailAddress = "johnnydepp@gmail.com"
        };
        _userRepositoryMock.Setup(repository => repository.AddUserAsync(It.IsAny<User>())).ReturnsAsync(true);

        // Act
        var result = await _userService.AddUserAsync(newUser);

        // Assert
        Assert.IsTrue(result);
    }
}