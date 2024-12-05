using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Data.Entities;

public class Answer : EntityBase
{
    public List<string> StudentAnswer { get; set; } = new List<string>();     
    
    [ForeignKey("ApplicationId")]
    public required int ApplicationId { get; set; }
    public Application Application { get; set; } = null!;

    [ForeignKey("InternshipQuestionId")]
    public required int InternshipQuestionId { get; set; }
    public InternshipQuestion InternshipQuestion { get; set; } = null!;
}