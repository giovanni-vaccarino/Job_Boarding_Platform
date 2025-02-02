using backend.Business.Feedback.AddPlatformFeedbackUseCase;
using backend.Data;
using backend.Service.Contracts.Feedback;
using backend.Shared.Enums;

namespace UnitTests.UseCases.Feedback;

/// <summary>
/// Unit tests for the <see cref="AddPlatformFeedbackUseCase"/> class, validating platform feedback functionality.
/// </summary>
public class AddPlatformFeedbackUseCaseTests
{
    private readonly IsolatedUseCaseTestServices<AddPlatformFeedbackUseCase> _services;
    private readonly AppDbContext _dbContext;
    private readonly AddPlatformFeedbackUseCase _addPlatformFeedbackUseCase;

    /// <summary>
    /// Initializes a new instance of the class, setting up the testing environment.
    /// </summary>
    public AddPlatformFeedbackUseCaseTests()
    {
        _services = new IsolatedUseCaseTestServices<AddPlatformFeedbackUseCase>("AddPlatformFeedbackUseCaseTests");
        _dbContext = _services.DbContext;
        _addPlatformFeedbackUseCase = (AddPlatformFeedbackUseCase)Activator.CreateInstance(
            typeof(AddPlatformFeedbackUseCase), _dbContext, _services.LoggerMock.Object)!;
    }

    /// <summary>
    /// Tests successful addition of platform feedback for a student.
    /// </summary>
    [Fact(DisplayName = "Should add platform feedback for a student")]
    public async Task Should_Add_Platform_Feedback_For_Student()
    {
        var student = new backend.Data.Entities.Student
        {
            Name = "Test Student",
            Cf = "123456789",
            CvPath = "/student/1",
            UserId = 1,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        _dbContext.Students.Add(student);
        await _dbContext.SaveChangesAsync();

        var feedbackDto = new AddPlatformFeedbackDto
        {
            ProfileId = 1,
            Actor = ProfileType.Student,
            Text = "Great platform!",
            Rating = Rating.FiveStars
        };
        var command = new AddPlatformFeedbackCommand(feedbackDto);

        await _addPlatformFeedbackUseCase.Handle(command, CancellationToken.None);

        var feedback = _dbContext.PlatformFeedbacks.FirstOrDefault();
        Assert.NotNull(feedback);
        Assert.Equal("Great platform!", feedback.Text);
        Assert.Equal(Rating.FiveStars, feedback.Rating);
        Assert.Equal(1, feedback.UserId);
    }

    /// <summary>
    /// Tests successful addition of platform feedback for a company.
    /// </summary>
    [Fact(DisplayName = "Should add platform feedback for a company")]
    public async Task Should_Add_Platform_Feedback_For_Company()
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
        await _dbContext.SaveChangesAsync();

        var feedbackDto = new AddPlatformFeedbackDto
        {
            ProfileId = 1,
            Actor = ProfileType.Company,
            Text = "Excellent experience!",
            Rating = Rating.FourStars
        };
        var command = new AddPlatformFeedbackCommand(feedbackDto);

        await _addPlatformFeedbackUseCase.Handle(command, CancellationToken.None);

        var feedback = _dbContext.PlatformFeedbacks.FirstOrDefault();
        Assert.NotNull(feedback);
        Assert.Equal("Excellent experience!", feedback.Text);
        Assert.Equal(Rating.FourStars, feedback.Rating);
        Assert.Equal(1, feedback.UserId);
    }
}
