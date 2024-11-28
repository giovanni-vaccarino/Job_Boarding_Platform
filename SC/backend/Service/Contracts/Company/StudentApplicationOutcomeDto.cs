using System.ComponentModel.DataAnnotations;
using backend.Shared.Enums;

namespace backend.Service.Contracts.Company;

public class StudentApplicationOutcomeDto
{
    [Required]
    public string nameApplicant { get; set; }
    
    [Required]
    public ApplicationStatus applicationStatus { get; set; }
}