using backend.Business.Internship.UpdateStatusApplicationUseCase;
using backend.Data;
using backend.Data.Entities;
using backend.Service.Contracts.Internship;
using backend.Shared.Enums;
using Microsoft.EntityFrameworkCore;

namespace UnitTests.UseCases.Internship;

/// <summary>
/// Unit tests for the <see cref="UpdateStatusApplicationUseCase"/>.
/// </summary>
public class UpdateStatusApplicationUseCaseTests
{
    private readonly IsolatedUseCaseTestServices<UpdateStatusApplicationUseCase> _services;
    private readonly AppDbContext _dbContext;
    private readonly UpdateStatusApplicationUseCase _updateStatusApplicationUseCase;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateStatusApplicationUseCaseTests"/> class.
    /// Sets up the isolated services, database context, and use case instance for testing.
    /// </summary>
    public UpdateStatusApplicationUseCaseTests()
    {
        _services = new IsolatedUseCaseTestServices<UpdateStatusApplicationUseCase>("UpdateStatusApplicationUseCaseTests");
        _dbContext = _services.DbContext;
        _updateStatusApplicationUseCase = (UpdateStatusApplicationUseCase)Activator.CreateInstance(
            typeof(UpdateStatusApplicationUseCase), _dbContext, _services.Mapper)!;
    }

    /// <summary>
    /// Tests that the application status is successfully updated.
    /// </summary>
    [Fact(DisplayName = "Successfully update application status")]
    public async Task Should_Update_Application_Status_Successfully()
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
            Company = company,
            CompanyId = company.Id,
            Description = "Develop software solutions.",
            ApplicationDeadline = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(30)),
            Location = "Remote",
            Duration = DurationType.ThreeToSixMonths,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        
        _dbContext.Internships.Add(internship);
        await _dbContext.SaveChangesAsync();


        var application = new Application
        {
            ApplicationStatus = ApplicationStatus.InProgress,
            StudentId = 1,
            Internship = internship,
            InternshipId = internship.Id,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _dbContext.Applications.Add(application);
        await _dbContext.SaveChangesAsync();

        var command = new UpdateStatusApplicationCommand(application.Id, 
            new UpdateStatusApplicationDto{ Status = ApplicationStatus.Accepted });
        
        var result = await _updateStatusApplicationUseCase.Handle(command, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(ApplicationStatus.Accepted, result.ApplicationStatus);

        var updatedApplication = await _dbContext.Applications.FirstOrDefaultAsync(app => app.Id == application.Id);
        Assert.NotNull(updatedApplication);
        Assert.Equal(ApplicationStatus.Accepted, updatedApplication.ApplicationStatus);
    }

    /// <summary>
    /// Tests that a KeyNotFoundException is thrown when the application is not found.
    /// </summary>
    [Fact(DisplayName = "Throw exception when application not found")]
    public async Task Should_Throw_Exception_When_Application_Not_Found()
    {
        var nonExistentApplicationId = 1;
        var command = new UpdateStatusApplicationCommand(nonExistentApplicationId, 
            new UpdateStatusApplicationDto{ Status = ApplicationStatus.Accepted });

        var act = async () => await _updateStatusApplicationUseCase.Handle(command, CancellationToken.None);

        var exception = await Assert.ThrowsAsync<KeyNotFoundException>(act);
        Assert.Equal($"Application with ID {nonExistentApplicationId} not found.", exception.Message);
    }
}
