using backend.Business.Student.GetStudentUseCase;
using backend.Data;
using backend.Data.Entities;

namespace UnitTests.UseCases.Student;

public class GetStudentUseCaseTests
{
    private readonly IsolatedUseCaseTestServices<GetStudentUseCase> _services;
    private readonly AppDbContext _dbContext;
    private readonly GetStudentUseCase _getStudentUseCase;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetStudentUseCaseTests"/> class.
    /// Sets up the necessary test environment including mock services, database context, and use case instance.
    /// </summary>
    public GetStudentUseCaseTests()
    {
        _services = new IsolatedUseCaseTestServices<GetStudentUseCase>("GetStudentUseCaseTests");
        _dbContext = _services.DbContext;
        _getStudentUseCase = (GetStudentUseCase)Activator.CreateInstance(
            typeof(GetStudentUseCase), _dbContext, _services.LoggerMock.Object, _services.Mapper)!;
    }
    
    /// <summary>
    /// Tests that the <see cref="GetStudentUseCase.Handle"/> method successfully retrieves a student
    /// when a valid student ID is provided.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    [Fact]
    public async Task Handle_ShouldReturnStudent_WhenStudentExists()
    {
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
            Name = "Existing Student",
            Cf = "AAABBB00H00A000A",
            UserId = existingUser.Id,
        };
        _dbContext.Students.Add(existingStudent);
        await _dbContext.SaveChangesAsync();

        var query = new GetStudentQuery(existingStudent.Id);

        var result = await _getStudentUseCase.Handle(query, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(existingStudent.Id, result.Id);
        Assert.Equal(existingStudent.Name, result.Name);
        Assert.Equal(existingStudent.Cf, result.Cf);
    }
    
    /// <summary>
    /// Tests that the <see cref="GetStudentUseCase.Handle"/> method throws a <see cref="KeyNotFoundException"/>
    /// when attempting to retrieve a student with a non-existent ID.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    [Fact]
    public async Task Handle_ShouldThrowKeyNotFoundException_WhenStudentDoesNotExist()
    {
        var nonExistentStudentId = 1;
        var query = new GetStudentQuery(nonExistentStudentId);

        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _getStudentUseCase.Handle(query, CancellationToken.None));
    }
}