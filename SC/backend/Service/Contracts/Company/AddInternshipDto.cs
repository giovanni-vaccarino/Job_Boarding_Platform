using System.ComponentModel.DataAnnotations;
namespace backend.Service.Contracts.Company;

public class AddInternshipDto
{
    [Required]
    public required AddJobDetailsDto JobDetails { get; set; }
    
    [Required]
    public required List<AddQuestionDto> Questions { get; set; }
    
    [Required]
    public required List<int> ExistingQuestions { get; set; }
}