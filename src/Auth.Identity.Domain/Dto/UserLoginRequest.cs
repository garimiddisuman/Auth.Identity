using MediatR;

namespace Auth.Identity.Domain.Dto;

public class UserLoginRequest:IRequest<UserLoginResponse>
{
    public string Name { get; set; } 
    public string Password { get; set; } 
}