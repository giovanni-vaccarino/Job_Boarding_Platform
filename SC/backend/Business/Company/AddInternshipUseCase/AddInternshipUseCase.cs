using AutoMapper;
using backend.Data;
using backend.Data.Entities;
using backend.Service.Contracts.Internship;
using MediatR;

namespace backend.Business.Company.AddInternshipUseCase;

public class AddInternshipUseCase : IRequestHandler<AddInternshipCommand, InternshipDto>
{
    private readonly AppDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly ILogger<AddInternshipUseCase> _logger;

    public AddInternshipUseCase(AppDbContext dbContext, IMapper mapper, ILogger<AddInternshipUseCase> logger)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _logger = logger;
    }
    
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