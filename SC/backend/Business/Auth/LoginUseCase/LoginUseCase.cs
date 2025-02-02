using backend.Data;
using backend.Data.Entities;
using backend.Service.Contracts.Auth;
using backend.Shared.Security;
using MediatR;
using Microsoft.EntityFrameworkCore;
using backend.Shared.Enums;
 

namespace backend.Business.Auth.LoginUseCase;

/// <summary>
/// Handles the login use case, verifying user credentials and generating access and refresh tokens.
/// </summary>
public class LoginUseCase: IRequestHandler<LoginCommand, TokenResponse>
{
    private readonly SecurityContext _securityContext;
    private readonly ILogger<LoginUseCase> _logger;
    private readonly AppDbContext _dbContext;

    /// <summary>
    /// Initializes a new instance of the LoginUseCase class.
    /// </summary>
    /// <param name="securityContext">The security context for token generation and validation.</param>
    /// <param name="dbContext">The application database context.</param>
    /// <param name="logger">The logger instance for logging operations.</param>
    public LoginUseCase(SecurityContext securityContext, AppDbContext dbContext, ILogger<LoginUseCase> logger)
    {
        _securityContext = securityContext;
        _logger = logger;
        _dbContext = dbContext;
    }
    
    /// <summary>
    /// Handles the login command by validating the user's credentials and generating a token response.
    /// </summary>
    /// <param name="request">The login command containing user credentials.</param>
    /// <param name="cancellationToken">The cancellation token for the operation.</param>
    /// <returns>The generated TokenResponse containing access and refresh tokens and the profile id</returns>
    /// <exception cref="UnauthorizedAccessException">Thrown if the user credentials are invalid.</exception>
    public async Task<TokenResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var loginInput = request.Dto;
        
        var user = await VerifyUserCredentials(loginInput);
        
        var profileId = user.Student?.Id ?? user.Company?.Id ?? throw new Exception("User profile not found.");
        
        var profileType = user.Student != null ? ProfileType.Student : ProfileType.Company; // Set profile type

        var tokenResponse = new TokenResponse
        {
            AccessToken = _securityContext.CreateAccessToken(user),
            RefreshToken = _securityContext.CreateRefreshToken(user),
            ProfileId = profileId,
            ProfileType = profileType,  // Set ProfileType here
            Verified = user.Verified
        };

        user.UpdatedAt = DateTime.UtcNow;
        user.RefreshToken = _securityContext.Hash(tokenResponse.RefreshToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        _logger.LogDebug("Successfully logged in user with email {Email}", user.Email);
        
        return tokenResponse;
    }

    /// <summary>
    /// Verifies the user's credentials by checking their email and password.
    /// </summary>
    /// <param name="loginInput">The user login DTO containing the email and password.</param>
    /// <returns>The <see cref="User"/> object if credentials are valid.</returns>
    /// <exception cref="UnauthorizedAccessException">Thrown if the user credentials are invalid.</exception>
    private async Task<User> VerifyUserCredentials(UserLoginDto loginInput)
    {
        var user =  await _dbContext.Users
            .Include(u => u.Student)
            .Include(u => u.Company)
            .FirstOrDefaultAsync(u => u.Email == loginInput.Email);
        
        if (user == null || !_securityContext.ValidateHashed(loginInput.Password, user.PasswordHash))
        {
            throw new UnauthorizedAccessException("Invalid email or password.");
        }
        
        return user;
    }
}