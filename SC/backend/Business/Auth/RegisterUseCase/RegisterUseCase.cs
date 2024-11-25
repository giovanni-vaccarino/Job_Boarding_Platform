using backend.Data;
using backend.Data.Entities;
using backend.Service.Contracts.Auth;
using backend.Shared.Security;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace backend.Business.Auth.RegisterUseCase;

/// <summary>
/// Handles the registration use case, creating a new user and generating access and refresh tokens.
/// </summary>
public class RegisterUseCase: IRequestHandler<RegisterCommand, TokenResponse>
{
    private readonly SecurityContext _securityContext;
    private readonly AppDbContext _dbContext;
    private readonly ILogger<RegisterUseCase> _logger;

    /// <summary>
    /// Initializes a new instance of the RegisterUseCas class.
    /// </summary>
    /// <param name="securityContext">The security context for hashing and token generation.</param>
    /// <param name="dbContext">The application database context.</param>
    /// <param name="logger">The logger instance for logging operations.</param>
    public RegisterUseCase(SecurityContext securityContext, AppDbContext dbContext, ILogger<RegisterUseCase> logger)
    {
        _securityContext = securityContext;
        _dbContext = dbContext;
        _logger = logger;
    }
    
    /// <summary>
    /// Handles the registration command by creating a new user and returning authentication tokens.
    /// </summary>
    /// <param name="request">The registration command containing user registration details.</param>
    /// <param name="cancellationToken">The cancellation token for the operation.</param>
    /// <returns>The generated TokenResponse containing access and refresh tokens.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the registration details are invalid.</exception>
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
    
    /// <summary>
    /// Verifies the registration input, ensuring it meets the required criteria.
    /// </summary>
    /// <param name="registerInput">The user registration Dto containing email and password details.</param>
    /// <returns>A task representing the verification process.</returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown if:
    /// - Passwords do not match.
    /// - A user with the given email already exists.
    /// - Password does not meet security requirements.
    /// </exception>
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