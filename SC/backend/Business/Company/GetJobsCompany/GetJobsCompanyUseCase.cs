using backend.Data;

namespace backend.Business.Company.GetJobsCompany;
using AutoMapper;

public class GetActivityUseCase
{
    private readonly AppDbContext _dbContext;
    private readonly ILogger _logger;
    private readonly IMapper _mapper;
    
    public GetActivityUseCase(AppDbContext dbContext, ILogger logger, IMapper mapper)
    {
        _dbContext = dbContext;
        _logger = logger;
        _mapper = mapper;
    }
    
    
}