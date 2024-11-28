using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Data.Entities;

public class Company : EntityBase
{
    [MaxLength(255)]
    public required string Name { get; set; }
    
    [MaxLength(16)]
    public required string VatNumber { get; set; }
    
    [ForeignKey("UserId")]
    public required int UserId { get; set; }

    public List<Internship>? Internships { get; set; } = null!;
    
    [MaxLength(32)]
    public required string CompanyWebsite { get; set; }
   
    [MaxLength(32)]
    public required string CompanyLinkedin { get; set; }
    public User User { get; set; } = null!;
}