namespace backend.Data.Entities;

public class User: EntityBase
{
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public string? RefreshToken { get; set; }
}