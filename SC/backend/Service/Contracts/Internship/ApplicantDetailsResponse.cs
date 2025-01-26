using System.ComponentModel.DataAnnotations;
using backend.Service.Contracts.Company;
using backend.Service.Contracts.Feedback;

namespace backend.Service.Contracts.Internship;

public class ApplicantDetailsResponse
{
    public ApplicantResponseDto[]? Answers { get; set; } = null!;
    
    public FeedbackResponseDto[]? Feedbacks { get; set; } = null!;
    
    [Required]
    public required int StudentId { get; set; }
    
    [Required]
    public required string Name { get; set; }
    
    [Required]
    public required List<string> Skills { get; set; } = null!;
    
}