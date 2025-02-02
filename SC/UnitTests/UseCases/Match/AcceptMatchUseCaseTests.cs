using backend.Business.Match.AcceptMatchUseCase;
using backend.Data;
using backend.Shared.Enums;


namespace UnitTests.UseCases.Match;

/// <summary>
/// Unit tests for the <see cref="AcceptMatchUseCase"/>.
/// </summary>
public class AcceptMatchUseCaseTests
{
    private readonly AppDbContext _dbContext;
    private readonly IsolatedUseCaseTestServices<AcceptMatchUseCase> _services;
    private readonly AcceptMatchUseCase _acceptMatchUseCase;

    /// <summary>
    /// Initializes a new instance of the <see cref="AcceptMatchUseCaseTests"/> class.
    /// Sets up the database context, mediator mock, and use case instance for testing.
    /// </summary>
    public AcceptMatchUseCaseTests()
    {
        _services = new IsolatedUseCaseTestServices<AcceptMatchUseCase>("AcceptMatchUseCaseTests");
        _dbContext = _services.DbContext;
        _acceptMatchUseCase = (AcceptMatchUseCase)Activator.CreateInstance(
            typeof(AcceptMatchUseCase), _dbContext, _services.MediatorMock.Object)!;
    }

    /// <summary>
    /// Tests that the match is accepted and removed from the database when a valid match ID is provided.
    /// </summary>
    [Fact(DisplayName = "Successfully accept and process a match")]
    public async Task Should_Accept_And_Process_Match_Successfully()
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
        var match = new backend.Data.Entities.Match { HasInvite = true, StudentId = student.Id, InternshipId = internship.Id };
        _dbContext.Matches.Add(match);
        await _dbContext.SaveChangesAsync();

        var command = new AcceptMatchCommand(match.Id);

        await _acceptMatchUseCase.Handle(command, CancellationToken.None);
        
        var matchInDb = await _dbContext.Matches.FindAsync(match.Id);
        Assert.Null(matchInDb);
    }
    
    /// <summary>
    /// Tests that an exception is thrown when the provided match ID does not exist in the database.
    /// </summary>
    [Fact(DisplayName = "Throw exception if match not found")]
    public async Task Should_Throw_Exception_If_Match_Not_Found()
    {
        var command = new AcceptMatchCommand(1);

        var act = async () => await _acceptMatchUseCase.Handle(command, CancellationToken.None);
        
        var exception = await Assert.ThrowsAsync<KeyNotFoundException>(act);
        Assert.Equal("Match not found.", exception.Message);
    }
}

