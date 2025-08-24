using Auth.Identity.Domain.Users;
using Auth.Identity.Domain.Users.Commands;
using FluentAssertions;
using FluentAssertions.Execution;

namespace Auth.Identity.Domain.UnitTests.Users.Commands;

public class CreateUserCommandTests
{
    [Fact]
    public void Name_ShouldThrowError_WhenNameIsEmpty()
    {
        // Arrange
        var command = new CreateUserCommand { Name = string.Empty, Password = "ValidPassword123" };

        var validator = new CreateUserCommandValidator();

        // Act
        var result = validator.Validate(command);

        // Assert
        using (new AssertionScope())
        {
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == nameof(CreateUserCommand.Name));
        }
    }

    [Fact]
    public void Name_ShouldThrowError_WhenNameExceedsMaxLength()
    {
        // Arrange
        var command = new CreateUserCommand { Name = new string('a', User.NameMaxLength + 20), Password = "ValidPassword123" };

        var validator = new CreateUserCommandValidator();

        // Act
        var result = validator.Validate(command);

        // Assert
        using (new AssertionScope())
        {
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == nameof(CreateUserCommand.Name));
        }
    }

    [Fact]
    public void Password_ShouldThrowError_WhenPasswordIsEmpty()
    {
        // Arrange
        var command = new CreateUserCommand { Name = "ValidName", Password = string.Empty };

        var validator = new CreateUserCommandValidator();

        // Act
        var result = validator.Validate(command);

        // Assert
        using (new AssertionScope())
        {
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == nameof(CreateUserCommand.Password));
        }
    }

    [Fact]
    public void Password_ShouldThrowError_WhenPasswordIsTooShort()
    {
        // Arrange
        var command = new CreateUserCommand { Name = "ValidName", Password = new string('a', User.PasswordMinLength - 2) };

        var validator = new CreateUserCommandValidator();

        // Act
        var result = validator.Validate(command);

        // Assert
        using (new AssertionScope())
        {
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == nameof(CreateUserCommand.Password));
        }
    }

    [Fact]
    public void Password_ShouldThrowError_WhenPasswordExceedsMaxLength()
    {
        // Arrange
        var command = new CreateUserCommand { Name = "ValidName", Password = new string('a', User.PasswordMaxLength + 5) };

        var validator = new CreateUserCommandValidator();

        // Act
        var result = validator.Validate(command);

        // Assert
        using (new AssertionScope())
        {
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == nameof(CreateUserCommand.Password));
        }
    }

    [Fact]
    public void Validate_ShouldPass_WhenCommandIsValid()
    {
        // Arrange
        var command = new CreateUserCommand { Name = "ValidName", Password = "ValidPassword123" };

        var validator = new CreateUserCommandValidator();

        // Act
        var result = validator.Validate(command);

        // Assert
        using (new AssertionScope())
        {
            result.IsValid.Should().BeTrue();
        }
    }
}