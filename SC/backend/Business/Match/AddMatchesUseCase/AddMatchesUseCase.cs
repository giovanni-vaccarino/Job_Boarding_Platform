using backend.Data;
using MediatR;

namespace backend.Business.Match.AddMatchesUseCase;

public class AddMatchesUseCase : IRequestHandler<AddMatchesCommand, Unit>
{
    private readonly AppDbContext _dbContext;
    
    public AddMatchesUseCase(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<Unit> Handle(AddMatchesCommand request, CancellationToken cancellationToken)
    {
        var internshipIds = request.InternshipIds;
        var studentIds = request.StudentIds;
        
        Console.WriteLine("Adding matches for students and internships...");


        if (internshipIds.Count == 1)
        {
            Console.WriteLine("for students and internships...");
            await AddInternshipMatches(internshipIds[0], studentIds); 
        }
        else
        { 
            await AddStudentMatches(studentIds[0], internshipIds);
        }

        return Unit.Value;
    }
    
    private async Task AddInternshipMatches(int internshipId, List<int> studentIds)
    {
        var matches = studentIds.Select(studentId => new Data.Entities.Match
        {
            InternshipId = internshipId,
            StudentId = studentId,
            HasInvite = false
        }).ToList();
        
        await _dbContext.Matches.AddRangeAsync(matches);
        await _dbContext.SaveChangesAsync();
    }
    
    private async Task AddStudentMatches(int studentId, List<int> internshipIds)
    {
        var matches = internshipIds.Select(internshipId => new Data.Entities.Match
        {
            StudentId = studentId,
            InternshipId = internshipId,
            HasInvite = false
        }).ToList();

        await _dbContext.Matches.AddRangeAsync(matches);
        await _dbContext.SaveChangesAsync();  
    }
}