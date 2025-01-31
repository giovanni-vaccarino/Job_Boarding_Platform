using backend.Data.Entities;
using backend.Business.Internship.GetApplicationsUseCase;
using backend.Data;
using backend.Shared.Enums;

namespace UnitTests.UseCases.Internship;

/// <summary>
/// Unit tests for the <see cref="GetApplicationsUseCase"/>.
/// </summary>
public class GetApplicationsUseCaseTests
{
    private readonly IsolatedUseCaseTestServices<GetApplicationsUseCase> _services;
    private readonly AppDbContext _dbContext;
    private readonly GetApplicationsUseCase _getApplicationsUseCase;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetApplicationsUseCaseTests"/> class.
    /// Sets up the isolated services, database context, and use case instance for testing.
    /// </summary>
    public GetApplicationsUseCaseTests()
    {
        _services = new IsolatedUseCaseTestServices<GetApplicationsUseCase>("GetApplicationsUseCaseTests");
        _dbContext = _services.DbContext;
        _getApplicationsUseCase = (GetApplicationsUseCase)Activator.CreateInstance(
            typeof(GetApplicationsUseCase), _dbContext, _services.Mapper)!;
    }

    /// <summary>
    /// Tests that applications for a specific internship are successfully retrieved.
    /// </summary>
    [Fact(DisplayName = "Retrieve applications for an internship successfully")]
    public async Task Should_Retrieve_Applications_For_Internship_Successfully()
    {
        var student1 = new backend.Data.Entities.Student
        {
            Name = "Test Student",
            Cf = "AAABBB00H00A000A",
            UserId = 2,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        
        var student2 = new backend.Data.Entities.Student
        {
            Name = "Test Student",
            Cf = "AAABBB00H00A000A",
            UserId = 2,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        
        _dbContext.Students.AddRange(student1, student2);
        await _dbContext.SaveChangesAsync();

        
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

        var application1 = new Application
        {
            ApplicationStatus = ApplicationStatus.Screening,
            StudentId = student1.Id,
            InternshipId = internship.Id,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var application2 = new Application
        {
            ApplicationStatus = ApplicationStatus.Screening,
            StudentId = student2.Id,
            InternshipId = internship.Id,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _dbContext.Applications.AddRange(application1, application2);
        await _dbContext.SaveChangesAsync();

        var query = new GetApplicationsQuery(internship.Id);

        var result = await _getApplicationsUseCase.Handle(query, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Contains(result, app => app.ApplicationStatus == ApplicationStatus.Screening);
        Assert.Contains(result, app => app.ApplicationStatus == ApplicationStatus.Screening);
    }

    /// <summary>
    /// Tests that an empty list is returned when no applications are associated with the internship.
    /// </summary>
    [Fact(DisplayName = "Return empty list when no applications for internship")]
    public async Task Should_Return_Empty_List_When_No_Applications_For_Internship()
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

        var query = new GetApplicationsQuery(internship.Id);

        var result = await _getApplicationsUseCase.Handle(query, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Empty(result);
    }
}
