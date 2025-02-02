using backend.Business.Auth.SendVerificationMailUseCase;
using backend.Data;
using backend.Data.Entities;
using Moq;

namespace UnitTests.UseCases.Auth;

public class SendVerificationMailUseCaseTests
{
    private readonly IsolatedUseCaseTestServices<SendVerificationMailUseCase> _services;
    private readonly AppDbContext _dbContext;
    private readonly SendVerificationMailUseCase _sendVerificationMailUseCase;

    /// <summary>
    /// Initializes a new instance of the class, setting up the testing environment.
    /// </summary>
    public SendVerificationMailUseCaseTests()
    {
        _services = new IsolatedUseCaseTestServices<SendVerificationMailUseCase>("SendVerificationMailUseCaseTests");
        _dbContext = _services.DbContext;
        _sendVerificationMailUseCase = (SendVerificationMailUseCase)Activator.CreateInstance(
            typeof(SendVerificationMailUseCase), _dbContext, _services.SecurityContext, _services.EmailServiceMock.Object, _services.LoggerMock.Object)!;
    }
    
    /// <summary>
    /// Tests successful sending of verification mail.
    /// </summary>
    [Fact(DisplayName = "Successfully send verification mail to a user")]
    public async Task Should_Send_Verification_Mail()
    {
        var user = new User
        {
            Email = "giovannivaccarino03@gmail.com",
            PasswordHash = "Password123!",
            Verified = false
        };
        
        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();
        
        var command = new SendVerificationMailCommand(user.Email);
        
        await _sendVerificationMailUseCase.Handle(command, CancellationToken.None);
        
        _services.EmailServiceMock.Verify(x => x.SendEmailAsync(user.Email, "SC Email Verification", It.IsAny<string>()), Times.Once);
    }
}