namespace backend.Service.Contracts.Company;

using System.ComponentModel.DataAnnotations;

public class UpdateCompanyProfileDto
{
    [Required]
    [MaxLength(255)]
    public required string CompanyName { get; set; }

    [Required]
    [MaxLength(9)]
    public required string VAT { get; set; }
    
    [Required]
    [Url]
    [MaxLength(255)]
    public required string Website { get; set; }
    
    [Required]
    [Url]
    [MaxLength(255)]
    public required string Linkedin{get;set;}
}