using backend.Data;
using MediatR;

namespace backend.Business.Match.InviteStudentUseCase;

public class InviteStudentUseCase : IRequestHandler<InviteStudentCommand, Unit>
{
    private readonly AppDbContext _dbContext;
    
    public InviteStudentUseCase(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<Unit> Handle(InviteStudentCommand request, CancellationToken cancellationToken)
    {
        var matchId = request.MatchId;

        var match = await _dbContext.Matches.FindAsync(matchId);

        if (match == null)
        {
            throw new KeyNotFoundException("Match not found.");
        }

        match.HasInvite = true;
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}