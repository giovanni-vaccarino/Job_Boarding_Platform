using backend.Business.Student.UpdateStudentUseCase;
using backend.Data;
using backend.Data.Entities;
using backend.Service.Contracts.Student;


namespace UnitTests.UseCases.Student;

/// <summary>
/// Contains unit tests for the <see cref="UpdateStudentUseCase"/> class.
/// </summary>
public class UpdateStudentUseCaseTests
{
    private readonly IsolatedUseCaseTestServices<UpdateStudentUseCase> _services;
    private readonly AppDbContext _dbContext;
    private readonly UpdateStudentUseCase _updateStudentUseCase;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateStudentUseCaseTests"/> class.
    /// Sets up the necessary test environment including mock services, database context, and use case instance.
    /// </summary>
    public UpdateStudentUseCaseTests()
    {
        _services = new IsolatedUseCaseTestServices<UpdateStudentUseCase>("UpdateStudentUseCaseTests");
        _dbContext = _services.DbContext;
        _updateStudentUseCase = (UpdateStudentUseCase)Activator.CreateInstance(
            typeof(UpdateStudentUseCase), _dbContext, _services.LoggerMock.Object, _services.Mapper,
            _services.JobQueue.Object, _services.StudentMatchingTaskFactory)!;
    }
    
    /// <summary>
    /// Tests that the <see cref="UpdateStudentUseCase.Handle"/> method successfully updates a student
    /// when a valid student ID and data are provided.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    [Fact]
    public async Task Handle_ShouldUpdateStudent_WhenStudentExists()
    {
        var updatedName = "Updated Name";
        var updatedCf = "AAABBB00H00A000A";
        var updatedSkills = new List<string>(["Skill 1", "Skill 2"]);
        var updatedInterests = new List<string>(["Interest 1", "Interest 2"]);
        
        var existingUser = new User
        {
            Email = "test@example.com",
            PasswordHash = _services.SecurityContext.Hash("Password123!"),
            Verified = false
        };
        _dbContext.Users.Add(existingUser);
        await _dbContext.SaveChangesAsync();
        
        var existingStudent = new backend.Data.Entities.Student
        {
            Name = "Old Name",
            Cf = "Old CF",
            UserId = existingUser.Id,
        };
        
        _dbContext.Students.Add(existingStudent);
        await _dbContext.SaveChangesAsync();

        var updateStudentDto = new UpdateStudentDto
        {
            Name = updatedName,
            Cf = updatedCf,
            Skills = updatedSkills,
            Interests = updatedInterests
        };

        var command = new UpdateStudentCommand(existingStudent.Id, updateStudentDto);
        
        var result = await _updateStudentUseCase.Handle(command, CancellationToken.None);
        
        Assert.NotNull(result);
        Assert.Equal(updatedName, result.Name);
        Assert.Equal(updatedCf, result.Cf);
        Assert.Equal(updatedSkills, result.Skills);
        Assert.Equal(updatedInterests, result.Interests);
    }
    
    /// <summary>
    /// Tests that the <see cref="UpdateStudentUseCase.Handle"/> method throws a <see cref="KeyNotFoundException"/>
    /// when attempting to update a student with a non-existent ID.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    [Fact]
    public async Task Handle_ShouldThrowKeyNotFoundException_WhenStudentDoesNotExist()
    {
        var nonExistentStudentId = 1;
        var updatedName = "Updated Name";
        var updatedCf = "AAABBB00H00A000A";

        var updateStudentDto = new UpdateStudentDto
        {
            Name = updatedName,
            Cf = updatedCf
        };

        var command = new UpdateStudentCommand(nonExistentStudentId, updateStudentDto);
        
        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _updateStudentUseCase.Handle(command, CancellationToken.None));
    }
}