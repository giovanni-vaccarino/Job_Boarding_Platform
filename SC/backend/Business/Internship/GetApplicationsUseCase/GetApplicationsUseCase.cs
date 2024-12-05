using AutoMapper;
using backend.Data;
using backend.Service.Contracts.Internship;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace backend.Business.Internship.GetApplicationsUseCase;

public class GetApplicationsUseCase : IRequestHandler<GetApplicationsQuery, List<ApplicationDto>>
{
    private readonly AppDbContext _dbContext;
    private readonly IMapper _mapper;
    
    public GetApplicationsUseCase(AppDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }
    
    public async Task<List<ApplicationDto>> Handle(GetApplicationsQuery request, CancellationToken cancellationToken)
    {
        var internshipId = request.InternshipId;
   
        var applications = await _dbContext.Applications
            .Where(app => app.InternshipId == internshipId)
            .Include(app => app.Internship)
            .ToListAsync(cancellationToken);
        
        return _mapper.Map<List<ApplicationDto>>(applications);
    }
}