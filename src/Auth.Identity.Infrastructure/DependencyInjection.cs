using Auth.Identity.Domain.Users;
using Auth.Identity.Infrastructure.Database;
using Auth.Identity.Infrastructure.Interfaces;
using Auth.Identity.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Auth.Identity.Infrastructure;

public static class DependencyInjection
{
    public static void AddDb(this IServiceCollection services, string connectionString)
    {
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new ArgumentNullException(nameof(connectionString));
        }

        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseNpgsql(connectionString);
        });
    }
    
    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IRepository<User>, UserRepository>();
    }
}