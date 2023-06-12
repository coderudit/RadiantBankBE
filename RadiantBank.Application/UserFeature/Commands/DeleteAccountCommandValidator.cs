using FluentValidation;

namespace RadiantBank.Application.UserFeature.Commands;

public class DeleteAccountCommandValidator : AbstractValidator<DeleteAccountCommand>
{
    public DeleteAccountCommandValidator()
    {
        RuleFor(v => v.UserId)
            .NotEmpty()
            .NotNull()
            .WithMessage("Invalid user id.");
        
        RuleFor(v => v.AccountNumber)
            .NotEmpty()
            .NotNull()
            .WithMessage("Invalid account number.");
    }
}