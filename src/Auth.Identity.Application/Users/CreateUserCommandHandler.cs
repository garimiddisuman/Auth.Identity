using Auth.Identity.Application.Exceptions;
using Auth.Identity.Domain.Dto;
using Auth.Identity.Domain.Users;
using Auth.Identity.Domain.Users.Commands;
using Auth.Identity.Infrastructure.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Auth.Identity.Application.Users;

public class CreateUserCommandHandler(IRepository<User> repository, PasswordHasher<CreateUserCommand> passwordHasher) : IRequestHandler<CreateUserCommand, UserResponse>
{
    public async Task<UserResponse> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        request.Password = passwordHasher.HashPassword(request, request.Password);
        var user = new User(request);
        
        var isUserExists = await repository.IsExistsAsync(user, cancellationToken);

        if (isUserExists)
        {
            throw new ObjectAlreadyExistsException("User already exists");
        }
        
        var insertedUser = await repository.InsertAsync(user, cancellationToken).ConfigureAwait(false);
        return new UserResponse() {Id = insertedUser.Id, Name = insertedUser.Name, CreateAt = insertedUser.CreateAt};
    }
}
