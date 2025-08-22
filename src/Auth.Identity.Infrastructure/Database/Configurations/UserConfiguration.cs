using Auth.Identity.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Auth.Identity.Infrastructure.Database.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(x => x.Name).IsRequired().HasMaxLength(User.NameMaxLength);
        builder.Property(x => x.PasswordHash).IsRequired().HasMaxLength(User.PasswordMaxLength);
    }
}