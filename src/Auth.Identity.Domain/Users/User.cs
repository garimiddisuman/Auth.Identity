using Auth.Identity.Domain.Users.Commands;

namespace Auth.Identity.Domain.Users;

public class User
{
    #region LengthConstraints
    public const int NameMaxLength = 50;
    public const int PasswordMaxLength = 16;
    public const int PasswordMinLength = 8;
    #endregion

    private User()
    {
    }
    
    public int Id { get; set; }
    public string Name { get; private set; }
    public string PasswordHash { get; private set; }
    public DateTime CreateAt { get; private set; }
    
    public User(CreateUserCommand command)
    {
        Name = command.Name;
        PasswordHash = command.Password;
        CreateAt = DateTime.UtcNow;
    }
}
