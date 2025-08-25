using Auth.Identity.Application.Exceptions;
using Auth.Identity.Domain.Dto;
using Auth.Identity.Domain.Users;
using Auth.Identity.Infrastructure.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Auth.Identity.Application.Users;

public class LoginService : IRequestHandler<UserLoginRequest, LoginResponse>
{
    private readonly IRepository<User> _usersRepo;
    private readonly PasswordHasher<User> _passwordHasher;
    private readonly TokenService _tokenService;

    public LoginService(IRepository<User> usersRepo, PasswordHasher<User> passwordHasher, TokenService tokenService)
    {
        _usersRepo = usersRepo;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
    }

    public async Task<LoginResponse> Handle(UserLoginRequest request, CancellationToken cancellationToken)
    {
        var user = await _usersRepo.GetByNameAsync(request.Name, cancellationToken);
        if (user == null)
        {
            throw new ObjectNotFoundException($"{request.Name} not found");
        }

        ValidatePassword(user, request.Password, _passwordHasher);
        var token = _tokenService.GenerateToken(user);

        return new LoginResponse() { Token = token };
    }

    private void ValidatePassword(User user, string password, PasswordHasher<User> passwordHasher)
    {
        var result = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
        if (result == PasswordVerificationResult.Failed)
        {
            throw new InvalidPasswordException("Invalid password");
        }
    }
}