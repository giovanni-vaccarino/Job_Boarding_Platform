using AutoMapper;
using backend.Data;
using backend.Service.Contracts.Internship;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace backend.Business.Company.UpdateInternshipUseCase;

/// <summary>
/// Handles the update of an existing internship for a company.
/// </summary>
public class UpdateInternshipUseCase : IRequestHandler<UpdateInternshipCommand, InternshipDto>
{
    private readonly AppDbContext _dbContext;
    private readonly IMapper _mapper;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateInternshipUseCase"/> class.
    /// </summary>
    /// <param name="dbContext">The application database context.</param>
    /// <param name="mapper">The AutoMapper instance for object mapping.</param>
    public UpdateInternshipUseCase(AppDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }
    
    /// <summary>
    /// Handles the command to update an internship associated with a specific company.
    /// </summary>
    /// <param name="request">The command containing the company ID, internship ID, and updated internship details.</param>
    /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
    /// <returns>An <see cref="InternshipDto"/> object containing the updated details of the internship.</returns>
    /// <exception cref="KeyNotFoundException">Thrown if the internship with the specified ID does not exist for the given company.</exception>
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