using MediatR;

namespace Auth.Identity.Domain.Dto;

public class UserLoginRequest : IRequest<UserLoginResponse>
{
    public required string Name { get; set; } 
    public required string Password { get; set; } 
}