using backend.Data;
using backend.Data.Entities;
using backend.Shared.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace backend.Business.Feedback.AddPlatformFeedbackUseCase;

/// <summary>
/// Use case for adding platform feedback.
/// </summary>
public class AddPlatformFeedbackUseCase : IRequestHandler<AddPlatformFeedbackCommand, Unit>
{
    private readonly AppDbContext _dbContext;
    private readonly ILogger<AddPlatformFeedbackUseCase> _logger;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="AddPlatformFeedbackUseCase"/> class.
    /// </summary>
    /// <param name="dbContext">The application database context.</param>
    /// <param name="logger">The logger instance.</param>
    public AddPlatformFeedbackUseCase(AppDbContext dbContext, ILogger<AddPlatformFeedbackUseCase> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }
    
    /// <summary>
    /// Handles the command to add platform feedback.
    /// </summary>
    /// <param name="request">The feedback command containing the feedback details.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A unit task representing the completion of the command.</returns>
    public async Task<Unit> Handle(AddPlatformFeedbackCommand request, CancellationToken cancellationToken)
    {
        var profileId = request.Dto.ProfileId;
        var actor = request.Dto.Actor;
        
        var userId = actor == ProfileType.Student ? 
            await _dbContext.Students.Where(s => s.Id == profileId).Select(s => s.UserId).FirstOrDefaultAsync(cancellationToken) :
            await _dbContext.Companies.Where(c => c.Id == profileId).Select(c => c.UserId).FirstOrDefaultAsync(cancellationToken);
        
        var platformFeedback = new PlatformFeedback
        {
            Text = request.Dto.Text,
            Rating = request.Dto.Rating,
            UserId = userId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        
        _dbContext.PlatformFeedbacks.Add(platformFeedback);
        await _dbContext.SaveChangesAsync(cancellationToken);
        
        return Unit.Value;
    }
}