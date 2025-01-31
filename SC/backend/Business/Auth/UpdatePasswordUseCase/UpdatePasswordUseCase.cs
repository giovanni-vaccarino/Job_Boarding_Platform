using backend.Data;
using backend.Shared.Security;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;

namespace backend.Business.Auth.UpdatePasswordUseCase;

public class UpdatePasswordUseCase : IRequestHandler<UpdatePasswordCommand, Unit>
{
    private readonly AppDbContext _dbContext;
    private readonly SecurityContext _securityContext;
    private readonly ILogger<UpdatePasswordUseCase> _logger;

    public UpdatePasswordUseCase(AppDbContext dbContext, SecurityContext securityContext, ILogger<UpdatePasswordUseCase> logger)
    {
        _dbContext = dbContext;
        _securityContext = securityContext;
        _logger = logger;
    }

    public async Task<Unit> Handle(UpdatePasswordCommand request, CancellationToken cancellationToken)
    {
        var principal = _securityContext.ValidateVerificationToken(request.Token);
        var userId = principal.FindFirst("userId")?.Value;

        if (string.IsNullOrEmpty(userId))
        {
            _logger.LogWarning("Invalid password reset token: no user ID found");
            throw new UnauthorizedAccessException("Invalid token.");
        }

        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id.ToString() == userId, cancellationToken)
            ?? throw new KeyNotFoundException("User not found.");

        user.PasswordHash = _securityContext.Hash(request.Password);
        user.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Password updated for user with email {Email}", user.Email);

        return Unit.Value;
    }
}