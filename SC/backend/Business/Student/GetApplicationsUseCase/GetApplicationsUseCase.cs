using AutoMapper;
using backend.Data;
using backend.Service.Contracts.Internship;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace backend.Business.Student.GetApplicationsUseCase;

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
        var studentId = request.Id;
        
        var applications = await _dbContext.Applications
            .Include(a => a.Internship)
            .Where(a => a.StudentId == studentId)
            .ToListAsync(cancellationToken);

        return _mapper.Map<List<ApplicationDto>>(applications);
    }
}