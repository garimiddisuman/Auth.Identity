// using Microsoft.EntityFrameworkCore;
// using Microsoft.EntityFrameworkCore.Design;
//
// namespace Auth.Identity.Infrastructure.Database;
//
// public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
// {
//     public AppDbContext CreateDbContext(string[] args)
//     {
//         var builder = new DbContextOptionsBuilder<AppDbContext>();
//         
//         builder.UseNpgsql(Environment.GetEnvironmentVariable("DB_STRING"));
//         return new AppDbContext(builder.Options);
//     }
// }