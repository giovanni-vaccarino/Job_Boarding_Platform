using System.ComponentModel.DataAnnotations;

namespace backend.Service.Contracts.Student;

public class StudentDto
{
    public required int Id { get; set; }
    
    [MaxLength(255)]
    public required string Name { get; set; }
    
    [MaxLength(255)]
    [EmailAddress]
    public required string Email { get; set; }
    
    [MaxLength(255)]
    public required string Cf { get; set; }
    
    [MaxLength(255)]
    public required string CvPath { get; set; }
    
    public List<string> Skills { get; set; } = new List<string>();
    
    public List<string> Interests { get; set; } = new List<string>();
}
