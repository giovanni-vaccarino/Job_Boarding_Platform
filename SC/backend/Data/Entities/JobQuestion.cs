using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Data.Entities;

public class JobQuestion : EntityBase
{
    [ForeignKey("JobId")]
    public required int JobId { get; set; }
    public Job Job { get; set; } = null!;

    [ForeignKey("QuestionId")]
    public required int QuestionId { get; set; }
    public Question Question { get; set; } = null!;
}