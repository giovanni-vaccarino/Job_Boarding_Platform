using backend.Business.Auth.LogoutUseCase;
using backend.Data;
using backend.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace UnitTests.UseCases.Auth;

/// <summary>
/// Unit tests for the <see cref="LogoutUseCase"/> class, validating user logout functionality.
/// </summary>
public class LogoutUseCaseTests
{
    private readonly IsolatedUseCaseTestServices<LogoutUseCase> _services;
    private readonly AppDbContext _dbContext;
    private readonly LogoutUseCase _logoutUseCase;

    /// <summary>
    /// Initializes a new instance of the class, setting up the testing environment.
    /// </summary>
    public LogoutUseCaseTests()
    {
        _services = new IsolatedUseCaseTestServices<LogoutUseCase>("LogoutUseCaseTests");
        _dbContext = _services.DbContext;
        _logoutUseCase = (LogoutUseCase)Activator.CreateInstance(
            typeof(LogoutUseCase), _services.HttpContextAccessorMock.Object ,_services.SecurityContext, _dbContext)!;
    }
    
    /// <summary>
    /// Tests successful logout when a valid access token is provided.
    /// </summary>
    [Fact(DisplayName = "Successfully logout with valid token")]
    public async Task Should_Logout_Successfully()
    {
        var user = new User
        {
            Id = 1,
            Email = "test@example.com",
            PasswordHash = _services.SecurityContext.Hash("Password123!"),
            Verified = false
        };
        var refreshToken = _services.SecurityContext.CreateRefreshToken(user);
        user.RefreshToken = _services.SecurityContext.Hash(refreshToken);
        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();

        var accessToken = _services.SecurityContext.CreateAccessToken(user);

        _services.HttpContextAccessorMock
            .Setup(x => x.HttpContext!.Request.Headers["Authorization"])
            .Returns($"Bearer {accessToken}");
        
        var command = new LogoutCommand();
        await _logoutUseCase.Handle(command, CancellationToken.None);
        
        var updatedUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == user.Id);
        Assert.NotNull(updatedUser);
        Assert.Null(updatedUser.RefreshToken);
    }
    
    /// <summary>
    /// Tests that an exception is thrown when the user associated with the token cannot be found.
    /// </summary>
    [Fact(DisplayName = "Should throw exception when user not found")]
    public async Task Should_Throw_Exception_When_User_Not_Found()
    {
        var user = new User
        {
            Id = 1,
            Email = "test@example.com",
            PasswordHash = _services.SecurityContext.Hash("Password123!"),
            Verified = false
        };
        var accessToken = _services.SecurityContext.CreateAccessToken(user);

        _services.HttpContextAccessorMock
            .Setup(x => x.HttpContext!.Request.Headers["Authorization"])
            .Returns($"Bearer {accessToken}");
        
        var command = new LogoutCommand();
        var act = async () => await _logoutUseCase.Handle(command, CancellationToken.None);
        
        var exception = await Assert.ThrowsAsync<UnauthorizedAccessException>(act);
        Assert.Equal("User not found.", exception.Message);
    }
    
    /// <summary>
    /// Tests that an exception is thrown when an invalid access token is provided.
    /// </summary>
    [Fact(DisplayName = "Should throw exception when token is invalid")]
    public async Task Should_Throw_Exception_When_Token_Is_Invalid()
    {
        var invalidAccessToken = "InvalidToken";
    
        _services.HttpContextAccessorMock
            .Setup(x => x.HttpContext!.Request.Headers["Authorization"])
            .Returns($"Bearer {invalidAccessToken}");
        
        var command = new LogoutCommand();
        var act = async () => await _logoutUseCase.Handle(command, CancellationToken.None);
        
        var exception = await Assert.ThrowsAsync<UnauthorizedAccessException>(act);
        Assert.Equal("Invalid access token.", exception.Message);
    }
    
    /// <summary>
    /// Tests that an exception is thrown when the authorization token is not provided.
    /// </summary>
    [Fact(DisplayName = "Should throw exception when token is not provided")]
    public async Task Should_Throw_Exception_When_Token_IsNot_Provided()
    {
        var command = new LogoutCommand();
        var act = async () => await _logoutUseCase.Handle(command, CancellationToken.None);
        
        var exception = await Assert.ThrowsAsync<UnauthorizedAccessException>(act);
        Assert.Equal("Authorization token is missing or invalid.", exception.Message);
    }
}