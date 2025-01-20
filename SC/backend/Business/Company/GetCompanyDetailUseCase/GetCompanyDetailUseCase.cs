using backend.Data;
using AutoMapper;
using backend.Service.Contracts.Company;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace backend.Business.Company.GetCompanyDetailUseCase;

/// <summary>
/// Handles the retrieval of information about a company.
/// </summary>
public class GetCompanyDetailUseCase : IRequestHandler<GetCompanyDetailQuery, CompanyDto>
{
    private readonly AppDbContext _dbContext;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetCompanyDetailUseCase"/> class.
    /// </summary>
    /// <param name="dbContext">The application database context.</param>
    /// <param name="mapper">The AutoMapper instance for object mapping.</param>
    public GetCompanyDetailUseCase(AppDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the query to retrieve detailed information about a company.
    /// </summary>
    /// <param name="request">The query containing the company ID.</param>
    /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
    /// <returns>A <see cref="CompanyDto"/> object containing the company details.</returns>
    /// <exception cref="KeyNotFoundException">Thrown if the company with the specified ID does not exist.</exception>
    public async Task<CompanyDto> Handle(GetCompanyDetailQuery request, CancellationToken cancellationToken)
    {
        var company = await _dbContext.Companies
                          .AsNoTracking()
                          .Where(c => c.Id == request.Id)
                          .Select(c => new
                          {
                              Company = c,
                              Email = _dbContext.Users
                                  .Where(u => u.Id == c.Id)
                                  .Select(u => u.Email)
                                  .FirstOrDefault()
                          })
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new KeyNotFoundException("Company not found.");


        var companyToReturn = new CompanyDto
        {
            Id = company.Company.Id,
            Name = company.Company.Name,
            Email = company.Company.User.Email,
            VatNumber = company.Company.VatNumber,
            Website = company.Company.Website,
        };
        
        return _mapper.Map<CompanyDto>(company);
    }
}