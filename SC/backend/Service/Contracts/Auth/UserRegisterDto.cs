using System.ComponentModel.DataAnnotations;
using backend.Shared.Enums;

namespace backend.Service.Contracts.Auth;

public class UserRegisterDto
{
    [Required]
    [EmailAddress]
    [MaxLength(255)]
    public required string Email { get; set; }

    [Required]
    [MaxLength(255)]
    public required string Password { get; set; }
    
    [Required]
    [MaxLength(255)]
    public required string ConfirmPassword { get; set; }

    [EnumDataType(typeof(ProfileType))]
    public ProfileType ProfileType { get; set; }
}