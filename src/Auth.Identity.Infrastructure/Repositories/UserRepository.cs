using Auth.Identity.Domain.Users;
using Auth.Identity.Infrastructure.Database;
using Auth.Identity.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Auth.Identity.Infrastructure.Repositories;

public class UserRepository(AppDbContext context) : IRepository<User>
{
    public async Task<User> InsertAsync(User user, CancellationToken cancellationToken)
    {
        context.Users.Add(user);
        await context.SaveChangesAsync(cancellationToken);
        return user;
    }
    
    public async Task<Boolean> IsExistsAsync(User user, CancellationToken cancellationToken)
    {
        return await context.Users.AnyAsync(u => u.Name == user.Name, cancellationToken);
    }

    // public async Task<User> UpdateAsync(User user, CancellationToken cancellationToken)
    // {
    //     context.Users.Update(user);
    //     await context.SaveChangesAsync(cancellationToken);
    //     return user;
    // }
    //
    // public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    // {
    //     return await context.Users.FindAsync(new object[] { id }, cancellationToken);
    // }
    //
    // public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    // {
    //     var user = await context.Users.FindAsync(new object[] { id }, cancellationToken);
    //     if (user != null)
    //     {
    //         context.Users.Remove(user);
    //         await context.SaveChangesAsync(cancellationToken);
    //     }
    // }
}