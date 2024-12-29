using AutoMapper;
using backend.Data;
using backend.Service.Contracts.Internship;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace backend.Business.Company.GetInternshipsUseCase;


/// <summary>
/// Handles the retrieval of a list of internships for a company.
/// </summary>
public class GetInternshipsUseCase : IRequestHandler<GetInternshipsQuery, List<InternshipDto>>
{
    private readonly AppDbContext _dbContext;
    private readonly IMapper _mapper;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="GetInternshipsUseCase"/> class.
    /// </summary>
    /// <param name="dbContext">The application database context.</param>
    /// <param name="mapper">The AutoMapper instance for object mapping.</param>
    public GetInternshipsUseCase(AppDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }
    
    /// <summary>
    /// Handles the query to retrieve internships associated with a specific company.
    /// </summary>
    /// <param name="request">The query containing the company ID and optional internship ID.</param>
    /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
    /// <returns>A list of <see cref="InternshipDto"/> objects containing the internship details.</returns>
    public async Task<List<InternshipDto>> Handle(GetInternshipsQuery request, CancellationToken cancellationToken)
    {
        var companyId = request.Id;
        var internshipId = request.InternshipId;
        
        var query = _dbContext.Internships.AsQueryable()
            .Where(i => i.CompanyId == companyId)
            .Select(i => new 
            {
                Internship = i,
                NumberOfApplications = _dbContext.Applications.Count(a => a.InternshipId == i.Id)
            });

        if (internshipId.HasValue)
        {
            query = query.Where(i => i.Internship.Id == internshipId);
        }
        
        var internshipsData = await query.ToListAsync(cancellationToken);

        var internships = internshipsData.Select(data => 
        {
            var internshipDto = _mapper.Map<InternshipDto>(data.Internship);
            internshipDto.NumberOfApplications = data.NumberOfApplications;
            return internshipDto;
        }).ToList();

        return internships;
    }
}
