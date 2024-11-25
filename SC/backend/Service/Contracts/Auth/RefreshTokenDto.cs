using System.ComponentModel.DataAnnotations;

namespace backend.Service.Contracts.Auth;

public class RefreshTokenDto
{
    [MaxLength(255)]
    public required string RefreshToken { get; set; }
}