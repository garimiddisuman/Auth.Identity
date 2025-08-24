using Auth.Identity.Domain.Users;

namespace Auth.Identity.Infrastructure.Interfaces;

public interface IRepository<T>
{
    Task<T> InsertAsync(T record, CancellationToken cancellationToken);
    // Task<User> UpdateAsync(User user, CancellationToken cancellationToken);
    // Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    // Task DeleteAsync(Guid id, CancellationToken cancellationToken);
    Task<Boolean> IsExistsAsync(T record, CancellationToken cancellationToken);
}