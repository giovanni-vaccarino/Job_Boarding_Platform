using AutoMapper;
using backend.Data;
using backend.Service.Contracts.Company;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Business.Internship.GetInternshipDetails;

public class QueryInternshipDetails
{
    private readonly AppDbContext _dbContext;
    private readonly IMapper _mapper;
    
    public QueryInternshipDetails(AppDbContext mediator, IMapper mapper)
    {
        _dbContext = mediator;
        _mapper = mapper;
    }
    
    public async Task<JobDetailsDto> GetInternshipDetails(int id, CancellationToken cancellationToken)
    {
        var internship = await _dbContext.Internships
            .Where(c => c.Id == id)
            .FirstOrDefaultAsync(cancellationToken);

        if (internship == null)
        {
            throw new KeyNotFoundException("Internship not found.");
        }
        
        var internshipDto = _mapper.Map<JobDetailsDto>(internship);
        return internshipDto;
    }
}