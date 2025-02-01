using AutoMapper;
using backend.Data;
using backend.Service.Contracts.Feedback;
using backend.Service.Contracts.Internship;
using backend.Shared.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace backend.Business.Internship.GetInternshipDetailsUseCase;

/// <summary>
/// Handles the retrieval of detailed information for a specific internship.
/// </summary>
public class GetInternshipDetailsUseCase : IRequestHandler<GetInternshipDetailsQuery, InternshipDto>
{
    private readonly AppDbContext _dbContext;
    private readonly IMapper _mapper;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="GetInternshipDetailsUseCase"/> class.
    /// </summary>
    /// <param name="mediator">The application database context.</param>
    /// <param name="mapper">The AutoMapper instance for object mapping.</param>
    public GetInternshipDetailsUseCase(AppDbContext mediator, IMapper mapper)
    {
        _dbContext = mediator;
        _mapper = mapper;
    }
    
    /// <summary>
    /// Handles the query to retrieve details for a specific internship.
    /// </summary>
    /// <param name="request">The query containing the ID of the internship to retrieve.</param>
    /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
    /// <returns>An <see cref="InternshipDto"/> object containing the details of the internship.</returns>
    /// <exception cref="KeyNotFoundException">Thrown if the internship with the specified ID is not found.</exception>
    public async Task<InternshipDto> Handle(GetInternshipDetailsQuery request, CancellationToken cancellationToken)
    {
        var internshipId = request.Id;
        
        var internship = await _dbContext.Internships
            .FirstOrDefaultAsync(i => i.Id == internshipId, cancellationToken);

        if (internship == null)
        {
            throw new KeyNotFoundException($"Internship with ID {internshipId} not found.");
        }
        
        var companyId = internship.CompanyId;

        var allInternships = await _dbContext.Internships
            .Where(i => i.CompanyId == companyId)
            .ToListAsync(cancellationToken);
        
        var allInternshipIds = allInternships.Select(i => i.Id).ToList();

        var internshipFeedbacks = await _dbContext.InternshipFeedbacks
            .Where(f => allInternshipIds.Contains(f.Application.InternshipId) && f.Actor == ProfileType.Student)
            .ToListAsync(cancellationToken);
        
        var internshipDto = _mapper.Map<InternshipDto>(internship);
        internshipDto.Feedbacks = _mapper.Map<List<FeedbackResponseDto>>(internshipFeedbacks);
            
        return internshipDto;
    }
}