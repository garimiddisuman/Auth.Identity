using FluentValidation;

namespace Auth.Identity.Domain.Users.Commands;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(User.NameMaxLength).WithMessage($"Name must not exceed {User.NameMaxLength} characters.");
        
        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(User.PasswordMinLength).WithMessage($"Password must be at least {User.PasswordMinLength} characters long.")
            .MaximumLength(User.PasswordMaxLength).WithMessage($"Password must not exceed {User.PasswordMaxLength} characters.");
    }
}