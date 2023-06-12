using FluentValidation;

namespace RadiantBank.Application.UserFeature.Commands;

public class CreateUserCommandValidator:AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotNull()
            .NotEmpty();
        
        RuleFor(x => x.Address)
            .NotNull()
            .NotEmpty();
        RuleFor(x => x.EmailAddress)
            .NotNull()
            .NotEmpty();
        
        RuleFor(x => x.ContactNumber)
            .NotNull()
            .NotEmpty();

    }
}