using backend.Business.Company.GetInternshipsUseCase;
using backend.Data;
using backend.Data.Entities;
using backend.Service.Contracts.Internship;
using backend.Shared.Enums;
using Microsoft.EntityFrameworkCore;

namespace UnitTests.UseCases.Company;

/// <summary>
/// Unit tests for the <see cref="GetInternshipsUseCase"/>.
/// </summary>
public class GetInternshipsUseCaseTests
{
    private readonly IsolatedUseCaseTestServices<GetInternshipsUseCase> _services;
    private readonly AppDbContext _dbContext;
    private readonly GetInternshipsUseCase _getInternshipsUseCase;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetInternshipsUseCaseTests"/> class.
    /// Sets up the isolated services, database context, and use case instance for testing.
    /// </summary>
    public GetInternshipsUseCaseTests()
    {
        _services = new IsolatedUseCaseTestServices<GetInternshipsUseCase>("GetInternshipsUseCaseTests");
        _dbContext = _services.DbContext;
        _getInternshipsUseCase = (GetInternshipsUseCase)Activator.CreateInstance(
            typeof(GetInternshipsUseCase), _dbContext, _services.Mapper)!;
    }

    /// <summary>
    /// Tests that all internships for a specific company are retrieved successfully.
    /// </summary>
    [Fact(DisplayName = "Retrieve all internships for a company")]
    public async Task Should_Retrieve_All_Internships_For_Company()
    {
        var company = new backend.Data.Entities.Company
        {
            Name = "Test Company",
            VatNumber = "123456789",
            UserId = 1,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        
        var internship1 = new backend.Data.Entities.Internship
        {
            Title = "Software Developer Intern",
            CompanyId = company.Id,
            Company = company,
            Description = "Develop software solutions.",
            ApplicationDeadline = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(30)),
            Location = "Remote",
            Duration = DurationType.ThreeToSixMonths,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var internship2 = new backend.Data.Entities.Internship
        {
            Title = "Data Analyst Intern",
            CompanyId = company.Id,
            Company = company,
            Description = "Analyze and interpret data.",
            ApplicationDeadline = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(45)),
            Location = "Onsite",
            Duration = DurationType.TwoToThreeMonths,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _dbContext.Internships.AddRange(internship1, internship2);
        await _dbContext.SaveChangesAsync();

        var query = new GetInternshipsQuery(company.Id);

        var result = await _getInternshipsUseCase.Handle(query, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Contains(result, i => i.Title == internship1.Title);
        Assert.Contains(result, i => i.Title == internship2.Title);
    }

    /// <summary>
    /// Tests that a specific internship for a company is retrieved successfully.
    /// </summary>
    [Fact(DisplayName = "Retrieve a specific internship for a company")]
    public async Task Should_Retrieve_Specific_Internship_For_Company()
    {
        var company = new backend.Data.Entities.Company
        {
            Name = "Test Company",
            VatNumber = "123456789",
            UserId = 1,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _dbContext.Companies.Add(company);
        _dbContext.SaveChanges();
        
        var internship = new backend.Data.Entities.Internship
        {
            Title = "Software Developer Intern",
            CompanyId = company.Id,
            Company = company,
            Description = "Develop software solutions.",
            ApplicationDeadline = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(30)),
            Location = "Remote",
            Duration = DurationType.ThreeToSixMonths,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _dbContext.Internships.Add(internship);
        await _dbContext.SaveChangesAsync();

        var query = new GetInternshipsQuery(company.Id, internship.Id);

        var result = await _getInternshipsUseCase.Handle(query, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal(internship.Title, result.First().Title);
    }

    /// <summary>
    /// Tests that an empty list is returned when no internships exist for a company.
    /// </summary>
    [Fact(DisplayName = "Return empty list when no internships for a company")]
    public async Task Should_Return_Empty_List_When_No_Internships_For_Company()
    {
        var companyId = 1;
        var query = new GetInternshipsQuery(companyId);

        var result = await _getInternshipsUseCase.Handle(query, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Empty(result);
    }
}
