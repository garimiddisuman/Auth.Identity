using System.ComponentModel.DataAnnotations;
using Auth.Identity.Application.Exceptions;
using Auth.Identity.Domain.Users;
using Auth.Identity.Domain.Users.Commands;
using Auth.Identity.Infrastructure.Interfaces;
using Moq;
using Moq.AutoMock;
using Auth.Identity.Application.Users;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Shouldly;

public class CreateUserCommandHandlerTests
{
    private readonly AutoMocker _autoMocker;
    private readonly Mock<IRepository<User>> _repositoryMock;
    private readonly CreateUserCommandHandler _handler;

    public CreateUserCommandHandlerTests()
    {
        _autoMocker = new AutoMocker();
        _repositoryMock = _autoMocker.GetMock<IRepository<User>>();
        _handler = _autoMocker.CreateInstance<CreateUserCommandHandler>();
    }
    
    [Fact]
    public async Task WhenCommandIsValid_ShouldCreateUser()
    {
        // Arrange
        var originalPassword = "ValidPassword123";
        var command = new CreateUserCommand { Name = "ValidName", Password = originalPassword };
        var user = new User(command);
        
        var passwordHasher = new PasswordHasher<CreateUserCommand>();
        var hashedPassword = passwordHasher.HashPassword(command, command.Password);

        _autoMocker.GetMock<PasswordHasher<CreateUserCommand>>().Setup(p => p.HashPassword(command, command.Password)).Returns(hashedPassword);

        _repositoryMock.Setup(r => r.IsExistsAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        _repositoryMock.Setup(r => r.InsertAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
            .Callback<User, CancellationToken>((u, _) => user = u)
            .ReturnsAsync(() => user!);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);
        
        // Assert
        using (new AssertionScope())
        {
            result.ShouldNotBeNull();
            result.Name.ShouldBe(command.Name);
            result.PasswordHash.ShouldBe(hashedPassword);
        }
    }

    [Fact]
    public async Task Name_WhenUserNameAlreadyExists_ShouldThrowException()
    {
        // Arrange
        var command = new CreateUserCommand { Name = "ExistingUser", Password = "ValidPassword123" };

        _repositoryMock.Setup(r => r.IsExistsAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ObjectAlreadyExistsException>(() => _handler.Handle(command, CancellationToken.None));
        exception.StatusCode.ShouldBe(409);
    }
}