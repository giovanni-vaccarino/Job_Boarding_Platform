using System.Security.Claims;
using backend.Data;
using backend.Shared.Enums;
using backend.Shared.Security;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace backend.Business.Auth.LogoutUseCase;

/// <summary>
/// Handles the logout use case by invalidating the user's refresh token and updating user data.
/// </summary>
public class LogoutUseCase: IRequestHandler<LogoutCommand, Unit>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly AppDbContext _dbContext;
    private readonly SecurityContext _securityContext;

    /// <summary>
    /// Initializes a new instance of the LogoutUseCase class.
    /// </summary>
    /// <param name="httpContextAccessor">Accessor for the current HTTP context.</param>
    /// <param name="securityContext">The security context for token validation.</param>
    /// <param name="dbContext">The application database context.</param>
    public LogoutUseCase(IHttpContextAccessor httpContextAccessor, SecurityContext securityContext, AppDbContext dbContext)
    {
        _dbContext = dbContext;
        _securityContext = securityContext;
        _httpContextAccessor = httpContextAccessor;
    }
    
    /// <summary>
    /// Handles the logout command by invalidating the user's refresh token and updating the database.
    /// </summary>
    /// <param name="request">The logout command.</param>
    /// <param name="cancellationToken">The cancellation token for the operation.</param>
    /// <returns>A <see cref="Unit"/> indicating that the logout operation has completed successfully.</returns>
    /// <exception cref="UnauthorizedAccessException">
    /// Thrown if:
    /// - Authorization token is missing or invalid.
    /// - User associated with the token cannot be found.
    /// - Token validation fails.
    /// </exception>
    public async Task<Unit> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        var authorizationHeader = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString();

        if (string.IsNullOrEmpty(authorizationHeader) || !authorizationHeader.StartsWith("Bearer "))
        {
            throw new UnauthorizedAccessException("Authorization token is missing or invalid.");
        }
        var accessToken = authorizationHeader.Substring("Bearer ".Length);

        ValidateToken(accessToken);
        
        var userId = GetUserIdFromClaims(ValidateToken(accessToken));
        
        var user =  await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

        if (user == null)
        {
            throw new UnauthorizedAccessException("User not found.");
        }
        
        user.UpdatedAt = DateTime.UtcNow;
        user.RefreshToken = null;
        await _dbContext.SaveChangesAsync(cancellationToken);
   
        return Unit.Value;
    }
    
    /// <summary>
    /// Validates the access token and returns the associated claims principal.
    /// </summary>
    /// <param name="accessToken">The access token to validate.</param>
    /// <returns>A <see cref="ClaimsPrincipal"/> representing the token's claims.</returns>
    /// <exception cref="UnauthorizedAccessException">Thrown if the token is invalid.</exception>
    private ClaimsPrincipal ValidateToken(string accessToken)
    {
        try
        {
            return _securityContext.ValidateJwtToken(accessToken, TokenType.Access);
        }
        catch (Exception)
        {
            throw new UnauthorizedAccessException("Invalid access token.");
        }
    }
    
    /// <summary>
    /// Extracts the user ID from the claims principal.
    /// </summary>
    /// <param name="principal">The claims principal to extract the user ID from.</param>
    /// <returns>The user ID as an integer.</returns>
    /// <exception cref="FormatException">
    /// Thrown if:
    /// - The name identifier claim is missing.
    /// - The user ID cannot be parsed.
    /// </exception>
    private int GetUserIdFromClaims(ClaimsPrincipal principal)
    {
        var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier)
                          ?? throw new FormatException("Invalid refresh token payload.");

        if (!int.TryParse(userIdClaim.Value, out int userId))
        {
            throw new FormatException("Invalid parse error.");
        }

        return userId;
    }
}