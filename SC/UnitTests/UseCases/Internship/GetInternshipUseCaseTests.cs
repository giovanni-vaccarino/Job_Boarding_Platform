using backend.Business.Internship.GetInternshipUseCase;
using backend.Data;
using backend.Shared.Enums;

namespace UnitTests.UseCases.Internship;

/// <summary>
/// Unit tests for the <see cref="GetInternshipUseCase"/>.
/// </summary>
public class GetInternshipUseCaseTests
{
    private readonly IsolatedUseCaseTestServices<GetInternshipUseCase> _services;
    private readonly AppDbContext _dbContext;
    private readonly GetInternshipUseCase _getInternshipUseCase;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetInternshipUseCaseTests"/> class.
    /// Sets up the isolated services, database context, and use case instance for testing.
    /// </summary>
    public GetInternshipUseCaseTests()
    {
        _services = new IsolatedUseCaseTestServices<GetInternshipUseCase>("GetInternshipUseCaseTests");
        _dbContext = _services.DbContext;
        _getInternshipUseCase = (GetInternshipUseCase)Activator.CreateInstance(
            typeof(GetInternshipUseCase), _dbContext, _services.Mapper)!;
    }

    /// <summary>
    /// Tests that all internships are successfully retrieved.
    /// </summary>
    [Fact(DisplayName = "Retrieve all internships successfully")]
    public async Task Should_Retrieve_All_Internships_Successfully()
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
        _dbContext.SaveChanges();
        
        var internship1 = new backend.Data.Entities.Internship
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

        var internship2 = new backend.Data.Entities.Internship
        {
            Title = "Data Analyst Intern",
            Company = company,
            CompanyId = company.Id,
            Description = "Analyze and interpret data.",
            ApplicationDeadline = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(45)),
            Location = "Onsite",
            Duration = DurationType.SixToTwelveMonths,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _dbContext.Internships.AddRange(internship1, internship2);
        await _dbContext.SaveChangesAsync();

        var query = new GetInternshipQuery();

        var result = await _getInternshipUseCase.Handle(query, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Contains(result, i => i.Title == internship1.Title);
        Assert.Contains(result, i => i.Title == internship2.Title);
    }

    /// <summary>
    /// Tests that an empty list is returned when no internships are available.
    /// </summary>
    [Fact(DisplayName = "Return empty list when no internships are available")]
    public async Task Should_Return_Empty_List_When_No_Internships_Available()
    {
        var query = new GetInternshipQuery();

        var result = await _getInternshipUseCase.Handle(query, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Empty(result);
    }
}
