using System.ComponentModel.DataAnnotations;
using backend.Shared.Enums;

namespace backend.Service.Contracts.Company;

public class CompanyJobsDto
{
    [Required]
    [MaxLength(255)]
    public string title { get; set; }
    
    [Required]
    [Range(0, int.MaxValue, ErrorMessage = "ApplicationRecevied must be greater or equal than 0")]
    public int applicationReceived { get; set; }
    
    [Required]
    public JobType jobType { get; set; }
    
    [Required]
    [MaxLength(255)]
    public string location { get; set; }
}