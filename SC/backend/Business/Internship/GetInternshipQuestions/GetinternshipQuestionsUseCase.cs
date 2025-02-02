using AutoMapper;
using backend.Data;
using backend.Service.Contracts.Company;
using backend.Service.Contracts.Internship;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace backend.Business.Internship.GetInternshipQuestions;

public class GetInternshipQuestionsUseCase : IRequestHandler<GetInternshipQuestionsQuery, List<QuestionDto>>
{
    private readonly AppDbContext _dbContext;
    private readonly IMapper _mapper;

    public GetInternshipQuestionsUseCase(AppDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<List<QuestionDto>> Handle(GetInternshipQuestionsQuery request, CancellationToken cancellationToken)
    {
        var internshipId = request.InternshipId;

        // Retrieve question IDs for the given internship ID
        var questionIds = await _dbContext.InternshipQuestions
            .Where(iq => iq.InternshipId == internshipId)
            .Select(iq => iq.QuestionId)
            .ToListAsync(cancellationToken);

        // Retrieve questions using the question IDs
        var questions = await _dbContext.Questions
            .Where(q => questionIds.Contains(q.Id))
            .ToListAsync(cancellationToken);

        return _mapper.Map<List<QuestionDto>>(questions);
    }
}