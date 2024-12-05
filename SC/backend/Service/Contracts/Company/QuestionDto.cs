using System.ComponentModel.DataAnnotations;
using backend.Shared.Enums;

namespace backend.Service.Contracts.Company;

public class QuestionDto
{
    [Required]
    public required int Id { get; set; }
    
    [MaxLength(255)]
    [Required]
    public required string Title { get; set; }
    
    [Required]
    public required QuestionType QuestionType { get; set; }
    
    [Required]
    public required List<string> Options { get; set; }
}