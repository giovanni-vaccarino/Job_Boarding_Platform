using System.ComponentModel.DataAnnotations;
namespace backend.Service.Contracts.Company;

public class ReturnFeedbackDto
{
    [Required]
    [MaxLength(255)]
    public string feedbackTitle { get; set; }
    
    
    [Required]
    [MaxLength(255)]
    public string feedbackAnswer { get; set; }
    
    [Required]
    [Range(0, 5, ErrorMessage = "feedbackRating must be between 0 and 5")]
    public int feedbackRating { get; set; }
        
}