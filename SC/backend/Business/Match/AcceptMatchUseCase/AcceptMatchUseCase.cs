using backend.Business.Internship.ApplyToInternshipUseCase;
using backend.Data;
using MediatR;

namespace backend.Business.Match.AcceptMatchUseCase;

public class AcceptMatchUseCase : IRequestHandler<AcceptMatchCommand, Unit>
{
    private readonly AppDbContext _dbContext;
    private readonly IMediator _mediator;
    
    public AcceptMatchUseCase(AppDbContext dbContext, IMediator mediator)
    {
        _dbContext = dbContext;
        _mediator = mediator;
    }
    
    public async Task<Unit> Handle(AcceptMatchCommand request, CancellationToken cancellationToken)
    {
        var matchId = request.MatchId;
        
        var match = await _dbContext.Matches.FindAsync(matchId);
        
        if (match == null)
        {
            throw new KeyNotFoundException("Match not found.");
        }

        await _mediator.Send(new ApplyToInternshipCommand(match.StudentId, match.InternshipId));
        
        _dbContext.Matches.Remove(match);
        await _dbContext.SaveChangesAsync(cancellationToken);
        
        return Unit.Value;
    }
}