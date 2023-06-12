using MediatR;
using RadiantBank.Application.DTOs;
using RadiantBank.Application.Services.Interfaces;

namespace RadiantBank.Application.UserFeature.Queries;

public class GetUserByIdQuery : IRequest<UserDTO>
{
    public string UserId { get; set; }
}

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserDTO?>
{
    private readonly IUserService _userService;
    public GetUserByIdQueryHandler(IUserService userService)
    {
        _userService = userService;
    }
    public async Task<UserDTO?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var response = await _userService.GetUserByIdAsync(request.UserId).ConfigureAwait(false);
        if (response == null)
            return null;

        var user = new UserDTO
        {
            Id = response.Id,
            Address = response.Address,
            ContactNumber = response.ContactNumber,
            EmailAddress = response.EmailAddress,
            Name = response.Name,
            Accounts = new List<string>()
        };
        foreach (var account in response.Accounts)
        {
            user.Accounts.Add(account.AccountNumber);
        }
        return user;
    }
}