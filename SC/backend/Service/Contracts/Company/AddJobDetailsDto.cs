using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;
using backend.Shared.Enums;

namespace backend.Service.Contracts.Company;

public class AddJobDetailsDto
{
    [MaxLength(255)]
    [Required]
    public required string Title { get; set; }
   
    [Required]
    public required DurationType Duration { get; set; }
   
    [Required]
    [MaxLength(1000)]
    public required string Description { get; set; }
   
    [Required]
    public required DateOnly ApplicationDeadline { get; set; }
   
    [MaxLength(255)]
    [Required]
    public required string Location { get; set; }
   
    public JobCategory? JobCategory { get; set; }
   
    public JobType? JobType { get; set; }

    [Required]
    public required List<string> Requirements { get; set; }
}