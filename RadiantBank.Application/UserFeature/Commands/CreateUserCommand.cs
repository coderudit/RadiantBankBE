using MediatR;
using RadiantBank.Application.Services.Interfaces;
using RadiantBank.Domain.Models;

namespace RadiantBank.Application.UserFeature.Commands;

public record CreateUserCommand : IRequest<string>
{
    public string Name { get; set; }
    public string Address { get; set; }
    public string ContactNumber { get; set; }
    public string EmailAddress { get; set; }
}

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, string>
{
    private readonly IUserService _userService;

    public CreateUserCommandHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<string> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var user = new User
        {
            Id = Guid.NewGuid().ToString(),
            Name = request.Name,
            Address = request.Address,
            ContactNumber = request.ContactNumber,
            EmailAddress = request.EmailAddress
        };
        await _userService.AddUserAsync(user).ConfigureAwait(false);

        return user.Id;
    }
}