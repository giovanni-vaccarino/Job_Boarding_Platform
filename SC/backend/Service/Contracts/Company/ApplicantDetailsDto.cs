using System.ComponentModel.DataAnnotations;
using backend.Shared.Enums;

namespace backend.Service.Contracts.Company;

public class ApplicantDetailsDto
{
    [Required]
    [MaxLength(255)]
    public string? CvPath { get; set; }
    
    [Required]
    [MaxLength(255)]
    public List<string>? Skills { get; set; }
    
    [Required]
    public DataType? SubmissionDate { get; set; }
    
    [Required]
    public ApplicationStatus? ApplicationStatus { get; set; }
}