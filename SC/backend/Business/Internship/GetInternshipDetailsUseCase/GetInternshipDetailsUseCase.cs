using AutoMapper;
using backend.Data;
using backend.Service.Contracts.Internship;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace backend.Business.Internship.GetInternshipDetailsUseCase;

public class GetInternshipDetailsUseCase : IRequestHandler<GetInternshipDetailsQuery, InternshipDto>
{
    private readonly AppDbContext _dbContext;
    private readonly IMapper _mapper;
    
    public GetInternshipDetailsUseCase(AppDbContext mediator, IMapper mapper)
    {
        _dbContext = mediator;
        _mapper = mapper;
    }
    
    public async Task<InternshipDto> Handle(GetInternshipDetailsQuery request, CancellationToken cancellationToken)
    {
        var internshipId = request.Id;
        
        var internship = await _dbContext.Internships
            .FirstOrDefaultAsync(i => i.Id == internshipId, cancellationToken);

        if (internship == null)
        {
            throw new KeyNotFoundException($"Internship with ID {internshipId} not found.");
        }

        return _mapper.Map<InternshipDto>(internship);
    }
}