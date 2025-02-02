using backend.Business.Feedback.AddInternshipFeedbackUseCase;
using backend.Data;
using backend.Data.Entities;
using backend.Service.Contracts.Feedback;
using backend.Shared.Enums;

namespace UnitTests.UseCases.Feedback;

/// <summary>
/// Unit tests for the <see cref="AddInternshipFeedbackUseCase"/> class, validating internship feedback functionality.
/// </summary>
public class AddInternshipFeedbackUseCaseTests
{
    private readonly IsolatedUseCaseTestServices<AddInternshipFeedbackUseCase> _services;
    private readonly AppDbContext _dbContext;
    private readonly AddInternshipFeedbackUseCase _addInternshipFeedbackUseCase;

    /// <summary>
    /// Initializes a new instance of the class, setting up the testing environment.
    /// </summary>
    public AddInternshipFeedbackUseCaseTests()
    {
        _services = new IsolatedUseCaseTestServices<AddInternshipFeedbackUseCase>("AddInternshipFeedbackUseCaseTests");
        _dbContext = _services.DbContext;
        _addInternshipFeedbackUseCase = (AddInternshipFeedbackUseCase)Activator.CreateInstance(
            typeof(AddInternshipFeedbackUseCase), _dbContext)!;
    }

    /// <summary>
    /// Tests successful addition of internship feedback by a student.
    /// </summary>
    [Fact(DisplayName = "Should add internship feedback by a student")]
    public async Task Should_Add_Internship_Feedback_By_Student()
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
        
        var application = new Application
        {
            ApplicationStatus = ApplicationStatus.Screening,
            InternshipId = internship.Id,
            StudentId = student.Id,
        };
        _dbContext.Applications.Add(application);
        await _dbContext.SaveChangesAsync();

        var feedbackDto = new AddInternshipFeedbackDto
        {
            ApplicationId = application.Id,
            ProfileId = student.Id,
            Actor = ProfileType.Student,
            Text = "Great internship opportunity!",
            Rating = Rating.FiveStars
        };
        var command = new AddInternshipFeedbackCommand(feedbackDto);

        await _addInternshipFeedbackUseCase.Handle(command, CancellationToken.None);

        var feedback = _dbContext.InternshipFeedbacks.FirstOrDefault();
        Assert.NotNull(feedback);
        Assert.Equal("Great internship opportunity!", feedback.Text);
        Assert.Equal(Rating.FiveStars, feedback.Rating);
        Assert.Equal(application.Id, feedback.ApplicationId);
        Assert.Equal(ProfileType.Student, feedback.Actor);
    }

    /// <summary>
    /// Tests successful addition of internship feedback by a company.
    /// </summary>
    [Fact(DisplayName = "Should add internship feedback by a company")]
    public async Task Should_Add_Internship_Feedback_By_Company()
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
        
        var application = new Application
        {
            ApplicationStatus = ApplicationStatus.Screening,
            InternshipId = internship.Id,
            StudentId = student.Id,
        };
        _dbContext.Applications.Add(application);
        await _dbContext.SaveChangesAsync();
        
        var feedbackDto = new AddInternshipFeedbackDto
        {
            ApplicationId = application.Id,
            ProfileId = company.Id,
            Actor = ProfileType.Company,
            Text = "Excellent candidate!",
            Rating = Rating.FourStars
        };
        var command = new AddInternshipFeedbackCommand(feedbackDto);

        await _addInternshipFeedbackUseCase.Handle(command, CancellationToken.None);

        var feedback = _dbContext.InternshipFeedbacks.FirstOrDefault();
        Assert.NotNull(feedback);
        Assert.Equal("Excellent candidate!", feedback.Text);
        Assert.Equal(Rating.FourStars, feedback.Rating);
        Assert.Equal(application.Id, feedback.ApplicationId);
        Assert.Equal(ProfileType.Company, feedback.Actor);
    }

    /// <summary>
    /// Tests that an exception is thrown when the application does not exist.
    /// </summary>
    [Fact(DisplayName = "Should throw exception when application does not exist")]
    public async Task Should_Throw_Exception_When_Application_Does_Not_Exist()
    {
        var feedbackDto = new AddInternshipFeedbackDto
        {
            ApplicationId = 3,
            ProfileId = 1,
            Actor = ProfileType.Student,
            Text = "Feedback text",
            Rating = Rating.ThreeStars
        };
        var command = new AddInternshipFeedbackCommand(feedbackDto);

        var act = async () => await _addInternshipFeedbackUseCase.Handle(command, CancellationToken.None);

        var exception = await Assert.ThrowsAsync<KeyNotFoundException>(act);
        Assert.Equal("Application not found.", exception.Message);
    }

    /// <summary>
    /// Tests that an exception is thrown when a company not involved in the application tries to add feedback.
    /// </summary>
    [Fact(DisplayName = "Should throw exception when company not involved tries to add feedback")]
    public async Task Should_Throw_Exception_When_Company_Not_Involved()
    {
        var company = new backend.Data.Entities.Company
        {
            Name = "Test Company",
            VatNumber = "123456789",
            UserId = 1,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        var company2 = new backend.Data.Entities.Company
        {
            Name = "Test Company 2",
            VatNumber = "123456749",
            UserId = 2,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _dbContext.Companies.Add(company);
        _dbContext.Companies.Add(company2);
        _dbContext.SaveChanges();
        
        var student = new backend.Data.Entities.Student
        {
            Name = "Test Student",
            Cf = "123456789",
            CvPath = "/student/1",
            UserId = 3,
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
        
        var application = new Application
        {
            ApplicationStatus = ApplicationStatus.Screening,
            InternshipId = internship.Id,
            StudentId = student.Id,
        };
        _dbContext.Applications.Add(application);
        await _dbContext.SaveChangesAsync();

        var feedbackDto = new AddInternshipFeedbackDto
        {
            ApplicationId = application.Id,
            ProfileId = company2.Id,
            Actor = ProfileType.Company,
            Text = "Feedback text",
            Rating = Rating.FourStars
        };
        var command = new AddInternshipFeedbackCommand(feedbackDto);

        var act = async () => await _addInternshipFeedbackUseCase.Handle(command, CancellationToken.None);

        var exception = await Assert.ThrowsAsync<UnauthorizedAccessException>(act);
        Assert.Equal("Company is not involved in this application.", exception.Message);
    }

    /// <summary>
    /// Tests that an exception is thrown when a student not involved in the application tries to add feedback.
    /// </summary>
    [Fact(DisplayName = "Should throw exception when student not involved tries to add feedback")]
    public async Task Should_Throw_Exception_When_Student_Not_Involved()
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
        var student2 = new backend.Data.Entities.Student
        {
            Name = "Test Student",
            Cf = "123456789",
            CvPath = "/student/1",
            UserId = 3,
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
        
        var application = new Application
        {
            ApplicationStatus = ApplicationStatus.Screening,
            InternshipId = internship.Id,
            StudentId = student.Id,
        };
        _dbContext.Applications.Add(application);
        await _dbContext.SaveChangesAsync();

        var feedbackDto = new AddInternshipFeedbackDto
        {
            ApplicationId = application.Id,
            ProfileId = student2.Id, 
            Actor = ProfileType.Student,
            Text = "Feedback text",
            Rating = Rating.FiveStars
        };
        var command = new AddInternshipFeedbackCommand(feedbackDto);

        var act = async () => await _addInternshipFeedbackUseCase.Handle(command, CancellationToken.None);

        var exception = await Assert.ThrowsAsync<UnauthorizedAccessException>(act);
        Assert.Equal("Student is not involved in this application.", exception.Message);
    }
}
