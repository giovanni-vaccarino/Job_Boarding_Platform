using System.ComponentModel.DataAnnotations;

namespace backend.Service.Contracts.Auth;

public class UserLoginDto
{
    [Required]
    [MaxLength(255)]
    public required string Username { get; set; }

    [Required]
    [MaxLength(255)]
    public required string Password { get; set; }
}