using System.ComponentModel.DataAnnotations;
using backend.Shared.Enums;

namespace backend.Service.Contracts.Auth;

public class TokenResponse
{
    [Required]
    [MaxLength(1024)]
    public required string AccessToken { get; set; }

    [Required]
    [MaxLength(1024)]
    public required string RefreshToken { get; set; }
    
    [Required]
    public required int ProfileId { get; set; }
    
    [Required]
    public required ProfileType ProfileType { get; set; }
}