using System.ComponentModel.DataAnnotations;

namespace backend.Service.Contracts.Auth;

public class UpdatePasswordDto
{
    [Required]
    public required string Token { get; set; }
    
    [Required]
    public required string Password { get; set; }
}