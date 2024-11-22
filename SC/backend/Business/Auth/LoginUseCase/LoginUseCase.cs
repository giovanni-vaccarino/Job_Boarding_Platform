using backend.Data;
using backend.Data.Entities;
using backend.Service.Contracts.Auth;
using backend.Shared.Security;
using MediatR;

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
        
        // Verify user credentials
        var user = await VerifyUserCredentials(loginInput);
        
        // Generate JWT token
        var tokenResponse = new TokenResponse
        {
            AccessToken = _securityContext.CreateAccessToken(user),
            RefreshToken = _securityContext.CreateRefreshToken(user)
        };
        
        user.RefreshToken = tokenResponse.RefreshToken;
        // TODO await _dbContext.SaveChangesAsync(cancellationToken);

        return tokenResponse;
    }

    private async Task<User> VerifyUserCredentials(UserLoginDto loginInput)
    {
        var user = new User
        {
            Id = 1,
            Email = "aaa@gmail.com",
            PasswordHash = "password"
        };
        
        //TODO var user =  await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
        
        if (user == null || user.PasswordHash != _securityContext.Hash(loginInput.Password))
        {
            throw new UnauthorizedAccessException("Invalid email or password.");
        }
        
        
        return user;
    }
}