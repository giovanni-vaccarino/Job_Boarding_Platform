using backend.Data;
using backend.Shared.StorageService;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace backend.Business.Student.LoadCvUseCase;

/// <summary>
/// Handles the loading of a CV for a student, uploading it to the storage and updating the student's record in the database.
/// </summary>
public class LoadCvUseCase : IRequestHandler<LoadCvCommand, string>
{
    private readonly IS3Manager _s3Manager;
    private readonly AppDbContext _dbContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="LoadCvUseCase"/> class.
    /// </summary>
    /// <param name="dbContext">The database context for interacting with student data.</param>
    /// <param name="s3Manager">The S3 manager for handling file uploads.</param>
    public LoadCvUseCase(AppDbContext dbContext, IS3Manager s3Manager)
    {
        _dbContext = dbContext;
        _s3Manager = s3Manager;
    }
    
    /// <summary>
    /// Handles the loading of a CV by uploading the file to S3 and updating the student's CV path in the database.
    /// </summary>
    /// <param name="request">The command containing the student ID and CV file details.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>The unique key name of the uploaded CV in the S3 bucket.</returns>
    /// <exception cref="KeyNotFoundException">Thrown if the student is not found.</exception>
    /// <exception cref="ArgumentException">Thrown if the provided file is invalid.</exception>
    public async Task<string> Handle(LoadCvCommand request, CancellationToken cancellationToken)
    {
        var input = request.Dto;
        var studentId = request.StudentId.ToString();
        var student = await _dbContext.Students.FirstOrDefaultAsync(s => s.Id == request.StudentId, cancellationToken) ?? 
                      throw new KeyNotFoundException("Student not found");
        
        if (input.File.Length == 0)
            throw new ArgumentException("Invalid file");

        var keyName = GetUniqueFileKey(studentId, input.File.FileName);

        using (var fileStream = input.File.OpenReadStream())
        {
            await _s3Manager.UploadFileAsync(fileStream, keyName);
        }

        student.CvPath = keyName;
        student.UpdatedAt = DateTime.UtcNow;
        _dbContext.Students.Update(student);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return keyName;
    }
    
    /// <summary>
    /// Generates a unique key for the CV file based on the student ID and original file name.
    /// </summary>
    /// <param name="studentId">The ID of the student.</param>
    /// <param name="originalFileName">The original file name of the CV.</param>
    /// <returns>A unique key for storing the CV in the S3 bucket.</returns>
    public static string GetUniqueFileKey(string studentId, string originalFileName)
    {
        var fileExtension = Path.GetExtension(originalFileName);
        var uniqueName = $"students/{studentId}/cv{fileExtension}";
        return uniqueName;
    }
}