using Auth.Identity.Domain.Dto;
using MediatR;

namespace Auth.Identity.Domain.Users.Commands;

public class CreateUserCommand : IRequest<UserResponse>
{
    public required string Name { get; set; }
    public required string Password  { get; set; }
}