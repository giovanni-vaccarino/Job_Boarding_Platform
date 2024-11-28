using backend.Business.Auth.LoginUseCase;
using backend.Data;
using backend.Service.Contracts.Auth;
using backend.Shared.Security;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace backend.Business.Company.UpdateCompanyProfile;

public class UpdateCompanyProfileUseCase : IRequestHandler<UpdateCompanyProfileCommand, string>
{
    private readonly ILogger<UpdateCompanyProfileUseCase> _logger;
    private readonly AppDbContext _dbContext;
    
    public UpdateCompanyProfileUseCase(AppDbContext dbContext, ILogger<UpdateCompanyProfileUseCase> logger)
    {
        _logger = logger;
        _dbContext = dbContext;
    }
    
    public async Task<string> Handle(UpdateCompanyProfileCommand request, CancellationToken cancellationToken)
    {
        var updateInput = request.Dto;

        // Retrieve the existing company record
        var company = await _dbContext.Companies.FirstOrDefaultAsync(c => c.Id == updateInput.Id, cancellationToken);
     
        if (company == null)
        {
            throw new KeyNotFoundException("Company not found.");
        }

        // Update the company properties with the values from the Dto
        company.Name = updateInput.CompanyName;
        company.VatNumber = updateInput.VAT;
        //TODO add website and linkedin to the company entity
        company.UpdatedAt = DateTime.UtcNow;

        // Save the changes to the database
        await _dbContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Company profile updated successfully for CompanyId {CompanyId}", updateInput.Id);

        return "Company profile updated successfully.";
    }
}