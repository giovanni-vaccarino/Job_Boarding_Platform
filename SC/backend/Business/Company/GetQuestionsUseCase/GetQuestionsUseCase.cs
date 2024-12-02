using AutoMapper;
using backend.Data;
using backend.Service.Contracts.Company;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace backend.Business.Company.GetQuestionsUseCase;

public class GetQuestionsUseCase : IRequestHandler<GetQuestionsQuery, List<QuestionDto>>
{
    private readonly AppDbContext _dbContext;
    private readonly IMapper _mapper;
    
    public GetQuestionsUseCase(AppDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }
    
    public async Task<List<QuestionDto>> Handle(GetQuestionsQuery request, CancellationToken cancellationToken)
    {
        var companyId = request.Id;
        
        var questions = await _dbContext.Questions
            .Where(q => q.CompanyId == companyId)
            .ToListAsync(cancellationToken);
        
        return _mapper.Map<List<QuestionDto>>(questions);
    }
}