using AutoMapper;
using backend.Data;
using backend.Service.Contracts.Student;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace backend.Business.Student.UpdateStudentUseCase;

public class UpdateStudentUseCase : IRequestHandler<UpdateStudentCommand, StudentDto>
{
    private readonly ILogger<UpdateStudentUseCase> _logger;
    private readonly AppDbContext _dbContext;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the UpdateStudentUseCase class.
    /// </summary>
    /// <param name="dbContext">The application database context.</param>
    /// <param name="logger">The logger instance for logging operations.</param>
    /// <param name="mapper">The mapper instance for automapper operations.</param>
    public UpdateStudentUseCase(AppDbContext dbContext, ILogger<UpdateStudentUseCase> logger, IMapper mapper)
    {
        _mapper = mapper;
        _logger = logger;
        _dbContext = dbContext;
    }
    
    public async Task<StudentDto> Handle(UpdateStudentCommand request, CancellationToken cancellationToken)
    {
        var studentDto = request.Dto;
        
        var student = await GetUser(studentDto.Id);
        
        student.Name = studentDto.Name;
        student.CF = studentDto.Cf;
        // student.Skills = studentDto.Skills;
        // student.Interests = studentDto.Interests;

        _dbContext.Students.Update(student);
        await _dbContext.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation("Successfully updated student with ID {Id}.", student.Id);

        return _mapper.Map<StudentDto>(student);
    }
    
    /// <summary>
    /// Retrieves the user associated with the given user ID.
    /// </summary>
    /// <param name="studentId">The user ID to look up.</param>
    /// <returns>The <see cref="Student"/> object associated with the student ID.</returns>
    /// <exception cref="KeyNotFoundException">Thrown if the student cannot be found.</exception>
    private async Task<backend.Data.Entities.Student> GetUser(int studentId)
    {
        var student =  await _dbContext.Students
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == studentId);
        
        if (student == null)
        {
            throw new KeyNotFoundException($"Student with ID {studentId} not found.");
        }

        return student;
    }
}