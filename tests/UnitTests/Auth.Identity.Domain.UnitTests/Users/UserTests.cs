using Auth.Identity.Domain.Users;
using Auth.Identity.Domain.Users.Commands;
using Bogus;
using FluentAssertions;
using FluentAssertions.Execution;

namespace Auth.Identity.Domain.UnitTests.Users;

public class UserTests
{
    [Fact]
    public void Constructor_WhenCreateProductCommandIsValid_ShouldCreateProductWithAppropriateProperties()
    {
        // Arrange
        var command = new Faker<CreateUserCommand>().Generate();

        // Act
        var user = new User(command);

        // Assert
        using (new AssertionScope())
        {
            user.Name.Should().Be(command.Name);
            user.PasswordHash.Should().Be(command.Password);
        }
    }
}
