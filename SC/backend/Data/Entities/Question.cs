using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using backend.Shared.Enums;

namespace backend.Data.Entities;

public class Question : EntityBase
{
    [MaxLength(255)]
    [Required]
    public required string Title { get; set; }
    
    public QuestionType Type { get; set; }

    public List<string> Options { get; set; } = new List<string>();
    
    [ForeignKey("CompanyId")]
    public required int CompanyId { get; set; }
    public Company Company { get; set; } = null!;
    
    public List<JobQuestion> JobQuestions { get; set; } = new List<JobQuestion>();
}