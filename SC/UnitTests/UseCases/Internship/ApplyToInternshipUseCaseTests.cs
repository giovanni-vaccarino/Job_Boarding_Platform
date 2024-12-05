using backend.Business.Internship.ApplyToInternshipUseCase;
using backend.Data;
using backend.Data.Entities;
using backend.Shared.Enums;

namespace UnitTests.UseCases.Internship;

/// <summary>
/// Unit tests for the <see cref="ApplyToInternshipUseCase"/>.
/// </summary>
public class ApplyToInternshipUseCaseTests
{
    private readonly IsolatedUseCaseTestServices<ApplyToInternshipUseCase> _services;
    private readonly AppDbContext _dbContext;
    private readonly ApplyToInternshipUseCase _applyToInternshipUseCase;

    /// <summary>
    /// Initializes a new instance of the <see cref="ApplyToInternshipUseCaseTests"/> class.
    /// Sets up the isolated services, database context, and use case instance for testing.
    /// </summary>
    public ApplyToInternshipUseCaseTests()
    {
        _services = new IsolatedUseCaseTestServices<ApplyToInternshipUseCase>("ApplyToInternshipUseCaseTests");
        _dbContext = _services.DbContext;
        _applyToInternshipUseCase = (ApplyToInternshipUseCase)Activator.CreateInstance(
            typeof(ApplyToInternshipUseCase), _dbContext, _services.Mapper)!;
    }

    /// <summary>
    /// Tests a successful application.
    /// </summary>
    [Fact(DisplayName = "Successfully apply to internship")]
    public async Task Should_Apply_To_Internship_Successfully()
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

        var command = new ApplyToInternshipCommand(student.Id, internship.Id);

        var result = await _applyToInternshipUseCase.Handle(command, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(student.Id, result.Id);
        Assert.Equal(ApplicationStatus.Screening, result.ApplicationStatus);
        Assert.Equal(internship.Title, result.Internship.Title);
    }
    
    /// <summary>
    /// Tests that an exception is thrown when the student's profile is incomplete.
    /// </summary>
    [Fact(DisplayName = "Throw exception when student profile is incomplete")]
    public async Task Should_Throw_Exception_When_Student_Profile_Is_Incomplete()
    {
        var student = new backend.Data.Entities.Student { Name = null, Cf = null, CvPath = null, UserId = 1 };
        _dbContext.Students.Add(student);
        await _dbContext.SaveChangesAsync();

        var command = new ApplyToInternshipCommand(student.Id, 1);

        var act = async () => await _applyToInternshipUseCase.Handle(command, CancellationToken.None);

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(act);
        Assert.Equal("The student must have a name, a valid CF, and an uploaded CV to apply for an internship.", exception.Message);
    }

    /// <summary>
    /// Tests that an exception is thrown when the internship is not found.
    /// </summary>
    [Fact(DisplayName = "Throw exception when internship not found")]
    public async Task Should_Throw_Exception_When_Internship_Not_Found()
    {
        var student = new backend.Data.Entities.Student
        {
            Name = "Test Student",
            Cf = "123456789",
            CvPath = "/path/to/cv",
            UserId = 1
        };
        _dbContext.Students.Add(student);
        await _dbContext.SaveChangesAsync();

        var command = new ApplyToInternshipCommand(student.Id, 5);

        var act = async () => await _applyToInternshipUseCase.Handle(command, CancellationToken.None);

        var exception = await Assert.ThrowsAsync<KeyNotFoundException>(act);
        Assert.Equal("Internship with ID 5 not found.", exception.Message);
    }

    /// <summary>
    /// Tests that an exception is thrown when the application deadline has passed.
    /// </summary>
    [Fact(DisplayName = "Throw exception when application deadline has passed")]
    public async Task Should_Throw_Exception_When_Application_Deadline_Has_Passed()
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
        
        var student = new backend.Data.Entities.Student
        {
            Name = "Test Student",
            Cf = "123456789",
            CvPath = "/students/1",
            UserId = 2
        };
        var internship = new backend.Data.Entities.Internship
        {
            Title = "Software Developer Intern",
            Company = company,
            CompanyId = company.Id,
            ApplicationDeadline = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-1)),
            Location = "Remote",
            Duration = DurationType.ThreeToSixMonths,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            Description = "Develop software solutions.",
        };

        _dbContext.Students.Add(student);
        _dbContext.Internships.Add(internship);
        await _dbContext.SaveChangesAsync();

        var command = new ApplyToInternshipCommand(student.Id, internship.Id);

        var act = async () => await _applyToInternshipUseCase.Handle(command, CancellationToken.None);

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(act);
        Assert.Equal("The application deadline for this internship has passed.", exception.Message);
    }

    /// <summary>
    /// Tests that an exception is thrown when the student has already applied to the internship.
    /// </summary>
    [Fact(DisplayName = "Throw exception when duplicate application")]
    public async Task Should_Throw_Exception_When_Student_Has_Already_Applied()
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
        
        var student = new backend.Data.Entities.Student
        {
            Name = "Test Student",
            Cf = "123456789",
            CvPath = "/students/1",
            UserId = 2
        };
        var internship = new backend.Data.Entities.Internship
        {
            Title = "Software Developer Intern",
            Company = company,
            CompanyId = company.Id,
            ApplicationDeadline = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(5)),
            Location = "Remote",
            Duration = DurationType.ThreeToSixMonths,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            Description = "Develop software solutions.",
        };

        _dbContext.Students.Add(student);
        _dbContext.Internships.Add(internship);
        await _dbContext.SaveChangesAsync();

        var application = new Application
        {
            ApplicationStatus = ApplicationStatus.Screening,
            StudentId = student.Id,
            InternshipId = internship.Id
        };

        _dbContext.Applications.Add(application);
        await _dbContext.SaveChangesAsync();

        var command = new ApplyToInternshipCommand(student.Id, internship.Id);

        var act = async () => await _applyToInternshipUseCase.Handle(command, CancellationToken.None);

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(act);
        Assert.Equal("The student has already applied to this internship.", exception.Message);
    }
}
