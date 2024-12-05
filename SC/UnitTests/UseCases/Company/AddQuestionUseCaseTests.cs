using backend.Business.Company.AddQuestionUseCase;
using backend.Data;
using backend.Service.Contracts.Company;
using backend.Shared.Enums;
using Microsoft.EntityFrameworkCore;

namespace UnitTests.UseCases.Company;


/// <summary>
/// Unit tests for the <see cref="AddQuestionUseCase"/>.
/// </summary>
public class AddQuestionUseCaseTests
{
    private readonly IsolatedUseCaseTestServices<AddQuestionUseCase> _services;
    private readonly AppDbContext _dbContext;
    private readonly AddQuestionUseCase _addQuestionUseCase;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="AddQuestionUseCaseTests"/> class.
    /// Sets up the isolated services, database context, logger, and use case instance for testing.
    /// </summary>
    public AddQuestionUseCaseTests()
    {
        _services = new IsolatedUseCaseTestServices<AddQuestionUseCase>("AddQuestionUseCaseTests");
        _dbContext = _services.DbContext;
        _addQuestionUseCase = (AddQuestionUseCase)Activator.CreateInstance(
            typeof(AddQuestionUseCase), _dbContext, _services.Mapper, _services.LoggerMock.Object)!;
    }
    
    /// <summary>
    /// Tests that a question is successfully added when valid data is provided.
    /// </summary>
    [Fact(DisplayName = "Successfully add a question")]
    public async Task Should_Add_Question_Successfully()
    {
        var addQuestionCommand = new AddQuestionCommand(1,
            new AddQuestionDto
            {
                Title = "What is your preferred programming language?",
                QuestionType = QuestionType.MultipleChoice,
                Options = new List<string> { "C#", "Java", "Python" }
            });

        var result = await _addQuestionUseCase.Handle(addQuestionCommand, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(addQuestionCommand.Dto.Title, result.Title);
        Assert.Equal(addQuestionCommand.Dto.QuestionType, result.QuestionType);
        Assert.Equal(addQuestionCommand.Dto.Options, result.Options);

        var questionInDb = await _dbContext.Questions.FirstOrDefaultAsync(q => q.Id == result.Id);
        Assert.NotNull(questionInDb);
        Assert.Equal(addQuestionCommand.Dto.Title, questionInDb.Title);
    }
    
    /// <summary>
    /// Tests that an exception is thrown when options are provided for a non-multiple-choice question.
    /// </summary>
    [Fact(DisplayName = "Throw exception if options provided for non-multiple-choice question")]
    public async Task Should_Throw_Exception_If_Options_Provided_For_Non_Multiple_Choice()
    {
        var addQuestionCommand = new AddQuestionCommand(1,
            new AddQuestionDto
            {
                Title = "Is this a valid question?",
                QuestionType = QuestionType.TrueOrFalse,
                Options = new List<string> { "Yes", "No" }
            });

        var act = async () => await _addQuestionUseCase.Handle(addQuestionCommand, CancellationToken.None);

        var exception = await Assert.ThrowsAsync<HttpRequestException>(act);
        Assert.Equal("Options can only be provided for multiple-choice questions.", exception.Message);
    }

    /// <summary>
    /// Tests that an exception is thrown when less than three options are provided for a multiple-choice question.
    /// </summary>
    [Fact(DisplayName = "Throw exception if less than three options provided for multiple-choice question")]
    public async Task Should_Throw_Exception_If_Less_Than_Three_Options_For_Multiple_Choice()
    {
        var addQuestionCommand = new AddQuestionCommand(1,
            new AddQuestionDto
            {
                Title = "What is your favorite programming language?",
                QuestionType = QuestionType.MultipleChoice,
                Options = new List<string> { "C#" }
            });

        var act = async () => await _addQuestionUseCase.Handle(addQuestionCommand, CancellationToken.None);

        var exception = await Assert.ThrowsAsync<HttpRequestException>(act);
        Assert.Equal("A multiple-choice question must have at least three options. Please provide at least three distinct choices.", exception.Message);
    }
}