using backend.Business.Auth.UpdatePasswordUseCase;
using backend.Business.Auth.VerifyMailUseCase;
using backend.Data;
using backend.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace UnitTests.UseCases.Auth;

public class UpdatePasswordUseCaseTests
{
    private readonly IsolatedUseCaseTestServices<UpdatePasswordUseCase> _services;
    private readonly AppDbContext _dbContext;
    private readonly UpdatePasswordUseCase _updatePasswordUseCase;

    /// <summary>
    /// Initializes a new instance of the class, setting up the testing environment.
    /// </summary>
    public UpdatePasswordUseCaseTests()
    {
        _services = new IsolatedUseCaseTestServices<UpdatePasswordUseCase>("UpdatePasswordUseCaseTests");
        _dbContext = _services.DbContext;
        _updatePasswordUseCase = (UpdatePasswordUseCase)Activator.CreateInstance(
            typeof(UpdatePasswordUseCase), _dbContext, _services.SecurityContext, _services.LoggerMock.Object)!;
    }

    [Fact(DisplayName = "Successfully update password")]
    public async Task Should_Update_Password()
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
         
        var newPassword = "NewPassword";
        var command = new UpdatePasswordCommand(verificationToken, newPassword);
         
        await _updatePasswordUseCase.Handle(command, CancellationToken.None);
         
        var verifiedUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == user.Id);
        Assert.NotNull(verifiedUser);
        Assert.NotEqual("Password123!", verifiedUser.PasswordHash);
    }
    
    [Fact(DisplayName = "Successfully verify mail")]
    public async Task Should_Throw_Exception_If_User_Not_Found()
    {
        var verificationToken = _services.SecurityContext.CreateVerificationToken("1");
         
        var newPassword = "NewPassword";
        var command = new UpdatePasswordCommand(verificationToken, newPassword);
         
        var act = async() => await _updatePasswordUseCase.Handle(command, CancellationToken.None);

        
        await Assert.ThrowsAsync<KeyNotFoundException>(act);
    }
}