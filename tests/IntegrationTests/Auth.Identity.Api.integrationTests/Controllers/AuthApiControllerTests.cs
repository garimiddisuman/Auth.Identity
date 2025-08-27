using System.Net;
using System.Net.Http.Json;
using Auth.Identity.Domain.Users.Commands;
using Auth.Identity.Infrastructure.Database;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;

namespace Auth.Identity.Api.integrationTests.Controllers;

public class AuthApiControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public AuthApiControllerTests(CustomWebApplicationFactory factory)
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

    [Fact]
    public async Task LoginUser_ShouldReturnOk_WhenCredentialsAreValid()
    {
        // Arrange
        var command = new CreateUserCommand { Name = "LoginUser", Password = "TestPassword123" };
        await _client.PostAsJsonAsync("auth/register", command);

        var loginRequest = new { Name = "LoginUser", Password = "TestPassword123" };

        // Act
        var response = await _client.PostAsJsonAsync("auth/login", loginRequest);
        var setCookies = response.Headers.GetValues("Set-Cookie");
        var jwtCookieString = setCookies.FirstOrDefault(c => c.StartsWith("jwt="));
        var jwtCookie = SetCookieHeaderValue.Parse(jwtCookieString);

        // Assert
        using (new AssertionScope())
        {
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            jwtCookie.Should().NotBeNull("because a successful login should set a jwt cookie");
            jwtCookie!.Name.ToString().Should().Be("jwt", "cookie must be named jwt");
            jwtCookie.HttpOnly.Should().BeTrue("cookie must be HttpOnly");
            jwtCookie.Secure.Should().BeTrue("cookie must be Secure");
            jwtCookie.SameSite.Should().Be(SameSiteMode.Strict, "cookie must have SameSite=Strict");
            jwtCookie.Expires.Should().NotBeNull("cookie must have an expiration date");
        }
    }

    [Fact]
    public async Task LoginUser_ShouldReturnUnauthorized_WhenCredentialsAreInvalid()
    {
        // Arrange
        var command = new CreateUserCommand { Name = "AnotherUser", Password = "TestPassword123" };
        await _client.PostAsJsonAsync("auth/register", command);

        var loginRequest = new { Name = "AnotherUser", Password = "WrongPassword" };

        // Act
        var response = await _client.PostAsJsonAsync("auth/login", loginRequest);

        // Assert
        using (new AssertionScope())
        {
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }
    }

    [Fact]
    public async Task LoginUser_ShouldReturnObjectNotFound_WhenUserDoesNotExist()
    {
        // Arrange
        var loginRequest = new { Name = "AnotherUser-404", Password = "WrongPassword" };

        // Act
        var response = await _client.PostAsJsonAsync("auth/login", loginRequest);

        // Assert
        using (new AssertionScope())
        {
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }

    [Fact]
    public async Task MeEndpoint_ReturnsClaims_WhenAuthorized()
    {
        // Arrange
        var command = new CreateUserCommand { Name = "MeUser", Password = "TestPassword123" };
        await _client.PostAsJsonAsync("auth/register", command);

        var loginRequest = new { Name = "MeUser", Password = "TestPassword123" };
        var loginResponse = await _client.PostAsJsonAsync("auth/login", loginRequest);

        Assert.True(loginResponse.Headers.TryGetValues("Set-Cookie", out var setCookies));
        var jwtCookie = setCookies.FirstOrDefault(c => c.StartsWith("jwt="));
        Assert.NotNull(jwtCookie);

        var token = jwtCookie.Split(';')[0].Replace("jwt=", "");

        var request = new HttpRequestMessage(HttpMethod.Get, "auth/me");
        request.Headers.Add("Cookie", $"jwt={token}");

        // Act
        var meResponse = await _client.SendAsync(request);

        // Assert
        using (new AssertionScope())
        {
            meResponse.EnsureSuccessStatusCode();
        }
    }


    [Fact]
    public async Task Me_ShouldReturnUnauthorized_WhenJwtIsInvalid()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "auth/me");
        request.Headers.Add("Cookie", $"jwt=invalid.JWT.Token");

        // Act
        var response = await _client.SendAsync(request);

        // Assert
        using (new AssertionScope())
        {
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }
    }

    [Fact]
    public async Task Me_ShouldReturnUnauthorized_WhenNoJwtIsProvided()
    {
        // Act
        var response = await _client.GetAsync("auth/me");

        // Assert
        using (new AssertionScope())
        {
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }
    }

    [Fact]
    public async Task LogoutUser_ShouldRemoveJwtCookie_WhenCalled()
    {
        // Arrange: First log in to get a cookie
        var command = new CreateUserCommand { Name = "LogoutUser", Password = "TestPassword123" };
        await _client.PostAsJsonAsync("auth/register", command);
        await _client.PostAsJsonAsync("auth/login", new { Name = "LogoutUser", Password = "TestPassword123" });

        // Act: call logout
        var response = await _client.PostAsync("auth/logout", null);
        var setCookies = response.Headers.GetValues("Set-Cookie");
        var jwtCookieString = setCookies.FirstOrDefault(c => c.StartsWith("jwt="));
        var jwtCookie = SetCookieHeaderValue.Parse(jwtCookieString);

        // Assert
        using (new AssertionScope())
        {
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            jwtCookie.Should().NotBeNull("because logout should clear the jwt cookie");
            jwtCookie!.Name.ToString().Should().Be("jwt", "cookie must still be named jwt");
            jwtCookie.HttpOnly.Should().BeTrue("cookie must remain HttpOnly for security");
            jwtCookie.Secure.Should().BeTrue("cookie must remain Secure");
            jwtCookie.SameSite.Should().Be(SameSiteMode.Strict, "cookie must keep SameSite=Strict");
            jwtCookie.Expires.Should().NotBeNull("cookie must be expired to be removed");
            jwtCookieString.Should().Contain("path=/", "cookie path must be root to override previous cookie");
            jwtCookie.Expires.Should().BeBefore(DateTimeOffset.UtcNow, "logout must expire the cookie so the browser deletes it");
        }
    }
}