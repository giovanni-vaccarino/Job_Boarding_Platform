using System.ComponentModel.DataAnnotations.Schema;
using backend.Shared.Enums;

namespace backend.Data.Entities;

public class Application : EntityBase
{
    public required ApplicationStatus ApplicationStatus { get; set; }
    
    [ForeignKey("StudentId")]
    public required int StudentId { get; set; }
    
    public Student Student { get; set; } = null!;
    
    [ForeignKey("JobId")]
    public required int JobId { get; set; }
    
    public Job Job { get; set; } = null!;
}