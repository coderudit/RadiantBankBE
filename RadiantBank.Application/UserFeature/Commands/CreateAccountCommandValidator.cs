using FluentValidation;

namespace RadiantBank.Application.UserFeature.Commands;

public class CreateAccountCommandValidator : AbstractValidator<CreateAccountCommand>
{
    public CreateAccountCommandValidator()
    {
        RuleFor(v => v.UserId)
            .NotEmpty()
            .NotNull()
            .WithMessage("Invalid user id.");
        
        RuleFor(v => v.InitialBalance)
            .NotEmpty()
            .NotNull()
            .WithMessage("Invalid initial balance.");
    }
}