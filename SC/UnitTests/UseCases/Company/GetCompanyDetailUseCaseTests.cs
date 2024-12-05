using backend.Business.Company.GetCompanyDetailUseCase;
using backend.Data;

namespace UnitTests.UseCases.Company;

/// <summary>
/// Unit tests for the <see cref="GetCompanyDetailUseCase"/>.
/// </summary>
public class GetCompanyDetailUseCaseTests
{
    private readonly IsolatedUseCaseTestServices<GetCompanyDetailUseCase> _services;
    private readonly AppDbContext _dbContext;
    private readonly GetCompanyDetailUseCase _getCompanyDetailUseCase;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="GetCompanyDetailUseCaseTests"/> class.
    /// Sets up the isolated services, database context, and use case instance for testing.
    /// </summary>
    public GetCompanyDetailUseCaseTests()
    {
        _services = new IsolatedUseCaseTestServices<GetCompanyDetailUseCase>("GetCompanyDetailUseCaseTests");
        _dbContext = _services.DbContext;
        _getCompanyDetailUseCase = (GetCompanyDetailUseCase)Activator.CreateInstance(
            typeof(GetCompanyDetailUseCase), _dbContext, _services.Mapper)!;
    }
    
    /// <summary>
    /// Tests that the <see cref="GetCompanyDetailUseCase"/> successfully retrieves company details 
    /// when a valid company ID is provided.
    /// </summary>
    [Fact(DisplayName = "Successfully update company profile")]
    public async Task Should_Get_Company_Details_Successfully()
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
        
        var command = new GetCompanyDetailQuery(existingCompany.Id);
        
        var result = await _getCompanyDetailUseCase.Handle(command, CancellationToken.None);
        
        Assert.NotNull(result);
        Assert.Equal(existingCompany.Name, result.Name);
        Assert.Equal(existingCompany.VatNumber, result.VatNumber);
    }
    
    /// <summary>
    /// Tests that the <see cref="GetCompanyDetailUseCase"/> throws a <see cref="KeyNotFoundException"/>
    /// when the company with the specified ID does not exist.
    /// </summary>
    [Fact(DisplayName = "Successfully Throw An Exception If Company Not Found")]
    public async Task Should_Throw_If_Company_Not_Found()
    {
        var nonExistingCompanyId = 1;
        var command = new GetCompanyDetailQuery(nonExistingCompanyId);
        
        var act = async () => await _getCompanyDetailUseCase.Handle(command, CancellationToken.None);
        
        var exception = await Assert.ThrowsAsync<KeyNotFoundException>(act);
        
        Assert.Equal("Company not found.", exception.Message);
    }
}