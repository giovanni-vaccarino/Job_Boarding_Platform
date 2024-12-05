using backend.Business.Internship.GetApplicantAnswersUseCase;
using backend.Data;
using backend.Data.Entities;
using backend.Shared.Enums;

namespace UnitTests.UseCases.Internship;

/// <summary>
/// Unit tests for the <see cref="GetApplicantAnswersUseCase"/>.
/// </summary>
public class GetApplicantAnswersUseCaseTests
{
    private readonly IsolatedUseCaseTestServices<GetApplicantAnswersUseCase> _services;
    private readonly AppDbContext _dbContext;
    private readonly GetApplicantAnswersUseCase _getApplicantAnswersUseCase;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetApplicantAnswersUseCaseTests"/> class.
    /// Sets up the isolated services, database context, and use case instance for testing.
    /// </summary>
    public GetApplicantAnswersUseCaseTests()
    {
        _services = new IsolatedUseCaseTestServices<GetApplicantAnswersUseCase>("GetApplicantAnswersUseCaseTests");
        _dbContext = _services.DbContext;
        _getApplicantAnswersUseCase = (GetApplicantAnswersUseCase)Activator.CreateInstance(
            typeof(GetApplicantAnswersUseCase), _dbContext, _services.Mapper)!;
    }

    /// <summary>
    /// Tests that applicant answers for a specific application are successfully retrieved.
    /// </summary>
    [Fact(DisplayName = "Retrieve applicant answers successfully")]
    public async Task Should_Retrieve_Applicant_Answers_Successfully()
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

        var question = new Question
        {
            Title = "What is your favorite programming language?",
            Type = QuestionType.OpenQuestion,
            CompanyId = 1,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        
        _dbContext.Internships.Add(internship);
        _dbContext.Questions.Add(question);
        await _dbContext.SaveChangesAsync();

        var internshipQuestion = new InternshipQuestion
        {
            InternshipId = internship.Id,
            QuestionId = question.Id,
            Internship = internship,
            Question = question
        };

        var application = new Application
        {
            InternshipId = internship.Id,
            ApplicationStatus = ApplicationStatus.LastEvaluation,
            StudentId = 1,
            Internship = internship,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        
        _dbContext.InternshipQuestions.Add(internshipQuestion);
        _dbContext.Applications.Add(application);
        await _dbContext.SaveChangesAsync();

        var answer = new Answer
        {
            ApplicationId = application.Id,
            Application = application,
            InternshipQuestion = internshipQuestion,
            InternshipQuestionId = internshipQuestion.Id,
            StudentAnswer = ["C#"],
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        
        _dbContext.Answers.Add(answer);
        await _dbContext.SaveChangesAsync();

        var query = new GetApplicantAnswersQuery(application.Id);

        var result = await _getApplicantAnswersUseCase.Handle(query, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal("What is your favorite programming language?", result.First().Question.Title);
        Assert.Equal("C#", result.First().Answer[0]);
    }

    /// <summary>
    /// Tests that a KeyNotFoundException is thrown when no answers are associated with the application.
    /// </summary>
    [Fact(DisplayName = "Throw exception when no answers for application")]
    public async Task Should_Throw_Exception_When_No_Answers_For_Application()
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

        var application = new Application
        {
            InternshipId = internship.Id,
            ApplicationStatus = ApplicationStatus.OnlineAssessment,
            StudentId = 1,
            Internship = internship,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _dbContext.Applications.Add(application);
        await _dbContext.SaveChangesAsync();

        var query = new GetApplicantAnswersQuery(application.Id);

        var act = async () => await _getApplicantAnswersUseCase.Handle(query, CancellationToken.None);

        var exception = await Assert.ThrowsAsync<KeyNotFoundException>(act);
        Assert.Equal($"No answers found for Application ID {application.Id}.", exception.Message);
    }
}
