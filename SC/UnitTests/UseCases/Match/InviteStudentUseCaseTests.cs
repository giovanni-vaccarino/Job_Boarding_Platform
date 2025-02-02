using backend.Business.Match.InviteStudentUseCase;
using backend.Data;
using backend.Shared.Enums;

namespace UnitTests.UseCases.Match;

public class InviteStudentUseCaseTests
{
    private readonly AppDbContext _dbContext;
    private readonly IsolatedUseCaseTestServices<InviteStudentUseCase> _services;
    private readonly InviteStudentUseCase _inviteStudentUseCase;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="InviteStudentUseCaseTests"/> class.
    /// Sets up the database context and use case instance for testing.
    /// </summary>
    public InviteStudentUseCaseTests()
    {
        _services = new IsolatedUseCaseTestServices<InviteStudentUseCase>("InviteStudentUseCaseTests");
        _dbContext = _services.DbContext;
        _inviteStudentUseCase = (InviteStudentUseCase)Activator.CreateInstance(
            typeof(InviteStudentUseCase), _dbContext)!;
    }
    
    /// <summary>
    /// Tests that the student is invited when a valid match ID is provided.
    /// </summary>
    [Fact(DisplayName = "Successfully invite a student given a match")]
    public async Task Should_Invite_Student_Successfully()
    {
        var company = new backend.Data.Entities.Company
        {
            Name = "Test Company",
            VatNumber = "123456789",
            UserId = 1,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _dbContext.Companies.Add(company);
        await _dbContext.SaveChangesAsync();
        
        var student = new backend.Data.Entities.Student
        {
            Name = "Test Student",
            Cf = "123456789",
            CvPath = "/student/1",
            UserId = 2,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        var internship = new backend.Data.Entities.Internship
        {
            Title = "Software Developer Intern",
            Company = company,
            CompanyId = company.Id,
            Description = "Develop software solutions.",
            ApplicationDeadline = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(30)),
            Location = "Remote",
            Duration = DurationType.ThreeToSixMonths,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _dbContext.Students.Add(student);
        _dbContext.Internships.Add(internship);
        
        await _dbContext.SaveChangesAsync();
        var match = new backend.Data.Entities.Match { HasInvite = false, StudentId = student.Id, InternshipId = internship.Id };
        _dbContext.Matches.Add(match);
        await _dbContext.SaveChangesAsync();

        var command = new InviteStudentCommand(match.Id);

        await _inviteStudentUseCase.Handle(command, CancellationToken.None);
        
        var matchInDb = await _dbContext.Matches.FindAsync(match.Id);
        Assert.NotNull(matchInDb);
        Assert.True(matchInDb.HasInvite);
    }
    
    /// <summary>
    /// Tests that an exception is thrown when the provided match ID does not exist in the database.
    /// </summary>
    [Fact(DisplayName = "Throw exception if match not found")]
    public async Task Should_Throw_Exception_If_Match_Not_Found()
    {
        var command = new InviteStudentCommand(1);

        var act = async () => await _inviteStudentUseCase.Handle(command, CancellationToken.None);
        
        var exception = await Assert.ThrowsAsync<KeyNotFoundException>(act);
        Assert.Equal("Match not found.", exception.Message);
    }
}