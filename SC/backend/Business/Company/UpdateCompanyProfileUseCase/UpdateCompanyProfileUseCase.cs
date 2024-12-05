using AutoMapper;
using backend.Data;
using backend.Service.Contracts.Company;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace backend.Business.Company.UpdateCompanyProfileUseCase;

/// <summary>
/// Handles the update operation for a company's profile.
/// </summary>
public class UpdateCompanyProfileUseCase : IRequestHandler<UpdateCompanyProfileCommand, CompanyDto>
{
    private readonly ILogger<UpdateCompanyProfileUseCase> _logger;
    private readonly AppDbContext _dbContext;
    private readonly IMapper _mapper;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateCompanyProfileUseCase"/> class.
    /// </summary>
    /// <param name="dbContext">The application database context.</param>
    /// <param name="logger">The logger instance for logging operations.</param>
    /// <param name="mapper">The AutoMapper instance for object mapping.</param>
    public UpdateCompanyProfileUseCase(AppDbContext dbContext, ILogger<UpdateCompanyProfileUseCase> logger, IMapper mapper)
    {
        _logger = logger;
        _dbContext = dbContext;
        _mapper = mapper;
    }
    
    /// <summary>
    /// Handles the command to update a company's profile.
    /// This method retrieves the company by ID, updates its properties, 
    /// and saves the changes to the database.
    /// </summary>
    /// <param name="request">The command containing the updated company details and ID.</param>
    /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
    /// <returns>The updated company as a <see cref="CompanyDto"/>.</returns>
    /// <exception cref="KeyNotFoundException">Thrown if the company with the specified ID does not exist.</exception>
    public async Task<CompanyDto> Handle(UpdateCompanyProfileCommand request, CancellationToken cancellationToken)
    {
        var updateCompanyDto = request.Dto;

        var company = await _dbContext.Companies.FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken) 
                      ?? throw new KeyNotFoundException("Company not found.");

        company.Name = updateCompanyDto.Name;
        company.VatNumber = updateCompanyDto.Vat;
        company.Website = updateCompanyDto.Website;
        company.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Company profile updated successfully for CompanyId {CompanyId}", request.Id);

        return _mapper.Map<Data.Entities.Company, CompanyDto>(company);
    }
}