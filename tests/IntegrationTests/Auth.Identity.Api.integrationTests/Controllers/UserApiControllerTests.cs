using System.Net;
using System.Net.Http.Json;
using Auth.Identity.Domain.Users.Commands;
using Auth.Identity.Infrastructure.Database;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Auth.Identity.Api.integrationTests.Controllers;

public class UserApiControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public UserApiControllerTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task CreateUser_ShouldCreateUserAndSaveInDb()
    {
        // Arrange
        var command = new CreateUserCommand { Name = "TestUser", Password = "TestPassword123" };

        // Act
        var response = await _client.PostAsJsonAsync("auth/register", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        using (var scope = _factory.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Name == "TestUser");
            user.Should().NotBeNull();
            user.Name.Should().Be("TestUser");
            user.Id.Should().BeGreaterThan(0);
        }
    }

    [Fact]
    public void CreateUser_ShouldReturnConflict_WhenUserAlreadyExists()
    {
        // Arrange
        var command = new CreateUserCommand { Name = "ExistingUser", Password = "TestPassword123" };

        // Act
        var response1 = _client.PostAsJsonAsync("auth/register", command).Result;
        var response2 = _client.PostAsJsonAsync("auth/register", command).Result;

        // Assert
        using (new AssertionScope())
        {
            response1.StatusCode.Should().Be(HttpStatusCode.Created);
            response2.StatusCode.Should().Be(HttpStatusCode.Conflict);

            var errorMessage = response2.Content.ReadAsStringAsync().Result;
            errorMessage.Should().Be("User already exists");
        }
    }

    [Theory]
    [InlineData("", "")]
    [InlineData("ValidName", "short")]
    [InlineData("ValidName", "long-------------------------")]
    public void CreateUser_ShouldReturnBadRequest_WhenCommandIsInvalid(string name, string password)
    {
        // Arrange
        var command = new CreateUserCommand { Name = name, Password = password };

        // Act
        var response = _client.PostAsJsonAsync("auth/register", command).Result;

        // Assert
        using (new AssertionScope())
        {
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}