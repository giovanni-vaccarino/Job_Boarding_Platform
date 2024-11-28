using System.ComponentModel.DataAnnotations;
namespace backend.Service.Contracts.Company;

public class AddJobCompanyDto
{
    [Required]
    public AddJobDetailsCompanyDto JobDetails { get; set; }
    
    [Required]
    public List<AddQuestionCompanyDto> Questions { get; set; }
}