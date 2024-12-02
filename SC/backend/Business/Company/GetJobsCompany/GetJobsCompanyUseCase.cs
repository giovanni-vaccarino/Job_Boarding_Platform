using backend.Data;
using backend.Service.Contracts.Company;
using Microsoft.EntityFrameworkCore;

namespace backend.Business.Company.GetJobsCompany;

public class GetJobsCompanyUseCase
{
    private readonly ILogger<GetJobsCompanyUseCase> _logger;
    private readonly AppDbContext _dbContext;
    
    public GetJobsCompanyUseCase(AppDbContext dbContext, ILogger<GetJobsCompanyUseCase> logger)
    {
        _logger = logger;
        _dbContext = dbContext;
    }
    
    public async Task<List<CompanyJobsDto>> Handle(GetJobsCompanyQuery request, CancellationToken cancellationToken)
    {
        var companyId = request.Id;
        
        var jobs = await _dbContext.Jobs
            .Where(j => j.CompanyId == companyId)
            .ToListAsync(cancellationToken);
        // TODO
        // var jobDtos = jobs.Select(j => new CompanyJobsDto
        // {
        //     Title = j.Title,
        //     ApplicationReceived = j.NumberOfApplicants,
        //     JobType = j.JobType,
        //     Location = j.Location
        // }).ToList();
        
        return new List<CompanyJobsDto>();
    }
}
