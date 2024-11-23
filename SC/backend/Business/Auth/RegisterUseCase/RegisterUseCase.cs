using backend.Data;
using backend.Data.Entities;
using backend.Service.Contracts.Auth;
using backend.Shared.Security;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace backend.Business.Auth.RegisterUseCase;

public class RegisterUseCase: IRequestHandler<RegisterCommand, TokenResponse>
{
    private readonly SecurityContext _securityContext;
    private readonly AppDbContext _dbContext;
    private readonly ILogger<RegisterUseCase> _logger;

    public RegisterUseCase(SecurityContext securityContext, AppDbContext dbContext, ILogger<RegisterUseCase> logger)
    {
        _securityContext = securityContext;
        _dbContext = dbContext;
        _logger = logger;
    }
    
    public async Task<TokenResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var registerInput = request.Dto;
        
        await VerifyUserCredentials(registerInput);
        
        var user = new User
        {
            Email = registerInput.Email,
            PasswordHash = _securityContext.Hash(registerInput.Password),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        
        var accessToken = _securityContext.CreateAccessToken(user);
        var refreshToken = _securityContext.CreateRefreshToken(user);
        
        user.RefreshToken = _securityContext.Hash(refreshToken);
        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync(cancellationToken);

        _logger.LogDebug("Successfully registered user with email {Email}", user.Email);
        
        return new TokenResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }
    
    private async Task VerifyUserCredentials(UserRegisterDto registerInput)
    {
        if (registerInput.Password != registerInput.ConfirmPassword)
        {
            throw new InvalidOperationException("Passwords do not match.");
        }
        var user =  await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == registerInput.Email);
        
        if (user != null)
        {
            throw new InvalidOperationException("User already exists.");
        }
        
        if (registerInput.Password.Length < 8 || !registerInput.Password.Any(char.IsDigit) || !registerInput.Password.Any(char.IsUpper))
        {
            throw new InvalidOperationException("Password must be at least 8 characters long, contain at least one digit and one uppercase character.");
        }
    }
}