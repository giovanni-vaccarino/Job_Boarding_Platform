using System.Security.Claims;
using backend.Data;
using backend.Data.Entities;
using backend.Service.Contracts.Auth;
using backend.Shared.Enums;
using backend.Shared.Security;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace backend.Business.Auth.RefreshUseCase;

/// <summary>
/// Handles the token refresh use case, generating new access and refresh tokens for a user.
/// </summary>
public class RefreshUseCase : IRequestHandler<RefreshCommand, TokenResponse>
{
    private readonly SecurityContext _securityContext;
    private readonly AppDbContext _dbContext;
    private readonly ILogger<RefreshUseCase> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="RefreshUseCase"/> class.
    /// </summary>
    /// <param name="securityContext">The security context for token validation and generation.</param>
    /// <param name="dbContext">The application database context.</param>
    /// <param name="logger">The logger instance for logging operations.</param>
    public RefreshUseCase(SecurityContext securityContext, AppDbContext dbContext, ILogger<RefreshUseCase> logger)
    {
        _securityContext = securityContext;
        _dbContext = dbContext;
        _logger = logger;
    }
    
    /// <summary>
    /// Handles the refresh command by validating the refresh token and generating new tokens.
    /// </summary>
    /// <param name="request">The refresh command containing the refresh token.</param>
    /// <param name="cancellationToken">The cancellation token for the operation.</param>
    /// <returns>The new <see cref="TokenResponse"/> containing updated access and refresh tokens.</returns>
    /// <exception cref="UnauthorizedAccessException">
    /// Thrown if the refresh token is invalid, the user cannot be found, or the token does not match the stored value.
    /// </exception>
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
    
    /// <summary>
    /// Validates the provided refresh token and returns the associated claims principal.
    /// </summary>
    /// <param name="refreshToken">The refresh token to validate.</param>
    /// <returns>A <see cref="ClaimsPrincipal"/> representing the token's claims.</returns>
    /// <exception cref="UnauthorizedAccessException">Thrown if the refresh token is invalid.</exception>
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
    
    /// <summary>
    /// Extracts the user ID from the claims principal.
    /// </summary>
    /// <param name="principal">The claims principal to extract the user ID from.</param>
    /// <returns>The user ID as an integer.</returns>
    /// <exception cref="UnauthorizedAccessException">
    /// Thrown if the claims principal does not contain a valid user ID.
    /// </exception>
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
    
    /// <summary>
    /// Retrieves the user associated with the given user ID.
    /// </summary>
    /// <param name="userId">The user ID to look up.</param>
    /// <returns>The <see cref="User"/> object associated with the user ID.</returns>
    /// <exception cref="UnauthorizedAccessException">Thrown if the user cannot be found.</exception>
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
    
    /// <summary>
    /// Validates the refresh token against the stored value for the user.
    /// </summary>
    /// <param name="user">The user whose refresh token is being validated.</param>
    /// <param name="refreshToken">The refresh token to validate.</param>
    /// <exception cref="UnauthorizedAccessException">Thrown if the refresh token does not match the stored value.</exception>
    private void IsRefreshTokenValid(User user, string refreshToken)
    {
        var storedRefreshToken = user.RefreshToken;
        
        if (storedRefreshToken == null || !_securityContext.ValidateHashed(refreshToken, storedRefreshToken))
        {
            throw new UnauthorizedAccessException("Refresh token not matching with stored refresh token.");
        }
    }
}