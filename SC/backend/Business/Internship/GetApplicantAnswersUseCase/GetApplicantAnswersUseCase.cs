using AutoMapper;
using backend.Data;
using backend.Service.Contracts.Company;
using backend.Service.Contracts.Internship;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace backend.Business.Internship.GetApplicantAnswersUseCase;

public class GetApplicantAnswersUseCase : IRequestHandler<GetApplicantAnswersQuery, List<ApplicantResponseDto>>
{
    private readonly AppDbContext _dbContext;
    private readonly IMapper _mapper;
    
    public GetApplicantAnswersUseCase(AppDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }
    
    public async Task<List<ApplicantResponseDto>> Handle(GetApplicantAnswersQuery request, CancellationToken cancellationToken)
    {
        var applicationId = request.ApplicationId;

        var answers = await _dbContext.Answers
            .Where(answer => answer.ApplicationId == applicationId)
            .Include(answer => answer.InternshipQuestion)
            .ThenInclude(iq => iq.Question)
            .ToListAsync(cancellationToken);

        if (!answers.Any())
        {
            throw new KeyNotFoundException($"No answers found for Application ID {applicationId}.");
        }

        var responseDto = answers.Select(answer => new ApplicantResponseDto
        {
            Question = _mapper.Map<QuestionDto>(answer.InternshipQuestion.Question),
            Answer = answer.StudentAnswer
        }).ToList();

        return responseDto;
    }
}