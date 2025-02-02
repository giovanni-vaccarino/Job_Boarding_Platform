using backend.Business.Match.GetMatchesUseCase;
using backend.Data;
using backend.Shared.Enums;

namespace UnitTests.UseCases.Match;

public class GetMatchesUseCaseTests
{
    private readonly AppDbContext _dbContext;
    private readonly IsolatedUseCaseTestServices<GetMatchesUseCase> _services;
    private readonly GetMatchesUseCase _getMatchesUseCase;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="GetMatchesUseCaseTests"/> class.
    /// Sets up the database context, the mapper and use case instance for testing.
    /// </summary>
    public GetMatchesUseCaseTests()
    {
        _services = new IsolatedUseCaseTestServices<GetMatchesUseCase>("GetMatchesUseCaseTests");
        _dbContext = _services.DbContext;
        _getMatchesUseCase = (GetMatchesUseCase)Activator.CreateInstance(
            typeof(GetMatchesUseCase), _dbContext, _services.Mapper)!;
    }
    
    /// <summary>
    /// Tests that company matches are correctly retrieved based on the profile ID.
    /// </summary>
    [Fact(DisplayName = "Successfully retrieve matches for a company")]
    public async Task Should_Retrieve_Company_Matches_Successfully()
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

        _dbContext.Internships.Add(internship);
        await _dbContext.SaveChangesAsync();

        var studentId = 1;
        
        var match = new backend.Data.Entities.Match
        {
            HasInvite = false,
            StudentId = studentId,
            InternshipId = internship.Id
        };
        
        _dbContext.Matches.Add(match);
        await _dbContext.SaveChangesAsync();
    
        var query = new GetMatchesQuery (company.Id, ProfileType.Company);
    
        var result = await _getMatchesUseCase.Handle(query, CancellationToken.None);
    
        Assert.NotNull(result);
    }
    
    /// <summary>
    /// Tests that student matches are correctly retrieved based on the profile ID.
    /// </summary>
    [Fact(DisplayName = "Successfully retrieve matches for a student")]
    public async Task Should_Retrieve_Student_Matches_Successfully()
    {
        var studentId = 1;
    
        var match = new backend.Data.Entities.Match
        {
            HasInvite = false,
            StudentId = studentId,
            InternshipId = 3
        };
        
        _dbContext.Matches.Add(match);
        await _dbContext.SaveChangesAsync();
    
        var query = new GetMatchesQuery (studentId, ProfileType.Student);
    
        var result = await _getMatchesUseCase.Handle(query, CancellationToken.None);
    
        Assert.NotNull(result);
        Assert.All(result, dto => Assert.Equal(studentId, dto.Student.Id));
    }
}