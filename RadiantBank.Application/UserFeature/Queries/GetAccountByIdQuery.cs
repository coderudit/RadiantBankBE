using MediatR;
using RadiantBank.Application.DTOs;
using RadiantBank.Application.Services.Implementations;
using RadiantBank.Application.Services.Interfaces;
using RadiantBank.Domain.Enums;
using RadiantBank.Domain.Models;
using RadiantBank.Domain.Repository;

namespace RadiantBank.Application.UserFeature.Queries;

public class GetAccountByIdQuery : IRequest<AccountDTO?>
{
    public string UserId { get; set; }
    public string AccountNumber { get; set; }
}

public class GetAccountByIdQueryHandler : IRequestHandler<GetAccountByIdQuery, AccountDTO?>
{
    private readonly IAccountService _accountService;
    
    public GetAccountByIdQueryHandler(IAccountService accountService)
    {
        _accountService = accountService;
    }
    
    public async Task<AccountDTO?> Handle(GetAccountByIdQuery request, CancellationToken cancellationToken)
    {
        var response = await _accountService.GetAccountByIdAsync(request.UserId, request.AccountNumber).ConfigureAwait(false);
        if (response == null)
        {
            return null;
        }

        var account = new AccountDTO()
        {
            AccountNumber = response.AccountNumber,
            IsActive = response.IsActive ? 1: 0,
            OpenDate = response.OpenDate,
            TotalBalance = response.TotalBalance,
            TransactionHistory = new List<TransactionDTO>()
        };

        foreach (var transaction in response.TransactionHistory)
        {
            account.TransactionHistory.Add(new TransactionDTO
            {
                Id = transaction.Id,
                CurrentBalance = transaction.CurrentBalance,
                PreviousBalance = transaction.PreviousBalance,
                Time = transaction.Time,
                TransactionType = ((TransactionType)transaction.TypeId).ToString()
            });
        }
        return account;
    }
}