using backend.Business.Company.UpdateCompanyProfileUseCase;
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
            typeof(UpdateCompanyProfileUseCase), _dbContext, _services.LoggerMock.Object, _services.Mapper)!;
    }
    
    [Fact(DisplayName = "Successfully update company profile")]
    public async Task Should_Update_Successfully()
    {
        var existingCompany = new backend.Data.Entities.Company
        {
            Name = "Test Company",
            VatNumber = "12345678910",
            UserId = 1,
            Website = "www.testcompany.com",
        };
        _dbContext.Companies.Add(existingCompany);
        await _dbContext.SaveChangesAsync();
        
        var updatedName = "Updated Company";
        var updatedVat = "12345678910";

        var updateDto = new UpdateCompanyProfileDto()
        {
            Name = updatedName,
            Vat = updatedVat,
        };
        var command = new UpdateCompanyProfileCommand(existingCompany.Id, updateDto);
        
        var result = await _updateCompanyUseCase.Handle(command, CancellationToken.None);
        
        Assert.NotNull(result);
        Assert.Equal(updatedName, result.Name);
        Assert.Equal(updatedVat, result.VatNumber);
    }
    
    [Fact(DisplayName = "Successfully Throw An Exception If Company Not Found")]
    public async Task Should_Throw_If_Company_Not_Found()
    {
        var updateDto = new UpdateCompanyProfileDto()
        {
            Name = "Updated Company",
            Vat = "12345678910", 
        };
        
        var nonExistingCompanyId = 1;
        var command = new UpdateCompanyProfileCommand(nonExistingCompanyId, updateDto);
        
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _updateCompanyUseCase.Handle(command, CancellationToken.None));
    }
}