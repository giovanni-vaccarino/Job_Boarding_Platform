using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Data.Entities;

public class Company : EntityBase
{
    [MaxLength(255)]
    public string? Name { get; set; }
    
    [MaxLength(16)]
    public string? VatNumber { get; set; }
    
    [MaxLength(128)]
    public string? Website { get; set; }
    
    [ForeignKey("UserId")]
    public required int UserId { get; set; }
    
    public User User { get; set; } = null!;

    public List<Job> Jobs { get; set; } = new List<Job>();
    
    public List<Question> Questions { get; set; } = new List<Question>(); 
}