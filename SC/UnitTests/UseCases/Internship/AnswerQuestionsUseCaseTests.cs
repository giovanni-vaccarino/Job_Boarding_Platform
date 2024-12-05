using backend.Business.Internship.AnswerQuestionsUseCase;
using backend.Data;
using backend.Data.Entities;
using backend.Service.Contracts.Internship;
using backend.Shared.Enums;

namespace UnitTests.UseCases.Internship;

/// <summary>
/// Unit tests for the <see cref="AnswerQuestionsUseCase"/>.
/// </summary>
public class AnswerQuestionsUseCaseTests
{
    private readonly IsolatedUseCaseTestServices<AnswerQuestionsUseCase> _services;
    private readonly AppDbContext _dbContext;
    private readonly AnswerQuestionsUseCase _answerQuestionsUseCase;

    /// <summary>
    /// Initializes a new instance of the <see cref="AnswerQuestionsUseCaseTests"/> class.
    /// Sets up the isolated services, database context, and use case instance for testing.
    /// </summary>
    public AnswerQuestionsUseCaseTests()
    {
        _services = new IsolatedUseCaseTestServices<AnswerQuestionsUseCase>("AnswerQuestionsUseCaseTests");
        _dbContext = _services.DbContext;
        _answerQuestionsUseCase = (AnswerQuestionsUseCase)Activator.CreateInstance(
            typeof(AnswerQuestionsUseCase), _dbContext, _services.Mapper)!;
    }

    /// <summary>
    /// Tests successfully answering internship questions.
    /// </summary>
    [Fact(DisplayName = "Successfully answer internship questions")]
    public async Task Should_Answer_Internship_Questions_Successfully()
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
            ApplicationStatus = ApplicationStatus.OnlineAssessment,
            StudentId = 1,
            Internship = internship,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        
        _dbContext.InternshipQuestions.Add(internshipQuestion);
        _dbContext.Applications.Add(application);
        await _dbContext.SaveChangesAsync();

        var command = new AnswerQuestionsCommand(application.Id,
            new AnswerQuestionsDto
            {
                Questions = new List<SingleAnswerQuestion>
                {
                    new SingleAnswerQuestion
                    {
                        QuestionId = question.Id,
                        Answer = new List<string> { "C#" }
                    }
                }
            });

        var result = await _answerQuestionsUseCase.Handle(command, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(application.Id, result.Id);
        Assert.Equal(ApplicationStatus.LastEvaluation, application.ApplicationStatus);
    }

    /// <summary>
    /// Tests that an exception is thrown when the number of answers does not match the number of internship questions.
    /// </summary>
    [Fact(DisplayName = "Throw exception when number of answers does not match")]
    public async Task Should_Throw_Exception_When_Answer_Count_Does_Not_Match()
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
            Duration = DurationType.ThreeToSixMonths
        };

        var question = new Question
        {
            Title = "What is your favorite programming language?",
            Type = QuestionType.OpenQuestion,
            CompanyId = 1
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
            ApplicationStatus = ApplicationStatus.OnlineAssessment,
            StudentId = 1,
            Internship = internship
        };

        _dbContext.InternshipQuestions.Add(internshipQuestion);
        _dbContext.Applications.Add(application);
        await _dbContext.SaveChangesAsync();

        var command = new AnswerQuestionsCommand(application.Id,
            new AnswerQuestionsDto
            {
                Questions = new List<SingleAnswerQuestion>()
            });

        var exception = await Assert.ThrowsAsync<ArgumentException>(() =>
            _answerQuestionsUseCase.Handle(command, CancellationToken.None));

        Assert.Equal("The number of provided answers does not match the number of internship questions.", exception.Message);
    }

    /// <summary>
    /// Tests that an exception is thrown for an invalid question.
    /// </summary>
    [Fact(DisplayName = "Throw exception for invalid question")]
    public async Task Should_Throw_Exception_For_Invalid_Question()
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
            Duration = DurationType.ThreeToSixMonths
        };

        var question = new Question
        {
            Title = "What is your favorite programming language?",
            Type = QuestionType.OpenQuestion,
            CompanyId = 1
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
            ApplicationStatus = ApplicationStatus.OnlineAssessment,
            StudentId = 1,
            Internship = internship
        };
        
        _dbContext.InternshipQuestions.Add(internshipQuestion);
        _dbContext.Applications.Add(application);
        await _dbContext.SaveChangesAsync();

        var command = new AnswerQuestionsCommand(application.Id,
            new AnswerQuestionsDto
            {
                Questions = new List<SingleAnswerQuestion>
                {
                    new SingleAnswerQuestion
                    {
                        QuestionId = 5,
                        Answer = new List<string> { "C#" }
                    }
                }
            });

        var exception = await Assert.ThrowsAsync<ArgumentException>(() =>
            _answerQuestionsUseCase.Handle(command, CancellationToken.None));

        Assert.Equal("Question with ID 5 is not part of the internship.", exception.Message);
    }
    
    /// <summary>
    /// Tests that an exception is thrown for open questions when no answer is provided.
    /// </summary>
    [Fact(DisplayName = "Throw exception when no answer provided for open question")]
    public async Task Should_Throw_Exception_When_No_Answer_Provided_For_Open_Question()
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
            Duration = DurationType.ThreeToSixMonths
        };

        var question = new Question
        {
            Title = "What is your favorite programming language?",
            Type = QuestionType.OpenQuestion,
            CompanyId = company.Id
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
            ApplicationStatus = ApplicationStatus.OnlineAssessment,
            StudentId = 1,
            Internship = internship
        };

        _dbContext.InternshipQuestions.Add(internshipQuestion);
        _dbContext.Applications.Add(application);
        await _dbContext.SaveChangesAsync();

        var command = new AnswerQuestionsCommand(application.Id,
            new AnswerQuestionsDto
            {
                Questions = new List<SingleAnswerQuestion>
                {
                    new SingleAnswerQuestion
                    {
                        QuestionId = question.Id,
                        Answer = new List<string>()
                    }
                }
            });

        var exception = await Assert.ThrowsAsync<ArgumentException>(() =>
            _answerQuestionsUseCase.Handle(command, CancellationToken.None));

        Assert.Equal("Open questions must have exactly one answer.", exception.Message);
    }
    
    /// <summary>
    /// Tests that an exception is thrown for multiple-choice questions when no valid index is provided.
    /// </summary>
    [Fact(DisplayName = "Throw exception when no valid index for multiple-choice question")]
    public async Task Should_Throw_Exception_When_No_Valid_Index_For_Multiple_Choice_Question()
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
            Duration = DurationType.ThreeToSixMonths
        };

        var question = new Question
        {
            Title = "Select all applicable programming languages.",
            Type = QuestionType.MultipleChoice,
            CompanyId = company.Id
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
            ApplicationStatus = ApplicationStatus.OnlineAssessment,
            StudentId = 1,
            Internship = internship
        };

        _dbContext.InternshipQuestions.Add(internshipQuestion);
        _dbContext.Applications.Add(application);
        await _dbContext.SaveChangesAsync();

        var command = new AnswerQuestionsCommand(application.Id,
            new AnswerQuestionsDto
            {
                Questions = new List<SingleAnswerQuestion>
                {
                    new SingleAnswerQuestion
                    {
                        QuestionId = question.Id,
                        Answer = new List<string> { "Invalid" }
                    }
                }
            });

        var exception = await Assert.ThrowsAsync<ArgumentException>(() =>
            _answerQuestionsUseCase.Handle(command, CancellationToken.None));

        Assert.Equal("Multiple-choice questions must have one or more valid answer indices.", exception.Message);
    }
    
    /// <summary>
    /// Tests that an exception is thrown for true/false questions with invalid answer.
    /// </summary>
    [Fact(DisplayName = "Throw exception for invalid answer for true/false question")]
    public async Task Should_Throw_Exception_For_Invalid_Answer_For_TrueOrFalse_Question()
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
            Duration = DurationType.ThreeToSixMonths
        };

        var question = new Question
        {
            Title = "Is C# your favorite language?",
            Type = QuestionType.TrueOrFalse,
            CompanyId = company.Id
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
            ApplicationStatus = ApplicationStatus.OnlineAssessment,
            StudentId = 1,
            Internship = internship
        };

        _dbContext.InternshipQuestions.Add(internshipQuestion);
        _dbContext.Applications.Add(application);
        await _dbContext.SaveChangesAsync();

        var command = new AnswerQuestionsCommand(application.Id,
            new AnswerQuestionsDto
            {
                Questions = new List<SingleAnswerQuestion>
                {
                    new SingleAnswerQuestion
                    {
                        QuestionId = question.Id,
                        Answer = new List<string> { "Not sure" }
                    }
                }
            });

        var exception = await Assert.ThrowsAsync<ArgumentException>(() =>
            _answerQuestionsUseCase.Handle(command, CancellationToken.None));

        Assert.Equal("The answer to a true or false question must be either 'true' or 'false'.", exception.Message);
    }
    
    /// <summary>
    /// Tests that an exception is thrown if the application status is not compatible with the following action.
    /// </summary>
    [Fact(DisplayName = "Throw exception for invalid state of the application")]
    public async Task Should_Throw_Exception_For_Invalid_Application_Status()
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
            Duration = DurationType.ThreeToSixMonths
        };

        var question = new Question
        {
            Title = "Is C# your favorite language?",
            Type = QuestionType.TrueOrFalse,
            CompanyId = company.Id
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
            ApplicationStatus = ApplicationStatus.Screening,
            StudentId = 1,
            Internship = internship
        };

        _dbContext.InternshipQuestions.Add(internshipQuestion);
        _dbContext.Applications.Add(application);
        await _dbContext.SaveChangesAsync();

        var command = new AnswerQuestionsCommand(application.Id,
            new AnswerQuestionsDto
            {
                Questions = new List<SingleAnswerQuestion>
                {
                    new SingleAnswerQuestion
                    {
                        QuestionId = question.Id,
                        Answer = new List<string> { "Not sure" }
                    }
                }
            });

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _answerQuestionsUseCase.Handle(command, CancellationToken.None));

        Assert.Equal("The application status is not valid for answering assessment questions.", exception.Message);
    }
}
