using backend.Data;
using backend.Shared.Security;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;

namespace backend.Business.Auth.VerifyMailUseCase;

public class VerifyMailUseCase : IRequestHandler<VerifyMailCommand, Unit>
{
    private readonly AppDbContext _dbContext;
    private readonly SecurityContext _securityContext;
    private readonly ILogger<VerifyMailUseCase> _logger;

    public VerifyMailUseCase(AppDbContext dbContext, SecurityContext securityContext, ILogger<VerifyMailUseCase> logger)
    {
        _dbContext = dbContext;
        _securityContext = securityContext;
        _logger = logger;
    }
    
    public async Task<Unit> Handle(VerifyMailCommand request, CancellationToken cancellationToken)
    {
        var verificationToken = request.VerificationToken;
        
        var principal = _securityContext.ValidateVerificationToken(verificationToken);
        var userId = principal.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

        if (string.IsNullOrEmpty(userId))
        {
            _logger.LogWarning("Invalid verification token: no user ID found");
            throw new Exception("Invalid token.");
        }

        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id.ToString() == userId, cancellationToken)
            ?? throw new Exception("User not found.");

        user.Verified = true;
        await _dbContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("User with email {Email} successfully verified", user.Email);
        
        return Unit.Value;
    }
}