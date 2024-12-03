using AutoMapper;
using backend.Data;
using backend.Service.Contracts.Internship;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace backend.Business.Internship.GetInternshipUseCase;

public class GetInternshipUseCase: IRequestHandler<GetInternshipQuery, List<InternshipDto>>
{
    private readonly AppDbContext _dbContext;
    private readonly IMapper _mapper;

    public GetInternshipUseCase(AppDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }
    
    public async Task<List<InternshipDto>> Handle(GetInternshipQuery request, CancellationToken cancellationToken)
    { 
        var internships = await _dbContext.Internships
            .ToListAsync(cancellationToken);

        return _mapper.Map<List<InternshipDto>>(internships);
    }
}