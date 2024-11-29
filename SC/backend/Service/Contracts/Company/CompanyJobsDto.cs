using System.ComponentModel.DataAnnotations;
using backend.Shared.Enums;

namespace backend.Service.Contracts.Company;

public class CompanyJobsDto
{
    [Required]
    [MaxLength(255)]
    public string? Title { get; set; }
    
    [Required]
    [Range(0, int.MaxValue, ErrorMessage = "ApplicationReceived must be greater or equal than 0")]
    public int ApplicationReceived { get; set; }
    
    [Required]
    public JobType? JobType { get; set; }
    
    [Required]
    [MaxLength(255)]
    public string? Location { get; set; }
}