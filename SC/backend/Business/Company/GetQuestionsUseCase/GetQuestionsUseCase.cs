using AutoMapper;
using backend.Data;
using backend.Service.Contracts.Company;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace backend.Business.Company.GetQuestionsUseCase;

/// <summary>
/// Handles the retrieval of questions associated with a company.
/// </summary>
public class GetQuestionsUseCase : IRequestHandler<GetQuestionsQuery, List<QuestionDto>>
{
    private readonly AppDbContext _dbContext;
    private readonly IMapper _mapper;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="GetQuestionsUseCase"/> class.
    /// </summary>
    /// <param name="dbContext">The application database context.</param>
    /// <param name="mapper">The AutoMapper instance for object mapping.</param>
    public GetQuestionsUseCase(AppDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }
    
    /// <summary>
    /// Handles the query to retrieve all questions associated with a specific company.
    /// </summary>
    /// <param name="request">The query containing the company ID.</param>
    /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
    /// <returns>A list of <see cref="QuestionDto"/> objects containing the details of the questions.</returns>
    public async Task<List<QuestionDto>> Handle(GetQuestionsQuery request, CancellationToken cancellationToken)
    {
        var companyId = request.Id;
        
        var questions = await _dbContext.Questions
            .Where(q => q.CompanyId == companyId)
            .ToListAsync(cancellationToken);
        
        return _mapper.Map<List<QuestionDto>>(questions);
    }
}