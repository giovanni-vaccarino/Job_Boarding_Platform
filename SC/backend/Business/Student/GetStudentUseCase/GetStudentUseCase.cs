using AutoMapper;
using backend.Data;
using backend.Data.Entities;
using backend.Service.Contracts.Student;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace backend.Business.Student.GetStudentUseCase;

public class GetStudentUseCase : IRequestHandler<GetStudentQuery, StudentDto>
{
    private readonly ILogger<GetStudentUseCase> _logger;
    private readonly AppDbContext _dbContext;

    /// <summary>
    /// Initializes a new instance of the GetStudentUseCase class.
    /// </summary>
    /// <param name="dbContext">The application database context.</param>
    /// <param name="logger">The logger instance for logging operations.</param>
    /// <param name="mapper">The mapper instance for automapper operations.</param>
    public GetStudentUseCase(AppDbContext dbContext, ILogger<GetStudentUseCase> logger)
    {
        _logger = logger;
        _dbContext = dbContext;
    }
    
    public async Task<StudentDto> Handle(GetStudentQuery request, CancellationToken cancellationToken)
    {
        var student = await GetUser(request.Id);

        _logger.LogInformation("Successfully retrieved student with ID {Id}.", request.Id);

        return student;
    }
    
    /// <summary>
    /// Retrieves the user associated with the given user ID.
    /// </summary>
    /// <param name="studentId">The user ID to look up.</param>
    /// <returns>The <see cref="Student"/> object associated with the student ID.</returns>
    /// <exception cref="KeyNotFoundException">Thrown if the student cannot be found.</exception>
    private async Task<StudentDto> GetUser(int studentId)
    {
        var studentData = await _dbContext.Students
            .AsNoTracking()
            .Where(s => s.Id == studentId)
            .Select(s => new
            {
                Student = s,
                Email = _dbContext.Users
                    .Where(u => u.Id == s.UserId)
                    .Select(u => u.Email)
                    .FirstOrDefault()
            })
            .FirstOrDefaultAsync();

        if (studentData == null)
        {
            throw new KeyNotFoundException($"Student with ID {studentId} not found.");
        }
        
        var studentDto = new StudentDto
        {
            Id = studentData.Student.Id,
            Name = studentData.Student.Name,
            Cf = studentData.Student.Cf,
            CvPath = studentData.Student.CvPath,
            Email = studentData.Email,
            Skills = studentData.Student.Skills,
            Interests = studentData.Student.Interests
        };

        return studentDto;
    }

}