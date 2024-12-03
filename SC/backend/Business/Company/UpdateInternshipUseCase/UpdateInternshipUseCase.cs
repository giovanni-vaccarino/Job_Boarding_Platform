using AutoMapper;
using backend.Data;
using backend.Service.Contracts.Internship;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace backend.Business.Company.UpdateInternshipUseCase;

public class UpdateInternshipUseCase : IRequestHandler<UpdateInternshipCommand, InternshipDto>
{
    private readonly AppDbContext _dbContext;
    private readonly IMapper _mapper;
    
    public UpdateInternshipUseCase(AppDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }
    
    public async Task<InternshipDto> Handle(UpdateInternshipCommand request, CancellationToken cancellationToken)
    {
        var companyId = request.Id;
        var internshipId = request.InternshipId;
        var updateInternshipDto = request.Dto.JobDetails;
        
        var internship = await _dbContext.Internships
            .FirstOrDefaultAsync(i => i.Id == internshipId && i.CompanyId == companyId, cancellationToken);

        if (internship == null)
        {
            throw new KeyNotFoundException($"Internship with ID {internshipId} for Company ID {companyId} not found.");
        }

        internship.Title = updateInternshipDto.Title;
        internship.Duration = updateInternshipDto.Duration;
        internship.Description = updateInternshipDto.Description;
        internship.ApplicationDeadline = updateInternshipDto.ApplicationDeadline;
        internship.Location = updateInternshipDto.Location;
        internship.JobCategory = updateInternshipDto.JobCategory;
        internship.JobType = updateInternshipDto.JobType;
        internship.Requirements = updateInternshipDto.Requirements;
        internship.UpdatedAt = DateTime.UtcNow;

        _dbContext.Internships.Update(internship);
        await _dbContext.SaveChangesAsync(cancellationToken);
        
        return _mapper.Map<InternshipDto>(internship);
    }
}