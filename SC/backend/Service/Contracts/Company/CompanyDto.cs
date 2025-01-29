using System.ComponentModel.DataAnnotations;

namespace backend.Service.Contracts.Company;

public class CompanyDto
{
    [Required]
    public required int Id { get; set; }
    [Required]
    
    [MaxLength(255)]
    public required string Name { get; set; }

    [Required]
    [MaxLength(255)]
    [EmailAddress]
    public required string Email { get; set; }
    
    [Required]
    [MaxLength(16)]
    public required string VatNumber { get; set; }
    
    [MaxLength(64)]
    public required string Website { get; set; }
}   