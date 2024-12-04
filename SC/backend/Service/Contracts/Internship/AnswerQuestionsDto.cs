namespace backend.Service.Contracts.Internship;

public class AnswerQuestionsDto
{
    public required List<SingleAnswerQuestion> Questions { get; set; } = null!;
}

public class SingleAnswerQuestion
{
    public required int QuestionId { get; set; }
    public required List<string> Answer { get; set; } = null!;
}
