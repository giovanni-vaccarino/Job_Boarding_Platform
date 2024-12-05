using System.ComponentModel.DataAnnotations;

namespace backend.Service.Contracts.Company;

public class UpdateInternshipDto
{
    [Required]
    public required AddJobDetailsDto JobDetails { get; set; }
}