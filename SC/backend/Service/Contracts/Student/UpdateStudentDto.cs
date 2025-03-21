﻿using System.ComponentModel.DataAnnotations;

namespace backend.Service.Contracts.Student;

public class UpdateStudentDto
{
    [MaxLength(255)]
    public string? Name { get; set; }
    
    [MaxLength(255)]
    public string? Cf { get; set; }
    
    public List<string> Skills { get; set; } = new List<string>();
    
    public List<string> Interests { get; set; } = new List<string>();
}