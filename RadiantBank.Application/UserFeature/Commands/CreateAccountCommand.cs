using MediatR;
using RadiantBank.Application.Services.Interfaces;
using RadiantBank.Domain.Models;

namespace RadiantBank.Application.UserFeature.Commands;

public record CreateAccountCommand : IRequest<string?>
{
    public string UserId { get; set; }
    public decimal InitialBalance { get; set; }

}

public class CreateAccountCommandHandler : IRequestHandler<CreateAccountCommand, string?>
{
    private readonly IAccountService _accountService;
    public CreateAccountCommandHandler(IAccountService accountService)
    {
        _accountService = accountService;
    }
    
    public async Task<string?> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
    {
        var account = new Account
        {
            AccountNumber = Guid.NewGuid().ToString(),
            OpenDate = DateTime.Now,
            IsActive = true,
            TotalBalance = 0,
            UserId = request.UserId
        };
        if (!await _accountService.AddAccountAsync(account, request.InitialBalance))
        {
            return null;
        }

        return account.AccountNumber;
    }
}