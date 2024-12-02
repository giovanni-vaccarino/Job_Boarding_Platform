using System.Net;
using AutoMapper;
using backend.Data;
using backend.Data.Entities;
using backend.Service.Contracts.Company;
using backend.Shared.Enums;
using MediatR;

namespace backend.Business.Company.AddQuestionUseCase;

public class AddQuestionUseCase : IRequestHandler<AddQuestionCommand, QuestionDto>
{
    private readonly AppDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly ILogger<AddQuestionUseCase> _logger;
    
    public AddQuestionUseCase(AppDbContext dbContext, IMapper mapper, ILogger<AddQuestionUseCase> logger)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _logger = logger;
    }

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