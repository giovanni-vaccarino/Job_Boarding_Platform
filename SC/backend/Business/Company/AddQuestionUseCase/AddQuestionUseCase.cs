using System.Net;
using AutoMapper;
using backend.Data;
using backend.Data.Entities;
using backend.Service.Contracts.Company;
using backend.Shared.Enums;
using MediatR;

namespace backend.Business.Company.AddQuestionUseCase;

/// <summary>
/// Handles the addition of a question for a company.
/// </summary>
public class AddQuestionUseCase : IRequestHandler<AddQuestionCommand, QuestionDto>
{
    private readonly AppDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly ILogger<AddQuestionUseCase> _logger;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="AddQuestionUseCase"/> class.
    /// </summary>
    /// <param name="dbContext">The application database context.</param>
    /// <param name="mapper">The AutoMapper instance for object mapping.</param>
    /// <param name="logger">The logger instance for logging operations.</param>
    public AddQuestionUseCase(AppDbContext dbContext, IMapper mapper, ILogger<AddQuestionUseCase> logger)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _logger = logger;
    }

    /// <summary>
    /// Handles the command to add a question for a company.
    /// </summary>
    /// <param name="request">The command containing the question details and associated data.</param>
    /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
    /// <returns>A <see cref="QuestionDto"/> object containing the details of the added question.</returns>
    /// <exception cref="HttpRequestException">Thrown if the question validation fails.</exception>
    public async Task<QuestionDto> Handle(AddQuestionCommand request, CancellationToken cancellationToken)
    {
        var companyId = request.Id;
        var addQuestionDto = request.Dto;

        ValidateQuestion(addQuestionDto);
        
        var question = _mapper.Map<Question>(addQuestionDto);
        question.CompanyId = companyId;
        question.CreatedAt = DateTime.UtcNow;
        question.UpdatedAt = DateTime.UtcNow;

        await _dbContext.Questions.AddAsync(question, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        
        return _mapper.Map<QuestionDto>(question);
    }


    private void ValidateQuestion(AddQuestionDto dto)
    {
        if (dto.QuestionType != QuestionType.MultipleChoice && dto.Options.Count > 0)
        {
            _logger.LogWarning($"Options can only be provided for multiple-choice questions.");
            throw new HttpRequestException("Options can only be provided for multiple-choice questions.", null, HttpStatusCode.BadRequest);
        }

        if (dto.QuestionType == QuestionType.MultipleChoice && dto.Options.Count < 3)
        {
            _logger.LogWarning($"A multiple-choice question must have at least three options. Provided options: {dto.Options.Count}.");
            throw new HttpRequestException(
                "A multiple-choice question must have at least three options. Please provide at least three distinct choices.",
                null,
                HttpStatusCode.BadRequest
            );
        }
    }
}