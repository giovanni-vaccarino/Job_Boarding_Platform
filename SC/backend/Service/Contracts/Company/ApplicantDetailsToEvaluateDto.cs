using System.ComponentModel.DataAnnotations;

namespace backend.Service.Contracts.Company;

public class ApplicantDetailsToEvaluateDto
{
    [Required]
    public ApplicantDetailsDto applicantDetails { get; set; }
    
    [Required]
    public List<QuestionDto> questions { get; set; }
    
}