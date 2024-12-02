using AutoMapper;
using backend.Data;
using backend.Service.Contracts.Internship;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace backend.Business.Company.GetInternshipsUseCase;

public class GetInternshipsUseCase : IRequestHandler<GetInternshipsQuery, List<InternshipDto>>
{
    private readonly AppDbContext _dbContext;
    private readonly IMapper _mapper;
    
    public GetInternshipsUseCase(AppDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }
    
    public async Task<List<InternshipDto>> Handle(GetInternshipsQuery request, CancellationToken cancellationToken)
    {
        var companyId = request.Id;
        var internshipId = request.InternshipId;
        
        var query = _dbContext.Internships.AsQueryable();
        
        query = query.Where(i => i.CompanyId == companyId);

        if (internshipId.HasValue)
        {
            query = query.Where(i => i.Id == internshipId);
        }

        var internships = await query.ToListAsync(cancellationToken);

        return _mapper.Map<List<InternshipDto>>(internships);
    }
}
