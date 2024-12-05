using System.ComponentModel.DataAnnotations;

namespace backend.Service.Contracts.Auth;

public class UserLoginDto
{
    [Required]
    [EmailAddress]
    [MaxLength(255)]
    public required string Email { get; set; }

    [Required]
    [MaxLength(255)]
    public required string Password { get; set; }
}