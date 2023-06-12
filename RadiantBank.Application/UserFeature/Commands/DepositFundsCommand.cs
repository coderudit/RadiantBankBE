using MediatR;
using RadiantBank.Application.DTOs;
using RadiantBank.Application.Services.Interfaces;

namespace RadiantBank.Application.UserFeature.Commands;

public class DepositFundsCommand: IRequest<FundsMessage>
{
    public string UserId { get; set; }
    public string AccountNumber { get; set; }
    public string Amount { get; set; }
}

public class DepositFundsCommandHandler : IRequestHandler<DepositFundsCommand, FundsMessage>
{
    private readonly IAccountService _accountService;

    public DepositFundsCommandHandler(IAccountService accountService)
    {
        _accountService = accountService;
    }
    public async Task<FundsMessage> Handle(DepositFundsCommand request, CancellationToken cancellationToken)
    {
        if (!decimal.TryParse(request.Amount, out var depositAmount))
            return new FundsMessage (false, "Unable to parse deposit amount.");
        var isDeposited = await _accountService
            .DepositToAccountAsync(request.UserId, request.AccountNumber, depositAmount)
            .ConfigureAwait(false);
        var message = $"Successfully deposited {depositAmount}";
        if (!isDeposited)
        {
            message = $"Unable to deposit {depositAmount}. Deposit amount should in range 1-10000.";
        }
        return new FundsMessage(isDeposited, message);
    }
}