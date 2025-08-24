using Auth.Identity.Application.Exceptions;
using Auth.Identity.Domain.Users;
using Auth.Identity.Domain.Users.Commands;
using Auth.Identity.Infrastructure.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Auth.Identity.Application.Users;

public class CreateUserCommandHandler(IRepository<User> repository, PasswordHasher<CreateUserCommand> passwordHasher) : IRequestHandler<CreateUserCommand, User>
{
    public async Task<User> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        request.Password = passwordHasher.HashPassword(request, request.Password);
        var user = new User(request);
        
        var isUserExists = await repository.IsExistsAsync(user, cancellationToken);

        if (isUserExists)
        {
            throw new ObjectAlreadyExistsException("User already exists");
        }
        
        return await repository.InsertAsync(user, cancellationToken).ConfigureAwait(false);
    }
}