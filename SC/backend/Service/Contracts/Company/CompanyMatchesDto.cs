using System.ComponentModel.DataAnnotations;

namespace backend.Service.Contracts.Company;

public class ReturnMatchDto
{
    [Required]
    [MaxLength(255)]
    public string nameMatchedPerson { get; set; }
    
    [Required]
    [MaxLength(255)]
    public string suggestedJob { get; set; }
    
}