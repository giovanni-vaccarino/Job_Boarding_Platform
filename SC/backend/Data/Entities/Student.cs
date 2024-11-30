using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace backend.Data.Entities;

public class Student : EntityBase
{
    [ForeignKey("UserId")]
    public required int UserId { get; set; }
    
    public User User { get; set; } = null!;
    
    public List<Application>? Applications { get; set; }
    
    //Attributes of the class
    
    [MaxLength(255)]
    public required string Name { get; set; }
    
    [MaxLength(16)]
    public required string CF { get; set; }
    
    [MaxLength(128)]
    public string? CvPath { get; set; }
    
    [MaxLength(128)]
    public string? ProfileImagePath { get; set; }
    
    public List<string>? Skills { get; set; }

    public List<string>? Interests { get; set; }

}