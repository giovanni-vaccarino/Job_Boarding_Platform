using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Data.Entities;

public class InternshipQuestion : EntityBase
{
    [ForeignKey("InternshipId")]
    public required int InternshipId { get; set; }
    public Internship Internship { get; set; } = null!;

    [ForeignKey("QuestionId")]
    public required int QuestionId { get; set; }
    public Question Question { get; set; } = null!;
}