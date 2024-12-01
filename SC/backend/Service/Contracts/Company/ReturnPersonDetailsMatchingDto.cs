using System.ComponentModel.DataAnnotations;
using backend.Shared.Enums;

namespace backend.Service.Contracts.Company;

//

public class ReturnPersonDetailsMatchingDto
{
    [Required]
    [MaxLength(255)]
    public string nameApplicant { get; set; }
    
    [Required]
    [MaxLength(255)]
    public List<SkillsType> skills { get; set; }
    
    [Required]
    public List<ReturnFeedbackDto> feedbacks { get; set; }
}