using backend.Business.Student.LoadCvUseCase;
using backend.Data;
using backend.Shared.StorageService;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace backend.Business.Assets.GetAssetUseCase;

/// <summary>
/// Handles the retrieval of a student's CV as a downloadable file from storage.
/// </summary>
/// <remarks>
/// This use case is responsible for validating the existence of a student in the database,
/// generating the appropriate file key for the CV, and returning the file as a downloadable stream.
/// </remarks>
public class GetAssetUseCase : IRequestHandler<GetAssetQuery, FileStreamResult>
{
    private readonly AppDbContext _dbContext;
    private readonly IS3Manager _s3Manager;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetAssetUseCase"/> class.
    /// </summary>
    /// <param name="dbContext">The database context used for validating student existence.</param>
    /// <param name="s3Manager">The S3 storage manager used to retrieve the CV file.</param>
    public GetAssetUseCase(AppDbContext dbContext, IS3Manager s3Manager)
    {
        _dbContext = dbContext;
        _s3Manager = s3Manager;
    }

    /// <summary>
    /// Handles the request to retrieve a student's CV.
    /// </summary>
    /// <param name="request">The request containing the student ID and any additional query parameters.</param>
    /// <param name="cancellationToken">A token to monitor for request cancellation.</param>
    /// <returns>A <see cref="FileStreamResult"/> containing the CV file as a PDF stream.</returns>
    /// <exception cref="Exception">Thrown if the specified student does not exist in the database.</exception>
    /// <remarks>
    /// The method:
    /// - Verifies the student exists in the database.
    /// - Constructs the unique file key for the CV using the student's ID and filename.
    /// - Downloads the file from S3 storage using the <see cref="IS3Manager"/>.
    /// - Returns the file as a <see cref="FileStreamResult"/> for the client to download.
    /// </remarks>
    public async Task<FileStreamResult> Handle(GetAssetQuery request, CancellationToken cancellationToken)
    {
        var student = await _dbContext.Students.FindAsync(request.StudentId);
        if (student == null)
        {
            throw new Exception("Student not found.");
        }

        var fileKey = LoadCvUseCase.GetUniqueFileKey(student.Id.ToString(), "cv.pdf");
        var fileStream = await _s3Manager.DownloadFileAsync(fileKey);

        var memoryStream = new MemoryStream();
        await fileStream.CopyToAsync(memoryStream);
        memoryStream.Position = 0;

        return new FileStreamResult(memoryStream, "application/pdf")
        {
            FileDownloadName = "cv.pdf"
        };
    }
}