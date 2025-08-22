using MediatR;

namespace Auth.Identity.Domain.Users.Commands;

public class CreateUserCommand : IRequest<User>
{
    public required string Name { get; set; }
    public required string Password  { get; set; }
}