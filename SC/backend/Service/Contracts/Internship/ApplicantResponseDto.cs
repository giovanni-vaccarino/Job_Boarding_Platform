using backend.Service.Contracts.Company;

namespace backend.Service.Contracts.Internship;

public class ApplicantResponseDto
{
    public required QuestionDto Question { get; set; }
    public required List<string> Answer { get; set; } = null!;
}