using System.ComponentModel.DataAnnotations.Schema;
using backend.Shared.Enums;

namespace backend.Data.Entities;

public class Application : EntityBase
{
    [ForeignKey("StudnetId")]
    public required int StudentId { get; set; }
    
    public Student Student { get; set; } = null!;
    
    [ForeignKey("InternshipId")]
    public required int InternshipId { get; set; }
    
    public Internship Internship { get; set; } = null!;
    
    //Attributes of the class
    
    public required DateTime SubmissionDate { get; set; }
    
    public ApplicationStatus ApplicationStatus { get; set; }
    
    public DateTime LastStatusUpdate { get; set; }
    
}