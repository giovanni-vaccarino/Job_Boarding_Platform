using backend.Business.Company.UpdateInternshipUseCase;
using backend.Data;
using backend.Data.Entities;
using backend.Service.Contracts.Company;
using backend.Shared.Enums;
using Microsoft.EntityFrameworkCore;

namespace UnitTests.UseCases.Company;

/// <summary>
/// Unit tests for the <see cref="UpdateInternshipUseCase"/>.
/// </summary>
public class UpdateInternshipUseCaseTests
{
    private readonly IsolatedUseCaseTestServices<UpdateInternshipUseCase> _services;
    private readonly AppDbContext _dbContext;
    private readonly UpdateInternshipUseCase _updateInternshipUseCase;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateInternshipUseCaseTests"/> class.
    /// Sets up the isolated services, database context, and use case instance for testing.
    /// </summary>
    public UpdateInternshipUseCaseTests()
    {
        _services = new IsolatedUseCaseTestServices<UpdateInternshipUseCase>("UpdateInternshipUseCaseTests");
        _dbContext = _services.DbContext;
        _updateInternshipUseCase = (UpdateInternshipUseCase)Activator.CreateInstance(
            typeof(UpdateInternshipUseCase), _dbContext, _services.Mapper)!;
    }

    /// <summary>
    /// Tests that an internship is successfully updated.
    /// </summary>
    [Fact(DisplayName = "Successfully update an internship")]
    public async Task Should_Update_Internship_Successfully()
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
            Description = "Old description",
            ApplicationDeadline = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(30)),
            Location = "Remote",
            Duration = DurationType.TwoToThreeMonths,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _dbContext.Internships.Add(internship);
        await _dbContext.SaveChangesAsync();

        var updateCommand = new UpdateInternshipCommand(company.Id, internship.Id,
            new UpdateInternshipDto
            {
                JobDetails = new AddJobDetailsDto
                {
                    Title = "Updated Developer Intern",
                    Description = "New description",
                    ApplicationDeadline = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(45)),
                    Location = "Onsite",
                    Duration = DurationType.ThreeToSixMonths,
                    JobCategory = JobCategory.Technology,
                    JobType = JobType.FullTime,
                    Requirements = new List<string> { "C#", "SQL", "Problem-Solving" }
                }
            });
       
        var result = await _updateInternshipUseCase.Handle(updateCommand, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(updateCommand.Dto.JobDetails.Title, result.Title);
        Assert.Equal(updateCommand.Dto.JobDetails.Description, result.Description);
        Assert.Equal(updateCommand.Dto.JobDetails.Location, result.Location);
        Assert.Equal(updateCommand.Dto.JobDetails.Duration, result.Duration);

        var updatedInternship = await _dbContext.Internships.FirstOrDefaultAsync(i => i.Id == internship.Id);
        Assert.NotNull(updatedInternship);
        Assert.Equal(updateCommand.Dto.JobDetails.Title, updatedInternship.Title);
        Assert.Equal(updateCommand.Dto.JobDetails.Description, updatedInternship.Description);
    }

    /// <summary>
    /// Tests that a KeyNotFoundException is thrown when trying to update a non-existing internship.
    /// </summary>
    [Fact(DisplayName = "Throw exception when internship does not exist")]
    public async Task Should_Throw_Exception_When_Internship_Does_Not_Exist()
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

        var updateCommand = new UpdateInternshipCommand(company.Id, 5,
            new UpdateInternshipDto
            {
                JobDetails = new AddJobDetailsDto
                {
                    Title = "Updated Developer Intern",
                    Description = "New description",
                    ApplicationDeadline = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(45)),
                    Location = "Onsite",
                    Duration = DurationType.ThreeToSixMonths,
                    JobCategory = JobCategory.Technology,
                    JobType = JobType.FullTime,
                    Requirements = new List<string> { "C#", "SQL", "Problem-Solving" }
                }
            });

        var act = async () => await _updateInternshipUseCase.Handle(updateCommand, CancellationToken.None);

        var exception = await Assert.ThrowsAsync<KeyNotFoundException>(act);
        Assert.Equal($"Internship with ID {updateCommand.InternshipId} for Company ID {updateCommand.Id} not found.", exception.Message);
    }
}
