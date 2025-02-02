using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Data.Entities;

public class Student : EntityBase
{
    [MaxLength(255)]
    public string? Name { get; set; }
    
    [MaxLength(16)]
    public string? Cf { get; set; }
    
    [MaxLength(128)]
    public string? CvPath { get; set; }

    public List<string> Skills { get; set; } = new List<string>();

    public List<string> Interests { get; set; } = new List<string>();
    
    [ForeignKey("UserId")]
    public required int UserId { get; set; }
    
    public User User { get; set; } = null!;

    public List<Application> Applications { get; set; } = new List<Application>();
    
    public List<Match> Matches { get; set; } = new List<Match>();
}