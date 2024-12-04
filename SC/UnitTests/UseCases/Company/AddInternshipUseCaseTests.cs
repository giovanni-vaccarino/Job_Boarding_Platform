using backend.Business.Company.AddInternshipUseCase;
using backend.Data;
using backend.Data.Entities;
using backend.Service.Contracts.Company;
using backend.Shared.Enums;
using Microsoft.EntityFrameworkCore;

namespace UnitTests.UseCases.Company;

/// <summary>
/// Unit tests for the <see cref="AddInternshipUseCase"/>.
/// </summary>
public class AddInternshipUseCaseTests
{
    private readonly IsolatedUseCaseTestServices<AddInternshipUseCase> _services;
    private readonly AppDbContext _dbContext;
    private readonly AddInternshipUseCase _addInternshipUseCase;

    /// <summary>
    /// Initializes a new instance of the <see cref="AddInternshipUseCaseTests"/> class.
    /// Sets up the isolated services, database context, logger, and use case instance for testing.
    /// </summary>
    public AddInternshipUseCaseTests()
    {
        _services = new IsolatedUseCaseTestServices<AddInternshipUseCase>("AddInternshipUseCaseTests");
        _dbContext = _services.DbContext;
        _addInternshipUseCase = (AddInternshipUseCase)Activator.CreateInstance(
            typeof(AddInternshipUseCase), _dbContext, _services.Mapper, _services.LoggerMock.Object)!;
    }

    /// <summary>
    /// Tests that an internship is successfully added to the database.
    /// </summary>
    [Fact(DisplayName = "Successfully add an internship with new and existing questions")]
    public async Task Should_Add_Internship_Successfully()
    {
        var companyId = 1;
        var existingQuestion = new Question
        {
            Title = "Do you have experience in software development?",
            Type = QuestionType.OpenQuestion,
            CompanyId = companyId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        _dbContext.Questions.Add(existingQuestion);
        await _dbContext.SaveChangesAsync();

        var addInternshipCommand = new AddInternshipCommand(companyId,
            new AddInternshipDto
            {
                JobDetails = new AddJobDetailsDto
                {
                    Title = "Software Developer Intern",
                    Duration = DurationType.TwoToThreeMonths,
                    Description = "An exciting opportunity to develop software solutions.",
                    ApplicationDeadline = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(30)),
                    Location = "Remote",
                    JobCategory = JobCategory.Technology,
                    JobType = JobType.FullTime,
                    Requirements = new List<string> { "C#", "SQL", "Problem-Solving" }
                },
                Questions = new List<AddQuestionDto>
                {
                    new AddQuestionDto
                    {
                        Title = "What is your preferred programming language?",
                        QuestionType = QuestionType.MultipleChoice,
                        Options = new List<string> { "C#", "Java", "Python" }
                    }
                },
                ExistingQuestions = new List<int> { existingQuestion.Id }
            });

        var result = await _addInternshipUseCase.Handle(addInternshipCommand, CancellationToken.None);
        
        Assert.NotNull(result);
        Assert.Equal(addInternshipCommand.Dto.JobDetails.Title, result.Title);
        Assert.Equal(addInternshipCommand.Dto.JobDetails.Duration, result.Duration);
        Assert.Equal(addInternshipCommand.Dto.JobDetails.Location, result.Location);
        Assert.Equal(addInternshipCommand.Dto.JobDetails.Description, result.Description);

        var internshipInDb = await _dbContext.Internships.Include(i => i.InternshipQuestions).FirstOrDefaultAsync(i => i.Id == result.Id);
        Assert.NotNull(internshipInDb);
        Assert.Equal(2, internshipInDb!.InternshipQuestions.Count);

        var newQuestion = internshipInDb.InternshipQuestions.FirstOrDefault(iq => iq.QuestionId != existingQuestion.Id)?.Question;
        Assert.NotNull(newQuestion);
        Assert.Equal("What is your preferred programming language?", newQuestion!.Title);
        Assert.Equal(QuestionType.MultipleChoice, newQuestion.Type);
        Assert.Equal(new List<string> { "C#", "Java", "Python" }, newQuestion.Options);

        var existingLink = internshipInDb.InternshipQuestions.FirstOrDefault(iq => iq.QuestionId == existingQuestion.Id);
        Assert.NotNull(existingLink);
    }
}
