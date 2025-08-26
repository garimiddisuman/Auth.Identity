using MediatR;

namespace Auth.Identity.Domain.Dto;

public class LoginRequest:IRequest<LoginResponse>
{
    public string Name { get; set; } 
    public string Password { get; set; } 
}