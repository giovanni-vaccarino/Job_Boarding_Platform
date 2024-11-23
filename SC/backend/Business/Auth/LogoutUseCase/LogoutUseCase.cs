using System.Security.Claims;
using backend.Data;
using backend.Shared.Enums;
using backend.Shared.Security;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace backend.Business.Auth.LogoutUseCase;

public class LogoutUseCase: IRequestHandler<LogoutCommand, Unit>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly AppDbContext _dbContext;
    private readonly SecurityContext _securityContext;

    public LogoutUseCase(IHttpContextAccessor httpContextAccessor, SecurityContext securityContext, AppDbContext dbContext)
    {
        _dbContext = dbContext;
        _securityContext = securityContext;
        _httpContextAccessor = httpContextAccessor;
    }
    
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
    
    private ClaimsPrincipal ValidateToken(string accessToken)
    {
        try
        {
            return _securityContext.ValidateJwtToken(accessToken, TokenType.Access);
        }
        catch (SecurityTokenException)
        {
            throw new UnauthorizedAccessException("Invalid access token.");
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
}