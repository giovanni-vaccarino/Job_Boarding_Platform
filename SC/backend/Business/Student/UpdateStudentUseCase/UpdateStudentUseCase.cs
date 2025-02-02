using AutoMapper;
using backend.Data;
using backend.Service.Contracts.Student;
using backend.Shared.MatchingBackgroundService;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace backend.Business.Student.UpdateStudentUseCase;

/// <summary>
/// Handles the update operation for a student.
/// </summary>
public class UpdateStudentUseCase : IRequestHandler<UpdateStudentCommand, StudentDto>
{
    private readonly ILogger<UpdateStudentUseCase> _logger;
    private readonly AppDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IJobQueue _jobQueue;
    private readonly IStudentMatchingTaskFactory _studentMatchingTaskFactory;

    /// <summary>
    /// Initializes a new instance of the UpdateStudentUseCase class.
    /// </summary>
    /// <param name="dbContext">The application database context.</param>
    /// <param name="logger">The logger instance for logging operations.</param>
    /// <param name="mapper">The mapper instance for automapper operations.</param>
    /// <param name="jobQueue">The jobQueue instance for background job operations.</param>
    /// <param name="studentMatchingTaskFactory">The _studentMatchingTaskFactory instance for creating a MatchingTask.</param>
    public UpdateStudentUseCase(AppDbContext dbContext, ILogger<UpdateStudentUseCase> logger, IMapper mapper, IJobQueue jobQueue, IStudentMatchingTaskFactory studentMatchingTaskFactory)
    {
        _mapper = mapper;
        _logger = logger;
        _dbContext = dbContext;
        _jobQueue = jobQueue;
        _studentMatchingTaskFactory = studentMatchingTaskFactory;
    }
    
    /// <summary>
    /// Handles the command to update a student entity in the database.
    /// This method retrieves the student by ID, updates its properties, 
    /// and saves the changes to the database.
    /// </summary>
    /// <param name="request">The command containing the updated student details.</param>
    /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
    /// <returns>The updated student as a <see cref="StudentDto"/>.</returns>
    /// <exception cref="KeyNotFoundException">Thrown if the student with the specified ID does not exist.</exception>
    public async Task<StudentDto> Handle(UpdateStudentCommand request, CancellationToken cancellationToken)
    {
        var updateStudentDto = request.Dto;
        
        var student = await GetStudent(request.StudentId);
        
        student.Name = updateStudentDto.Name;
        student.Cf = updateStudentDto.Cf;
        student.Skills = updateStudentDto.Skills;
        student.Interests = updateStudentDto.Interests;
        student.UpdatedAt = DateTime.UtcNow;

        _dbContext.Students.Update(student);
        await _dbContext.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation("Successfully updated student with ID {Id}.", student.Id);

        if (student.Skills.Count > 0 || student.Interests.Count > 0)
        {
            var matchingTask = _studentMatchingTaskFactory.Create(student.Id);
            _jobQueue.EnqueueJob(matchingTask);
        }

        return _mapper.Map<StudentDto>(student);
    }
    
    /// <summary>
    /// Retrieves the user associated with the given user ID.
    /// </summary>
    /// <param name="studentId">The user ID to look up.</param>
    /// <returns>The <see cref="Student"/> object associated with the student ID.</returns>
    /// <exception cref="KeyNotFoundException">Thrown if the student cannot be found.</exception>
    private async Task<backend.Data.Entities.Student> GetStudent(int studentId)
    {
        var student =  await _dbContext.Students
            .FirstOrDefaultAsync(s => s.Id == studentId);
        
        if (student == null)
        {
            throw new KeyNotFoundException($"Student with ID {studentId} not found.");
        }

        return student;
    }
}