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

        user.RefreshToken = newRefreshToken;
        // TODO await _dbContext.SaveChangesAsync(cancellationToken);
        
        
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
        catch (SecurityTokenException)
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
        //TODO var user =  await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
        var tempToken =
            _securityContext.Hash(
                "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiIxIiwidG9rZW5UeXBlIjoiUmVmcmVzaCIsIm5iZiI6MTczMjIzNTE3NSwiZXhwIjoxNzMyODM5OTc1LCJpYXQiOjE3MzIyMzUxNzV9.M0M_ofKLe1DKB3zDHyNDaEsEsZf1PBSA-GWskXR-7ZQ");
        var user = new User
        {
            Id = userId,
            Email = "ritorno@gmail.com",
            PasswordHash = "password",
            CreatedAt = new DateTime(),
            UpdatedAt = new DateTime(),
            RefreshToken = tempToken
        };
        
        if (user == null)
        {
            throw new UnauthorizedAccessException("Invalid refresh token or user not found.");
        }

        return user;
    }
    
    private void IsRefreshTokenValid(User user, string refreshToken)
    {
        var storedRefreshToken = user.RefreshToken;
        
        if (storedRefreshToken == null || !_securityContext.ValidateHashed(refreshToken, storedRefreshToken))
        {
            throw new UnauthorizedAccessException("Invalid refresh token.");
        }
    }
}