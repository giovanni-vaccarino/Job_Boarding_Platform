using backend.Business.Auth.RefreshUseCase;
using backend.Data;
using backend.Data.Entities;
using backend.Service.Contracts.Auth;
using backend.Shared.Enums;

namespace UnitTests.UseCases.Auth;

/// <summary>
/// Unit tests for the <see cref="RefreshUseCase"/> class, validating authentication token refresh functionality.
/// </summary>
public class RefreshUseCaseTests
{
    private readonly IsolatedUseCaseTestServices<RefreshUseCase> _services;
    private readonly AppDbContext _dbContext;
    private readonly RefreshUseCase _refreshUseCase;

    /// <summary>
    /// Initializes a new instance of the class, setting up the testing environment.
    /// </summary>
    public RefreshUseCaseTests()
    {
        _services = new IsolatedUseCaseTestServices<RefreshUseCase>("RefreshUseCaseTests");
        _dbContext = _services.DbContext;
        _refreshUseCase = (RefreshUseCase)Activator.CreateInstance(
            typeof(RefreshUseCase), _services.SecurityContext, _dbContext, _services.LoggerMock.Object)!;
    }
    
    /// <summary>
    /// Tests successful refresh when a valid refresh token is provided.
    /// </summary>
    [Fact(DisplayName = "Successfully refresh tokens")]
    public async Task Should_Refresh_Tokens_Successfully()
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
        
        _dbContext.Companies.Add(new backend.Data.Entities.Company() { UserId = user.Id, Name = "", VatNumber = "", Website = ""});
        await _dbContext.SaveChangesAsync();
        
        var refreshTokenDto = new RefreshTokenDto
        {
            RefreshToken = refreshToken
        };

        var command = new RefreshCommand(refreshTokenDto);
        
        var result = await _refreshUseCase.Handle(command, CancellationToken.None);

        Assert.NotNull(result);
        Assert.NotNull(result.AccessToken);
        Assert.NotNull(result.RefreshToken);
        Assert.NotNull(_services.SecurityContext.ValidateJwtToken(result.RefreshToken, TokenType.Refresh));
        
        var savedUser = _dbContext.Users.FirstOrDefault();
        
        Assert.NotNull(savedUser);
        Assert.NotNull(savedUser.RefreshToken);
        Assert.True(_services.SecurityContext.ValidateHashed(result.RefreshToken, savedUser.RefreshToken));
    }
    
    /// <summary>
    /// Tests that an exception is thrown when the refresh token provided is not valid.
    /// </summary>
    [Fact(DisplayName = "Should throw exception for invalid refresh token")]
    public async Task Should_Throw_Exception_For_Invalid_Refresh_Token()
    {
        var invalidRefreshTokenDto = new RefreshTokenDto
        {
            RefreshToken = "invalid_refresh_token"
        };

        var command = new RefreshCommand(invalidRefreshTokenDto);
        
        var act = async () => await _refreshUseCase.Handle(command, CancellationToken.None);
        
        var exception = await Assert.ThrowsAsync<UnauthorizedAccessException>(act);
        
        Assert.Equal("Invalid refresh token.", exception.Message);
    }
    
    /// <summary>
    /// Tests that an exception is thrown when the user associated with that refresh token cannot be found.
    /// </summary>
    [Fact(DisplayName = "Should throw exception when user is not found")]
    public async Task Should_Throw_Exception_When_User_Not_Found()
    {
        var user = new User
        {
            Id = 1,
            Email = "test@example.com",
            PasswordHash = _services.SecurityContext.Hash("Password123!"),
            Verified = false
        };
        user.RefreshToken = _services.SecurityContext.CreateRefreshToken(user);
        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();
        
        var secondUser = new User
        {
            Id = 2,
            Email = "test2@example.com",
            PasswordHash = _services.SecurityContext.Hash("Password123!"),
            Verified = false
        };
        
        var refreshTokenDto = new RefreshTokenDto
        {
            RefreshToken = _services.SecurityContext.CreateRefreshToken(secondUser)
        };

        var command = new RefreshCommand(refreshTokenDto);
        
        var act = async () => await _refreshUseCase.Handle(command, CancellationToken.None);
        
        var exception = await Assert.ThrowsAsync<UnauthorizedAccessException>(act);
        Assert.Equal("User associated to refresh token not found.", exception.Message);
    }
    
    [Fact(DisplayName = "Should throw exception when refresh token is not found")]
    public async Task Should_Throw_Exception_When_Token_Not_Found()
    {
        var user = new User
        {
            Id = 1,
            Email = "test@example.com",
            PasswordHash = _services.SecurityContext.Hash("Password123!"),
            Verified = false
        };
        var refreshToken = _services.SecurityContext.CreateRefreshToken(user);
        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();
        
        _dbContext.Companies.Add(new backend.Data.Entities.Company() { UserId = user.Id, Name = "", VatNumber = "", Website = ""});
        await _dbContext.SaveChangesAsync();
        
        var refreshTokenDto = new RefreshTokenDto
        {
            RefreshToken = refreshToken
        };

        var command = new RefreshCommand(refreshTokenDto);
        
        var act = async () => await _refreshUseCase.Handle(command, CancellationToken.None);
        
        var exception = await Assert.ThrowsAsync<UnauthorizedAccessException>(act);
        Assert.Equal("Refresh token not matching with stored refresh token.", exception.Message);
    }
}