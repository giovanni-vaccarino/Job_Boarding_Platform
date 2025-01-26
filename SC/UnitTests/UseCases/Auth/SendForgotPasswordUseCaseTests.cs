using backend.Business.Auth.SendForgotPasswordUseCase;
using backend.Data;
using backend.Data.Entities;
using Moq;

namespace UnitTests.UseCases.Auth;

public class SendForgotPasswordUseCaseTests
{
    private readonly IsolatedUseCaseTestServices<SendForgotPasswordUseCase> _services;
    private readonly AppDbContext _dbContext;
    private readonly SendForgotPasswordUseCase _sendForgotPasswordUseCase;

    /// <summary>
    /// Initializes a new instance of the class, setting up the testing environment.
    /// </summary>
    public SendForgotPasswordUseCaseTests()
    {
        _services = new IsolatedUseCaseTestServices<SendForgotPasswordUseCase>("SendForgotPasswordUseCaseTests");
        _dbContext = _services.DbContext;
        _sendForgotPasswordUseCase = (SendForgotPasswordUseCase)Activator.CreateInstance(
            typeof(SendForgotPasswordUseCase), _dbContext, _services.EmailServiceMock.Object, _services.SecurityContext, _services.LoggerMock.Object)!;
    }
    
    /// <summary>
    /// Tests successful sending of forgot password request.
    /// </summary>
    [Fact(DisplayName = "Successfully send forgot password to a user")]
    public async Task Should_Send_Forgot_Password()
    {
        var user = new User
        {
            Email = "giovannivaccarino03@gmail.com",
            PasswordHash = "Password123!",
            Verified = true
        };
        
        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();
        
        var command = new SendForgotPasswordCommand(user.Email);
        
        await _sendForgotPasswordUseCase.Handle(command, CancellationToken.None);
        
        _services.EmailServiceMock.Verify(x => x.SendEmailAsync(user.Email, "Password Reset", It.IsAny<string>()), Times.Once);
    }
}