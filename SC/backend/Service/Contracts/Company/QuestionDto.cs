using System.ComponentModel.DataAnnotations;
using backend.Shared.Enums;

namespace backend.Service.Contracts.Company;

public class QuestionDto
{
    [Required]
    [MaxLength(255)]
    public string questionName { get; set; }
    
    [Required]
    [MaxLength(255)]
    public List<string> questionAnswer { get; set; }
    
    [Required]
    public QuestionType questionType { get; set; }
    
    [Required]
    public List<string> questionOptions { get; set; }
}