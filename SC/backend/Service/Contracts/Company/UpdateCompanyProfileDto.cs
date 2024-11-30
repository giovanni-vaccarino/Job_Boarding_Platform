namespace backend.Service.Contracts.Company;

using System.ComponentModel.DataAnnotations;

public class UpdateCompanyProfileDto
{
    [Required]
    [MaxLength(255)]
    public required string Name { get; set; }

    [Required]
    [MaxLength(11)]
    [MinLength(11)]
    public required string Vat { get; set; }
    /*
    [Required]
    [Url]
    [MaxLength(255)]
    public required string Website { get; set; }
    */
}