using System.Security.Claims;
using backend.Data;
using backend.Data.Entities;
using backend.Service.Contracts.Auth;
using backend.Shared.Enums;
using backend.Shared.Security;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace backend.Business.Auth.RefreshUseCase;

public class RefreshUseCase : IRequestHandler<RefreshCommand, TokenResponse>
{
    private readonly SecurityContext _securityContext;
    private readonly AppDbContext _dbContext;
    private readonly ILogger<RefreshUseCase> _logger;

    public RefreshUseCase(SecurityContext securityContext, AppDbContext dbContext, ILogger<RefreshUseCase> logger)
    {
        _securityContext = securityContext;
        _dbContext = dbContext;
        _logger = logger;
    }
    
    public async Task<TokenResponse> Handle(RefreshCommand request, CancellationToken cancellationToken)
    {
        var refreshToken = request.Dto.RefreshToken;

        var principal = ValidateToken(refreshToken);
        var userId = GetUserIdFromClaims(principal);
        
        var user = await GetUser(userId);
        
        IsRefreshTokenValid(user, refreshToken);
        
        var newAccessToken = _securityContext.CreateAccessToken(user);
        var newRefreshToken = _securityContext.CreateRefreshToken(user);

        user.UpdatedAt = DateTime.UtcNow;
        user.RefreshToken = _securityContext.Hash(newRefreshToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        
        _logger.LogDebug("Successfully refreshed token for user with email {Email}", user.Email);
        
        return new TokenResponse
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken
        };
    }
    
    private ClaimsPrincipal ValidateToken(string refreshToken)
    {
        try
        {
            return _securityContext.ValidateJwtToken(refreshToken, TokenType.Refresh);
        }
        catch (Exception)
        {
            throw new UnauthorizedAccessException("Invalid refresh token.");
        }
    }
    
    private int GetUserIdFromClaims(ClaimsPrincipal principal)
    {
        var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier)
                          ?? throw new UnauthorizedAccessException("Invalid refresh token payload.");

        if (!int.TryParse(userIdClaim.Value, out int userId))
        {
            throw new UnauthorizedAccessException("Invalid parse error.");
        }

        return userId;
    }
    
    private async Task<User> GetUser(int userId)
    {
        var user =  await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
        _logger.LogDebug(userId.ToString());
        
        if (user == null)
        {
            throw new UnauthorizedAccessException("User associated to refresh token not found.");
        }

        return user;
    }
    
    private void IsRefreshTokenValid(User user, string refreshToken)
    {
        var storedRefreshToken = user.RefreshToken;
        
        if (storedRefreshToken == null || !_securityContext.ValidateHashed(refreshToken, storedRefreshToken))
        {
            throw new UnauthorizedAccessException("Refresh token not matching with stored refresh token.");
        }
    }
}