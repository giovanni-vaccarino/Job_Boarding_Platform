using System.ComponentModel.DataAnnotations;
using backend.Shared.Enums;

namespace backend.Service.Contracts.Company;

public class AddQuestionCompany
{
    [Required]
    [MaxLength(255)]
    public required QuestionType QuestionType { get; set; }
    
    [Required]
    [MaxLength(255)]
    public required string Question { get; set; }
    
    [Required]
    [MaxLength(255)]
    public required List<string> Answer { get; set; }
}