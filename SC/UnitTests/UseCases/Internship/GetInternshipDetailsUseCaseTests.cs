using backend.Business.Internship.GetInternshipDetailsUseCase;
using backend.Data;
using backend.Shared.Enums;

namespace UnitTests.UseCases.Internship;

/// <summary>
/// Unit tests for the <see cref="GetInternshipDetailsUseCase"/>.
/// </summary>
public class GetInternshipDetailsUseCaseTests
{
    private readonly IsolatedUseCaseTestServices<GetInternshipDetailsUseCase> _services;
    private readonly AppDbContext _dbContext;
    private readonly GetInternshipDetailsUseCase _getInternshipDetailsUseCase;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetInternshipDetailsUseCaseTests"/> class.
    /// Sets up the isolated services, database context, and use case instance for testing.
    /// </summary>
    public GetInternshipDetailsUseCaseTests()
    {
        _services = new IsolatedUseCaseTestServices<GetInternshipDetailsUseCase>("GetInternshipDetailsUseCaseTests");
        _dbContext = _services.DbContext;
        _getInternshipDetailsUseCase = (GetInternshipDetailsUseCase)Activator.CreateInstance(
            typeof(GetInternshipDetailsUseCase), _dbContext, _services.Mapper)!;
    }

    /// <summary>
    /// Tests that internship details are successfully retrieved.
    /// </summary>
    [Fact(DisplayName = "Retrieve internship details successfully")]
    public async Task Should_Retrieve_Internship_Details_Successfully()
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

        var query = new GetInternshipDetailsQuery(internship.Id);

        var result = await _getInternshipDetailsUseCase.Handle(query, CancellationToken.None);
        
        Assert.NotNull(result);
        Assert.Equal(internship.Title, result.Title);
        Assert.Equal(internship.Description, result.Description);
        Assert.Equal(internship.Location, result.Location);
        Assert.Equal(internship.Duration, result.Duration);
    }

    /// <summary>
    /// Tests that a KeyNotFoundException is thrown when the internship is not found.
    /// </summary>
    [Fact(DisplayName = "Throw exception when internship not found")]
    public async Task Should_Throw_Exception_When_Internship_Not_Found()
    {
        var query = new GetInternshipDetailsQuery(1);

        var act = async () => await _getInternshipDetailsUseCase.Handle(query, CancellationToken.None);

        var exception = await Assert.ThrowsAsync<KeyNotFoundException>(act);
        Assert.Equal("Internship with ID 1 not found.", exception.Message);
    }
}
