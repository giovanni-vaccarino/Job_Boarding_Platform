using backend.Data;
using backend.Data.Entities;
using backend.Service.Contracts.Auth;
using backend.Shared.Security;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace backend.Business.Auth.LoginUseCase;

public class LoginUseCase: IRequestHandler<LoginCommand, TokenResponse>
{
    private readonly SecurityContext _securityContext;
    private readonly ILogger<LoginUseCase> _logger;
    private readonly AppDbContext _dbContext;

    public LoginUseCase(SecurityContext securityContext, ILogger<LoginUseCase> logger, AppDbContext dbContext)
    {
        _securityContext = securityContext;
        _logger = logger;
        _dbContext = dbContext;
    }
    
    public async Task<TokenResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var loginInput = request.Dto;
        
        var user = await VerifyUserCredentials(loginInput);
        
        var tokenResponse = new TokenResponse
        {
            AccessToken = _securityContext.CreateAccessToken(user),
            RefreshToken = _securityContext.CreateRefreshToken(user)
        };

        user.UpdatedAt = DateTime.UtcNow;
        user.RefreshToken = _securityContext.Hash(tokenResponse.RefreshToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        _logger.LogDebug("Successfully logged in user with email {Email}", user.Email);
        
        return tokenResponse;
    }

    private async Task<User> VerifyUserCredentials(UserLoginDto loginInput)
    {
        var user =  await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == loginInput.Username);
        
        if (user == null || !_securityContext.ValidateHashed(loginInput.Password, user.PasswordHash))
        {
            throw new UnauthorizedAccessException("Invalid email or password.");
        }
        
        return user;
    }
}