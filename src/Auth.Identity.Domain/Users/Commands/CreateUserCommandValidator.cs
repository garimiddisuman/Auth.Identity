using FluentValidation;

namespace Auth.Identity.Domain.Users.Commands;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(User.NameMaxLength);
        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(User.PasswordMinLength)
            .MaximumLength(User.PasswordMaxLength);
    }
}