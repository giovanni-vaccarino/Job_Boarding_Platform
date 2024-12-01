using System.ComponentModel.DataAnnotations;
using backend.Shared.Enums;

namespace backend.Service.Contracts.Company;

public class JobDetailsDto
{
    [Required]
    [MaxLength(255)]
    public string Name { get; set; }
    
    [Required]
    public JobCategory JobCategory { get; set; }
    
    [Required]
    public JobType JobType { get; set; }
    
    [Required]
    [MaxLength(255)]
    public string? Location { get; set; }
    
    [Required]
    [MaxLength(1000)]
    public string? Description { get; set; }
    
    [Required]
    public List<string>? SkillsRequired { get; set; }
}