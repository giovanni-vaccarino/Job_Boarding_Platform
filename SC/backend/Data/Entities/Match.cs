using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Data.Entities;

public class Match : EntityBase
{
    public required bool HasInvite { get; set; }
    
    [ForeignKey("StudentId")]
    public required int StudentId { get; set; }
    
    public Student Student { get; set; } = null!;
    
    [ForeignKey("InternshipId")]
    public required int InternshipId { get; set; }
    
    public Internship Internship { get; set; } = null!;
}