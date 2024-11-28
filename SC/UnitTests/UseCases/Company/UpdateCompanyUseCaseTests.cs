using backend.Business.Company.UpdateCompanyProfile;
using backend.Data;
using backend.Service.Contracts.Company;

namespace UnitTests.UseCases.Company;

public class UpdateCompanyUseCaseTests
{
    private readonly IsolatedUseCaseTestServices<UpdateCompanyProfileUseCase> _services;
    private readonly AppDbContext _dbContext;
    private readonly UpdateCompanyProfileUseCase _updateCompanyUseCase;
    
    public UpdateCompanyUseCaseTests()
    {
        _services = new IsolatedUseCaseTestServices<UpdateCompanyProfileUseCase>("UpdateCompanyProfileUseCaseTests");
        _dbContext = _services.DbContext;
        _updateCompanyUseCase = (UpdateCompanyProfileUseCase)Activator.CreateInstance(
            typeof(UpdateCompanyProfileUseCase), _dbContext, _services.LoggerMock.Object)!;
    }
    
    [Fact(DisplayName = "Successfully update company profile")]
    public async Task Should_Update_Successfully()
    {
        var existingCompany = new backend.Data.Entities.Company()
        {
            Name = "Test Company",
            VatNumber = "12345678910",
            UserId = 1
        };
        _dbContext.Companies.Add(existingCompany);
        await _dbContext.SaveChangesAsync();

        var updateDto = new UpdateCompanyProfileDto()
        {
            CompanyName = "Updated Company",
            VAT = "12345678910", 
            Id = 1
        };
        var command = new UpdateCompanyProfileCommand(updateDto);
        
        var result = await _updateCompanyUseCase.Handle(command, CancellationToken.None);
        
        Assert.NotNull(result);
        Assert.Equal("Company profile updated successfully.", result);
    }
    
    [Fact(DisplayName = "Successfully update company profile")]
    public async Task Should_Throw_If_Company_Not_Found()
    {
        var updateDto = new UpdateCompanyProfileDto()
        {
            CompanyName = "Updated Company",
            VAT = "12345678910", 
            Id = 1
        };
        var command = new UpdateCompanyProfileCommand(updateDto);
        
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _updateCompanyUseCase.Handle(command, CancellationToken.None));
    }
}