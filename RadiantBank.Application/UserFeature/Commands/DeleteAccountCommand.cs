using MediatR;
using RadiantBank.Application.Services.Interfaces;

namespace RadiantBank.Application.UserFeature.Commands;

public class DeleteAccountCommand : IRequest<bool>
{
    public string UserId { get; set; }
    public string AccountNumber { get; set; }
}

public class DeleteAccountCommandHandler : IRequestHandler<DeleteAccountCommand, bool>
{
    private readonly IAccountService _accountService;
    public DeleteAccountCommandHandler(IAccountService accountService)
    {
        _accountService = accountService;
    }
    
    public async Task<bool> Handle(DeleteAccountCommand request, CancellationToken cancellationToken)
    {
        return await _accountService.RemoveAccountAsync(request.UserId, request.AccountNumber).ConfigureAwait(false);
    }
}
