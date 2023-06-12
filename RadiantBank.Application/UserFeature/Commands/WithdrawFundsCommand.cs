using MediatR;
using RadiantBank.Application.DTOs;
using RadiantBank.Application.Services.Interfaces;

namespace RadiantBank.Application.UserFeature.Commands;

public class WithdrawFundsCommand : IRequest<FundsMessage>
{
    public string UserId { get; set; }
    public string AccountNumber { get; set; }
    public string Amount { get; set; }
}

public class WithdrawFundsCommandHandler : IRequestHandler<WithdrawFundsCommand, FundsMessage>
{
    private readonly IAccountService _accountService;

    public WithdrawFundsCommandHandler(IAccountService accountService)
    {
        _accountService = accountService;
    }
    public async Task<FundsMessage> Handle(WithdrawFundsCommand request, CancellationToken cancellationToken)
    {
        if (!decimal.TryParse(request.Amount, out var withdrawAmount))
            return new FundsMessage(false, "Unable to parse withdraw amount.");
        var isWithdrawn = await _accountService
            .WithdrawFromAccountAsync(request.UserId, request.AccountNumber, withdrawAmount)
            .ConfigureAwait(false);
        var message = $"Successfully deposited {withdrawAmount}";
        if (!isWithdrawn)
        {
            message = $"Unable to withdraw {withdrawAmount}. Deposit amount should be at least 1." +
                      $" Max amount to withdraw should not exceed 90% of total balance. " +
                      $" An account cannot have less than $100 at any time in an account.";
        }
        return new FundsMessage(isWithdrawn, message);
    }
}