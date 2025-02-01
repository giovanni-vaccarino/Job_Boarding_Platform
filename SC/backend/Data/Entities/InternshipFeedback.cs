using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using backend.Shared.Enums;

namespace backend.Data.Entities;

public class InternshipFeedback : EntityBase
{
    [Required]
    [MaxLength(1000)]
    public required string Text { get; set; }
   
    [Required]
    public required Rating Rating { get; set; }
    
    [Required]
    public required ProfileType Actor { get; set; }
    
    public Application Application { get; set; } = null!;
    
    [ForeignKey("ApplicationId")]
    public required int ApplicationId { get; set; }
}