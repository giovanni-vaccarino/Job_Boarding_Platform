using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace backend.Service.Contracts.Auth;

public class SendVerificationMailDto
{
    [Required]
    public required string Email { get; set; }
}