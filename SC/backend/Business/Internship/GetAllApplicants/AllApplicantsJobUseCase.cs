using AutoMapper;
using backend.Data;
using backend.Data.Entities;
using backend.Service.Contracts.Company;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace backend.Business.Internship.GetAllApplicants;

public class AllApplicantsJobUseCase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly AppDbContext _dbContext;
    
    public AllApplicantsJobUseCase(IMediator mediator, IMapper mapper, AppDbContext dbContext)
    {
        _dbContext = dbContext;
        _mediator = mediator;
        _mapper = mapper;
    }
    
    public async Task<List<SingleApplicantToInternshipDto>> Handle(QueryAllApplicantsJob request, CancellationToken cancellationToken)
    {
        var internshipId = request.Id;

        var applications = await _dbContext.Applications
            .Where(a => a.InternshipId == internshipId) // Fixed the filter to use InternshipId
            .Include(a => a.Student) // Include the Student entity for mapping the name
            .ToListAsync(cancellationToken);
        
        var applicantsToEvaluate = _mapper.Map<List<SingleApplicantToInternshipDto>>(applications);
        
        return applicantsToEvaluate;
    }
}