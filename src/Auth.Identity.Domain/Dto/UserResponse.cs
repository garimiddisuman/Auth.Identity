namespace Auth.Identity.Domain.Dto;

public class UserResponse
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public DateTime CreateAt { get; set; }
}