using System.ComponentModel.DataAnnotations;
using backend.Shared.Enums;

namespace backend.Service.Contracts.Feedback;

public class AddInternshipFeedbackDto
{
    [Required]
    public required string Text { get; set; }
   
    [Required]
    public required Rating Rating { get; set; }
    
    [Required]
    public required int ProfileId { get; set; }
    
    [Required]
    public required int ApplicationId { get; set; }
    
    [Required]
    public required ProfileType Actor { get; set; }
}