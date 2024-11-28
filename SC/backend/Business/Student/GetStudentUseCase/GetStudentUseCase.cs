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
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the GetStudentUseCase class.
    /// </summary>
    /// <param name="dbContext">The application database context.</param>
    /// <param name="logger">The logger instance for logging operations.</param>
    /// <param name="mapper">The mapper instance for automapper operations.</param>
    public GetStudentUseCase(AppDbContext dbContext, ILogger<GetStudentUseCase> logger, IMapper mapper)
    {
        _mapper = mapper;
        _logger = logger;
        _dbContext = dbContext;
    }
    
    public async Task<StudentDto> Handle(GetStudentQuery request, CancellationToken cancellationToken)
    {
        var student = await GetUser(request.Id);

        _logger.LogInformation("Successfully retrieved student with ID {Id}.", request.Id);

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