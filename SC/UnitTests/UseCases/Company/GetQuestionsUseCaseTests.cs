using backend.Business.Company.GetQuestionsUseCase;
using backend.Data;
using backend.Data.Entities;
using backend.Service.Contracts.Company;
using backend.Shared.Enums;
using Microsoft.EntityFrameworkCore;

namespace UnitTests.UseCases.Company;

/// <summary>
/// Unit tests for the <see cref="GetQuestionsUseCase"/>.
/// </summary>
public class GetQuestionsUseCaseTests
{
    private readonly IsolatedUseCaseTestServices<GetQuestionsUseCase> _services;
    private readonly AppDbContext _dbContext;
    private readonly GetQuestionsUseCase _getQuestionsUseCase;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetQuestionsUseCaseTests"/> class.
    /// Sets up the isolated services, database context, and use case instance for testing.
    /// </summary>
    public GetQuestionsUseCaseTests()
    {
        _services = new IsolatedUseCaseTestServices<GetQuestionsUseCase>("GetQuestionsUseCaseTests");
        _dbContext = _services.DbContext;
        _getQuestionsUseCase = (GetQuestionsUseCase)Activator.CreateInstance(
            typeof(GetQuestionsUseCase), _dbContext, _services.Mapper)!;
    }

    /// <summary>
    /// Tests that a multiple-choice question is added and fetched correctly, including its options.
    /// </summary>
    [Fact(DisplayName = "Add and Fetch Multiple Choice Question Successfully")]
    public async Task Should_Add_And_Fetch_Multiple_Choice_Question()
    {
        var companyId = 1;
        var multipleChoiceQuestion = new Question
        {
            Title = "What is your favorite programming language?",
            Type = QuestionType.MultipleChoice,
            Options = new List<string> { "C#", "Java", "Python" },
            CompanyId = companyId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _dbContext.Questions.Add(multipleChoiceQuestion);
        await _dbContext.SaveChangesAsync();

        var query = new GetQuestionsQuery(companyId);
        var result = await _getQuestionsUseCase.Handle(query, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Single(result);
        var fetchedQuestion = result.First();

        Assert.Equal(multipleChoiceQuestion.Title, fetchedQuestion.Title);
        Assert.Equal(multipleChoiceQuestion.Type, fetchedQuestion.QuestionType);
        Assert.Equal(multipleChoiceQuestion.Options, fetchedQuestion.Options);
    }
}
