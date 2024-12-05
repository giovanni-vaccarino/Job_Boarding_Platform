using backend.Business.Student.GetApplicationsUseCase;
using backend.Data;
using backend.Data.Entities;
using backend.Shared.Enums;

namespace UnitTests.UseCases.Student;

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
    /// Tests that applications for a specific student are retrieved successfully.
    /// </summary>
    [Fact(DisplayName = "Retrieve applications for a student")]
    public async Task Should_Retrieve_Applications_For_Student()
    {
        var student = new backend.Data.Entities.Student
        {
            Name = "Test Student",
            Cf = "123456789",
            CvPath = "/path/to/cv",
            UserId = 1,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        
        var company = new backend.Data.Entities.Company
        {
            Name = "Test Company",
            VatNumber = "123456789",
            UserId = 1,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _dbContext.Companies.Add(company);
        _dbContext.Students.Add(student);
        _dbContext.SaveChanges();
        
        var internship = new backend.Data.Entities.Internship
        {
            Title = "Software Developer Intern",
            CompanyId = company.Id,
            Company = company,
            Description = "Develop software solutions.",
            ApplicationDeadline = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(30)),
            Location = "Remote",
            Duration = DurationType.ThreeToSixMonths,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _dbContext.Internships.Add(internship);
        await _dbContext.SaveChangesAsync();

        var application = new Application
        {
            ApplicationStatus = ApplicationStatus.Screening,
            StudentId = student.Id,
            InternshipId = internship.Id,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _dbContext.Applications.Add(application);
        await _dbContext.SaveChangesAsync();

        var query = new GetApplicationsQuery(student.Id);

        var result = await _getApplicationsUseCase.Handle(query, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Single(result);

        var fetchedApplication = result.First();
        Assert.Equal(application.Id, fetchedApplication.Id);
        Assert.Equal(application.ApplicationStatus, fetchedApplication.ApplicationStatus);
        Assert.Equal(internship.Title, fetchedApplication.Internship.Title);
    }

    /// <summary>
    /// Tests that an empty list is returned when a student has no applications.
    /// </summary>
    [Fact(DisplayName = "Return empty list when student has no applications")]
    public async Task Should_Return_Empty_List_When_Student_Has_No_Applications()
    {
        var student = new backend.Data.Entities.Student
        {
            Name = "Test Student",
            Cf = "123456789",
            CvPath = "/path/to/cv",
            UserId = 1,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        
        _dbContext.Students.Add(student);
        _dbContext.SaveChanges();
        
        var query = new GetApplicationsQuery(student.Id);

        var result = await _getApplicationsUseCase.Handle(query, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Empty(result);
    }
}
