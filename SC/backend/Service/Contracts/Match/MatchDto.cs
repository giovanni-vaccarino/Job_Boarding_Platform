using backend.Service.Contracts.Internship;
using backend.Service.Contracts.Student;

namespace backend.Service.Contracts.Match;

public class MatchDto
{
    public required int Id { get; set; }
    
    public required bool HasInvite { get; set; }

    public required InternshipDto Internship { get; set; } = null!;
    
    public required StudentDto Student { get; set; } = null!;
}