using AutoMapper;
using backend.Data;
using backend.Data.Entities;
using backend.Service.Contracts.Internship;
using backend.Shared.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace backend.Business.Internship.ApplyToInternshipUseCase;

/// <summary>
/// Handles the process of applying to an internship.
/// </summary>
public class ApplyToInternshipUseCase : IRequestHandler<ApplyToInternshipCommand, ApplicationDto>
{
    private readonly AppDbContext _dbContext;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="ApplyToInternshipUseCase"/> class.
    /// </summary>
    /// <param name="dbContext">The application database context.</param>
    /// <param name="mapper">The AutoMapper instance for object mapping.</param>
    public ApplyToInternshipUseCase(AppDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }
    
    /// <summary>
    /// Handles the command to apply a student to an internship.
    /// </summary>
    /// <param name="request">The command containing the student ID and internship ID.</param>
    /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
    /// <returns>An <see cref="ApplicationDto"/> object containing the details of the submitted application.</returns>
    /// <exception cref="KeyNotFoundException">Thrown if the internship is not found.</exception>
    /// <exception cref="InvalidOperationException">Thrown if the application deadline has passed or the student has already applied.</exception>
    public async Task<ApplicationDto> Handle(ApplyToInternshipCommand request, CancellationToken cancellationToken)
    {
        var studentId = request.StudentId;
        var internshipId = request.InternshipId;
        
        await ValidateApplication(studentId, internshipId, cancellationToken);

        var application = new Application
        {
            ApplicationStatus = ApplicationStatus.Screening,
            StudentId = studentId,
            InternshipId = internshipId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _dbContext.Applications.AddAsync(application, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        
        return _mapper.Map<ApplicationDto>(application);
    }
    
    /// <summary>
    /// Validates the application details to ensure the student and internship exist,
    /// the application deadline has not passed, the student has not already applied,
    /// and the student's profile is complete (name, CF, and CV are required).
    /// </summary>
    /// <param name="studentId">The ID of the student applying.</param>
    /// <param name="internshipId">The ID of the internship to apply for.</param>
    /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
    /// <exception cref="KeyNotFoundException">
    /// Thrown if the internship is not found.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown if the application deadline has passed, the student has already applied, 
    /// or the student's profile is incomplete (missing name, CF, or CV).
    /// </exception>
    private async Task ValidateApplication(int studentId, int internshipId, CancellationToken cancellationToken)
    {
        var student = await _dbContext.Students.FirstOrDefaultAsync(s => s.Id == studentId, cancellationToken) ?? throw new KeyNotFoundException($"Student with ID {studentId} not found.");
        
        if (string.IsNullOrWhiteSpace(student.Name) || string.IsNullOrWhiteSpace(student.Cf) || string.IsNullOrWhiteSpace(student.CvPath))
        {
            throw new InvalidOperationException("The student must have a name, a valid CF, and an uploaded CV to apply for an internship.");
        }

        var internship = await _dbContext.Internships.FirstOrDefaultAsync(i => i.Id == internshipId, cancellationToken);
        if (internship == null)
        {
            throw new KeyNotFoundException($"Internship with ID {internshipId} not found.");
        }

        if (internship.ApplicationDeadline < DateOnly.FromDateTime(DateTime.UtcNow))
        {
            throw new InvalidOperationException("The application deadline for this internship has passed.");
        }

        var alreadyApplied = await _dbContext.Applications
            .AnyAsync(a => a.StudentId == studentId && a.InternshipId == internshipId, cancellationToken);
        if (alreadyApplied)
        {
            throw new InvalidOperationException("The student has already applied to this internship.");
        }
    }
}