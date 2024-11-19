using System.ComponentModel.DataAnnotations;

namespace backend.Data.Entities;

public class User: EntityBase
{
    [MaxLength(255)]
    public required string Email { get; set; }
    
    [MaxLength(255)]
    public required string PasswordHash { get; set; }
    
    [MaxLength(255)]
    public string? RefreshToken { get; set; }
}