using FluentValidation;

namespace RadiantBank.Application.UserFeature.Commands;

public class WithdrawFundsCommandValidator : AbstractValidator<WithdrawFundsCommand>
{
    public WithdrawFundsCommandValidator()
    {
        RuleFor(v => v.UserId)
            .NotEmpty()
            .NotNull()
            .WithMessage("Invalid user id.");
        
        RuleFor(v => v.AccountNumber)
            .NotEmpty()
            .NotNull()
            .WithMessage("Invalid account number.");
        
        RuleFor(v => v.Amount)
            .NotEmpty()
            .NotNull()
            .WithMessage("Invalid withdraw amount.");
    }
}