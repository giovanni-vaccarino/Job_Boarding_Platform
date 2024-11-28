using System.ComponentModel.DataAnnotations;
using backend.Shared.Enums;

namespace backend.Service.Contracts.Company;

public class SingleApplicantToInternshipDto
{
    [Required]
    [MaxLength(255)]
    public string nameApplicant { get; set; }
    
    [Required]
    public ApplicationStatus applicationStatus { get; set; }
    
    [Required]
    public DataType submissionDate { get; set; }
}