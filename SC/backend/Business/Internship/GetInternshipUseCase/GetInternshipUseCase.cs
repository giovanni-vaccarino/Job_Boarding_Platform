using AutoMapper;
using backend.Data;
using backend.Service.Contracts.Internship;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace backend.Business.Internship.GetInternshipUseCase;

/// <summary>
/// Handles the retrieval of all internships.
/// </summary>
public class GetInternshipUseCase: IRequestHandler<GetInternshipQuery, List<InternshipDto>>
{
    private readonly AppDbContext _dbContext;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetInternshipUseCase"/> class.
    /// </summary>
    /// <param name="dbContext">The application database context.</param>
    /// <param name="mapper">The AutoMapper instance for object mapping.</param>
    public GetInternshipUseCase(AppDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }
    
    /// <summary>
    /// Handles the query to retrieve all internships.
    /// </summary>
    /// <param name="request">The query object. Currently, no filters are applied in this implementation.</param>
    /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
    /// <returns>A list of <see cref="InternshipDto"/> objects containing the details of all internships.</returns>
    public async Task<List<InternshipDto>> Handle(GetInternshipQuery request, CancellationToken cancellationToken)
    { 
        var internships = await _dbContext.Internships
            .ToListAsync(cancellationToken);

        Console.WriteLine(internships);
        return _mapper.Map<List<InternshipDto>>(internships);
    }
}