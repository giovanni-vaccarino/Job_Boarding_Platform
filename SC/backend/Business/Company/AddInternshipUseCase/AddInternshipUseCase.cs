using AutoMapper;
using backend.Data;
using backend.Data.Entities;
using backend.Service.Contracts.Internship;
using backend.Shared.MatchingBackgroundService;
using MediatR;

namespace backend.Business.Company.AddInternshipUseCase;

/// <summary>
/// Handles the addition of an internship for a company.
/// </summary>
public class AddInternshipUseCase : IRequestHandler<AddInternshipCommand, InternshipDto>
{
    private readonly AppDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly ILogger<AddInternshipUseCase> _logger;
    private readonly IJobQueue _jobQueue;

    /// <summary>
    /// Initializes a new instance of the <see cref="AddInternshipUseCase"/> class.
    /// </summary>
    /// <param name="dbContext">The application database context.</param>
    /// <param name="mapper">The AutoMapper instance for object mapping.</param>
    /// <param name="logger">The logger instance for logging operations.</param>
    /// <param name="backgroundService">The background service for task execution.</param>
    public AddInternshipUseCase(
        AppDbContext dbContext,
        IMapper mapper,
        ILogger<AddInternshipUseCase> logger,
        IJobQueue jobQueue)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _logger = logger;
        _jobQueue = jobQueue;
    }
    
    /// <summary>
    /// Handles the command to add an internship for a company.
    /// </summary>
    /// <param name="request">The command containing the internship details and associated data.</param>
    /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
    /// <returns>An <see cref="InternshipDto"/> object containing the details of the added internship.</returns>
    /// <exception cref="Exception">Thrown if the operation encounters issues while persisting data.</exception>
    public async Task<InternshipDto> Handle(AddInternshipCommand request, CancellationToken cancellationToken)
    {
        var companyId = request.Id;
        var jobDetails = request.Dto.JobDetails;
        var questions = request.Dto.Questions;
        var existingQuestionIds = request.Dto.ExistingQuestions;

        var internship = _mapper.Map<Data.Entities.Internship>(jobDetails);
        internship.CompanyId = companyId;
        internship.CreatedAt = DateTime.UtcNow;
        internship.UpdatedAt = DateTime.UtcNow;

        await _dbContext.Internships.AddAsync(internship, cancellationToken);

        var newQuestions = _mapper.Map<List<Question>>(questions);
        foreach (var question in newQuestions)
        {
            _logger.LogInformation($"Adding question of type: {question.Type}");
            question.CompanyId = companyId;
            question.CreatedAt = DateTime.UtcNow;
            question.UpdatedAt = DateTime.UtcNow;
        }

        await _dbContext.Questions.AddRangeAsync(newQuestions, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        var internshipQuestions = newQuestions
            .Select(q => new InternshipQuestion
            {
                InternshipId = internship.Id,
                QuestionId = q.Id,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            })
            .Concat(existingQuestionIds.Select(existingId => new InternshipQuestion
            {
                InternshipId = internship.Id,
                QuestionId = existingId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }))
            .ToList();

        await _dbContext.InternshipQuestions.AddRangeAsync(internshipQuestions, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        var internshipDto = _mapper.Map<InternshipDto>(internship);

        return internshipDto;
    }
}