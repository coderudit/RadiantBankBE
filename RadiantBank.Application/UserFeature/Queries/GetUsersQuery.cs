using MediatR;
using RadiantBank.Application.Services.Interfaces;
using RadiantBank.Domain.Models;

namespace RadiantBank.Application.UserFeature.Queries;

public class GetUsersQuery:IRequest<IEnumerable<User>>
{
    
}

public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, IEnumerable<User>>
{
    private readonly IUserService _userService;

    public GetUsersQueryHandler(IUserService userService)
    {
        _userService = userService;
    }
    public async Task<IEnumerable<User>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        return await _userService.GetUsersAsync().ConfigureAwait(false);
    }
}