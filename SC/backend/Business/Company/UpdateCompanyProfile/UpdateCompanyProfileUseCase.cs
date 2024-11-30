using AutoMapper;
using backend.Data;
using backend.Service.Contracts.Company;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace backend.Business.Company.UpdateCompanyProfile;

public class UpdateCompanyProfileUseCase : IRequestHandler<UpdateCompanyProfileCommand, CompanyDto>
{
    private readonly ILogger<UpdateCompanyProfileUseCase> _logger;
    private readonly AppDbContext _dbContext;
    private readonly IMapper _mapper;
    
    public UpdateCompanyProfileUseCase(AppDbContext dbContext, ILogger<UpdateCompanyProfileUseCase> logger, IMapper mapper)
    {
        _logger = logger;
        _dbContext = dbContext;
        _mapper = mapper;
    }
    
    public async Task<CompanyDto> Handle(UpdateCompanyProfileCommand request, CancellationToken cancellationToken)
    {
        var updateCompanyDto = request.Dto;

        var company = await _dbContext.Companies.FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken) 
                      ?? throw new KeyNotFoundException("Company not found.");

        company.Name = updateCompanyDto.Name;
        company.VatNumber = updateCompanyDto.Vat;
        company.UpdatedAt = DateTime.UtcNow;
        //TODO add website to the company entity

        await _dbContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Company profile updated successfully for CompanyId {CompanyId}", request.Id);

        return _mapper.Map<Data.Entities.Company, CompanyDto>(company);
    }
}