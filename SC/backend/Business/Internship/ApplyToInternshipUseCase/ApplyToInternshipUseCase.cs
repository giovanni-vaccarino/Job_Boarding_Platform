using AutoMapper;
using backend.Data;
using backend.Data.Entities;
using backend.Service.Contracts.Internship;
using backend.Shared.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace backend.Business.Internship.ApplyToInternshipUseCase;

public class ApplyToInternshipUseCase : IRequestHandler<ApplyToInternshipCommand, ApplicationDto>
{
    private readonly AppDbContext _dbContext;
    private readonly IMapper _mapper;

    public ApplyToInternshipUseCase(AppDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }
    
    public async Task<ApplicationDto> Handle(ApplyToInternshipCommand request, CancellationToken cancellationToken)
    {
        var studentId = request.StudentId;
        var internshipId = request.InternshipId;
        
        await ValidateApplication(studentId, internshipId, cancellationToken);

        var application = new Application
        {
            ApplicationStatus = ApplicationStatus.InProgress,
            StudentId = studentId,
            InternshipId = internshipId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _dbContext.Applications.AddAsync(application, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        
        return _mapper.Map<ApplicationDto>(application);;
    }

    private async Task ValidateApplication(int studentId, int internshipId, CancellationToken cancellationToken)
    {
        var student = await _dbContext.Students.FirstOrDefaultAsync(s => s.Id == studentId, cancellationToken);
        if (student == null)
        {
            throw new KeyNotFoundException($"Student with ID {studentId} not found.");
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