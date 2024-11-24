using backend.Business.Auth.RegisterUseCase;
using backend.Data;
using backend.Data.Entities;
using backend.Service.Contracts.Auth;
using backend.Shared.Enums;

namespace UnitTests.UseCases.Auth;

public class RegisterUseCaseTests
{
    private readonly IsolatedUseCaseTestServices<RegisterUseCase> _services;
    private readonly AppDbContext _dbContext;
    private readonly RegisterUseCase _registerUseCase;

    public RegisterUseCaseTests()
    {
        _services = new IsolatedUseCaseTestServices<RegisterUseCase>("RegisterUseCaseTests");
        _dbContext = _services.DbContext;
        _registerUseCase = (RegisterUseCase)Activator.CreateInstance(
            typeof(RegisterUseCase), _services.SecurityContext, _dbContext, _services.LoggerMock.Object)!;
    }

    [Fact(DisplayName = "Successfully register a new user")]
    public async Task Should_Register_User()
    {
        var userDto = new UserRegisterDto
        {
            Email = "test@example.com",
            Password = "Password123!",
            ConfirmPassword = "Password123!"
        };
        var command = new RegisterCommand(userDto);
        
        var result = await _registerUseCase.Handle(command, CancellationToken.None);
        
        var savedUser = _dbContext.Users.FirstOrDefault();
        
        Assert.NotNull(savedUser);
        Assert.Equal(userDto.Email, savedUser.Email);
        Assert.True(_services.SecurityContext.ValidateHashed(userDto.Password, savedUser.PasswordHash));
        Assert.NotNull(result);
        Assert.NotNull(result.AccessToken);
        Assert.NotNull(result.RefreshToken);
        Assert.NotNull(_services.SecurityContext.ValidateJwtToken(result.AccessToken, TokenType.Access));
        Assert.NotNull(_services.SecurityContext.ValidateJwtToken(result.RefreshToken, TokenType.Refresh));
    }
    
    [Fact(DisplayName = "Should throw exception when user already exists")]
    public async Task Should_Throw_Exception_When_User_Already_Exists()
    {
        var existingUser = new User
        {
            Email = "test@example.com",
            PasswordHash = _services.SecurityContext.Hash("ExistingPassword123!")
        };
        _dbContext.Users.Add(existingUser);
        await _dbContext.SaveChangesAsync();

        var userDto = new UserRegisterDto
        {
            Email = "test@example.com",
            Password = "Password123!",
            ConfirmPassword = "Password123!"
        };
        var command = new RegisterCommand(userDto);
        
        var act = async () => await _registerUseCase.Handle(command, CancellationToken.None);
        
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(act);
        Assert.Equal("User already exists.", exception.Message);
    }

    [Theory(DisplayName = "Should throw exception when password does not meet strength requirements")]
    [InlineData("short", "Password must be at least 8 characters long, contain at least one digit and one uppercase character.")]
    [InlineData("lowercaseonly", "Password must be at least 8 characters long, contain at least one digit and one uppercase character.")]
    [InlineData("12345678", "Password must be at least 8 characters long, contain at least one digit and one uppercase character.")]
    public async Task Should_Throw_Exception_When_Password_Strength_Is_Invalid(string invalidPassword, string expectedErrorMessage)
    {
        var userDto = new UserRegisterDto
        {
            Email = "newuser@example.com",
            Password = invalidPassword,
            ConfirmPassword = invalidPassword
        };
        var command = new RegisterCommand(userDto);

        var act = async () => await _registerUseCase.Handle(command, CancellationToken.None);
        
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(act);
        Assert.Equal(expectedErrorMessage, exception.Message);
    }
    
    [Fact(DisplayName = "Should throw exception when password and confirm password do not match")]
    public async Task Should_Throw_Exception_When_Passwords_Do_Not_Match()
    {
        var userDto = new UserRegisterDto
        {
            Email = "test@example.com",
            Password = "Password123!",
            ConfirmPassword = "DifferentPassword123!"
        };
        var command = new RegisterCommand(userDto);
        
        var act = async () => await _registerUseCase.Handle(command, CancellationToken.None);
        
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(act);
        Assert.Equal("Passwords do not match.", exception.Message);
    }


}

