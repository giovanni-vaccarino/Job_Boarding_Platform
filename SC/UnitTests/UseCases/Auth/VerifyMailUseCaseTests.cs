using backend.Business.Auth.VerifyMailUseCase;
using backend.Data;
using backend.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace UnitTests.UseCases.Auth;

public class VerifyMailUseCaseTests
{
    private readonly IsolatedUseCaseTestServices<VerifyMailUseCase> _services;
    private readonly AppDbContext _dbContext;
    private readonly VerifyMailUseCase _verifyMailUseCase;

    /// <summary>
    /// Initializes a new instance of the class, setting up the testing environment.
    /// </summary>
    public VerifyMailUseCaseTests()
    {
        _services = new IsolatedUseCaseTestServices<VerifyMailUseCase>("VerifyMailUseCaseTests");
        _dbContext = _services.DbContext;
        _verifyMailUseCase = (VerifyMailUseCase)Activator.CreateInstance(
            typeof(VerifyMailUseCase), _dbContext, _services.SecurityContext, _services.LoggerMock.Object)!;
    }

    [Fact(DisplayName = "Successfully verify mail")]
    public async Task Should_Verify_Mail()
    {
        
        var user = new User
        {
            Email = "student@gmail.com",
            PasswordHash = "Password123!",
            Verified = false
        };
       
         _dbContext.Users.Add(user);
         await _dbContext.SaveChangesAsync();
         
         var verificationToken = _services.SecurityContext.CreateVerificationToken(user.Id.ToString());
         
         var command = new VerifyMailCommand(verificationToken);
         
         await _verifyMailUseCase.Handle(command, CancellationToken.None);
         
         var verifiedUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == user.Id);
         Assert.NotNull(verifiedUser);
         Assert.True(verifiedUser.Verified);
    }
    
    [Fact(DisplayName = "Successfully verify mail")]
    public async Task Should_Throw_Exception_If_User_Not_Found()
    {
        var verificationToken = _services.SecurityContext.CreateVerificationToken("1");
         
        var command = new VerifyMailCommand(verificationToken);
         
        var act = async () => await _verifyMailUseCase.Handle(command, CancellationToken.None);
        
        await Assert.ThrowsAsync<KeyNotFoundException>(act);
    }
}