using System.ComponentModel.DataAnnotations;

namespace backend.Service.Contracts.Auth;

public class VerifyMailDto
{
    [Required]
    public required string VerificationToken { get; set; }
}