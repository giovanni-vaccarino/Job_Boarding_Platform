using backend.Shared.Enums;

namespace backend.Service.Contracts.Feedback;

public class FeedbackResponseDto
{
    public string Id { get; set; } = null!;
    
    public string Text { get; set; } = null!;
    
    public Rating Rating { get; set; }
}