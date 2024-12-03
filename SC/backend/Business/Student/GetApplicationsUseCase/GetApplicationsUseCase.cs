using AutoMapper;
using backend.Data;
using backend.Service.Contracts.Internship;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace backend.Business.Student.GetApplicationsUseCase;

/// <summary>
/// Handles the retrieval of internship applications for a specific student.
/// </summary>
public class GetApplicationsUseCase : IRequestHandler<GetApplicationsQuery, List<ApplicationDto>>
{
    private readonly AppDbContext _dbContext;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetApplicationsUseCase"/> class.
    /// </summary>
    /// <param name="dbContext">The application database context.</param>
    /// <param name="mapper">The AutoMapper instance for object mapping.</param>
    public GetApplicationsUseCase(AppDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }
    
    /// <summary>
    /// Handles the query to retrieve internship applications associated with a specific student.
    /// </summary>
    /// <param name="request">The query containing the student ID.</param>
    /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
    /// <returns>A list of <see cref="ApplicationDto"/> objects containing the details of the applications.</returns>
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