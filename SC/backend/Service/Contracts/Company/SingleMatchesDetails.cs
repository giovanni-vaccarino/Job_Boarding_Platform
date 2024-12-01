using System.ComponentModel.DataAnnotations;

namespace backend.Service.Contracts.Company;

public class SingleMatchesDetails
{
    [Required]
    [MaxLength(255)]
    public string name { get; set; }
    
    [Required]
    [MaxLength(255)]
    public string jobTitle { get; set; }
}