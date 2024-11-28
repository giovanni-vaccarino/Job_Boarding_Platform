using backend.Business.Auth.LoginUseCase;
using backend.Data;
using backend.Data.Entities;
using backend.Service.Contracts.Auth;
using backend.Shared.Enums;

namespace UnitTests.UseCases.Auth;

/// <summary>
/// Unit tests for the <see cref="LoginUseCase"/> class, validating login functionality.
/// </summary>
public class LoginUseCaseTests
{
    private readonly IsolatedUseCaseTestServices<LoginUseCase> _services;
    private readonly AppDbContext _dbContext;
    private readonly LoginUseCase _loginUseCase;

    /// <summary>
    /// Initializes a new instance of the class, setting up the testing environment.
    /// </summary>
    public LoginUseCaseTests()
    {
        _services = new IsolatedUseCaseTestServices<LoginUseCase>("LoginUseCaseTests");
        _dbContext = _services.DbContext;
        _loginUseCase = (LoginUseCase)Activator.CreateInstance(
            typeof(LoginUseCase), _services.SecurityContext, _dbContext, _services.LoggerMock.Object)!;
    }
    
    /// <summary>
    /// Tests successful login with valid user credentials.
    /// </summary>
    [Fact(DisplayName = "Successfully login with valid credentials")]
    public async Task Should_Login_Successfully()
    {
        var existingUser = new User
        {
            Email = "test@example.com",
            PasswordHash = _services.SecurityContext.Hash("Password123!")
        };
        _dbContext.Users.Add(existingUser);
        await _dbContext.SaveChangesAsync();
        
        _dbContext.Students.Add(new Student { UserId = existingUser.Id, Name = "", CF = ""});
        await _dbContext.SaveChangesAsync();

        var loginDto = new UserLoginDto
        {
            Email = "test@example.com",
            Password = "Password123!"
        };
        var command = new LoginCommand(loginDto);
        
        var result = await _loginUseCase.Handle(command, CancellationToken.None);
        
        Assert.NotNull(result);
        Assert.NotNull(result.AccessToken);
        Assert.NotNull(result.RefreshToken);
        Assert.NotNull(_services.SecurityContext.ValidateJwtToken(result.AccessToken, TokenType.Access));
        Assert.NotNull(_services.SecurityContext.ValidateJwtToken(result.RefreshToken, TokenType.Refresh));

        var user = _dbContext.Users.FirstOrDefault();
        Assert.NotNull(user);
        Assert.NotNull(user.RefreshToken);
        Assert.True(_services.SecurityContext.ValidateHashed(result.RefreshToken, user.RefreshToken));
    }
    
    /// <summary>
    /// Tests that an exception is thrown when attempting to log in with an invalid email.
    /// </summary>
    [Fact(DisplayName = "Should throw exception when email is invalid")]
    public async Task Should_Throw_Exception_When_Email_Is_Invalid()
    {
        var loginDto = new UserLoginDto
        {
            Email = "invalid@example.com",
            Password = "Password123!"
        };
        var command = new LoginCommand(loginDto);

        var act = async () => await _loginUseCase.Handle(command, CancellationToken.None);

        var exception = await Assert.ThrowsAsync<UnauthorizedAccessException>(act);
        Assert.Equal("Invalid email or password.", exception.Message);
    }

    /// <summary>
    /// Tests that an exception is thrown when the provided password does not match the stored hash password.
    /// </summary>
    [Fact(DisplayName = "Should throw exception when password does not match")]
    public async Task Should_Throw_Exception_When_Password_Does_Not_Match()
    {
        var existingUser = new User
        {
            Email = "test@example.com",
            PasswordHash = _services.SecurityContext.Hash("CorrectPassword123!")
        };
        _dbContext.Users.Add(existingUser);
        await _dbContext.SaveChangesAsync();

        var loginDto = new UserLoginDto
        {
            Email = "test@example.com",
            Password = "WrongPassword123!"
        };
        var command = new LoginCommand(loginDto);
        
        var act = async () => await _loginUseCase.Handle(command, CancellationToken.None);
        
        var exception = await Assert.ThrowsAsync<UnauthorizedAccessException>(act);
        Assert.Equal("Invalid email or password.", exception.Message);
    }
}