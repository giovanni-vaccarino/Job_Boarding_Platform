using backend.Data.Entities;
using backend.Service.Contracts.Auth;
using backend.Shared.Security;
using MediatR;

namespace backend.Business.Auth.LoginUseCase;

public class LoginUseCase: IRequestHandler<LoginCommand, TokenResponse>
{
    private readonly SecurityContext _securityContext;
    private readonly ILogger<LoginUseCase> _logger;

    public LoginUseCase(SecurityContext securityContext, ILogger<LoginUseCase> logger)
    {
        _securityContext = securityContext;
        _logger = logger;
    }
    
    public Task<TokenResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var loginInput = request.Dto;
        if (loginInput.Username != "")
        {
            _logger.LogDebug("Username not empty");
            _logger.LogDebug(loginInput.Username);
            _logger.LogDebug(loginInput.Password);
        }else{
            _logger.LogDebug("Username empty");
        }
        
        // Verify user credentials
        
        var user = new User
        {
            Id = 1,
            Email = "aaa@gmail.com",
            PasswordHash = "password"
        };
        
        // Generate JWT token
         var tokenResponse = new TokenResponse
         {
             AccessToken = _securityContext.CreateAccessToken(user),
             RefreshToken = _securityContext.CreateRefreshToken(user)
         };

         return Task.FromResult(tokenResponse);
    }
}