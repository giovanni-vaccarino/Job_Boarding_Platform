using backend.Data;
using backend.Shared.StorageService;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace backend.Business.Student.LoadCvUseCase;

public class LoadCvUseCase : IRequestHandler<LoadCvCommand, string>
{
    private readonly S3Manager _s3Manager;
    private readonly AppDbContext _dbContext;

    public LoadCvUseCase(AppDbContext dbContext, S3Manager s3Manager)
    {
        _dbContext = dbContext;
        _s3Manager = s3Manager;
    }
    
    public async Task<string> Handle(LoadCvCommand request, CancellationToken cancellationToken)
    {
        var input = request.Dto;
        var studentId = request.StudentId.ToString();
        var student = await _dbContext.Students.FirstOrDefaultAsync(s => s.Id == request.StudentId, cancellationToken) ?? 
                      throw new InvalidOperationException("Student not found");
        
        if (input.File.Length == 0)
            throw new ArgumentException("Invalid file");

        var keyName = GetUniqueFileKey(studentId, input.File.FileName);

        using (var fileStream = input.File.OpenReadStream())
        {
            await _s3Manager.UploadFileAsync(fileStream, keyName);
        }

        student.CvPath = keyName;
        _dbContext.Students.Update(student);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return keyName;
    }
    
    private string GetUniqueFileKey(string studentId, string originalFileName)
    {
        var fileExtension = Path.GetExtension(originalFileName);
        var uniqueName = $"students/{studentId}/cv{fileExtension}";
        return uniqueName;
    }
}