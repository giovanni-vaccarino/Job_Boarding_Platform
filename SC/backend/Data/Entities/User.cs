﻿using System.ComponentModel.DataAnnotations;

namespace backend.Data.Entities;

public class User: EntityBase
{
    [MaxLength(255)]
    [EmailAddress]
    public required string Email { get; set; }
    
    [MaxLength(255)]
    public required string PasswordHash { get; set; }
    
    [MaxLength(255)]
    public string? RefreshToken { get; set; }
    
    public required bool Verified { get; set; }
    
    public Student? Student { get; set; } 
    
    public Company? Company { get; set; }
    
    public List<PlatformFeedback> PlatformFeedbacks { get; set; } = new();
}