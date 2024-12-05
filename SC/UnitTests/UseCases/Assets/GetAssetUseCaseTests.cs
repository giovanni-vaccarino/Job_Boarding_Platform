using backend.Business.Assets.GetAssetUseCase;
using backend.Business.Student.LoadCvUseCase;
using backend.Data;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace UnitTests.UseCases.Assets;

/// <summary>
/// Unit tests for the <see cref="GetAssetUseCase"/>.
/// </summary>
public class GetAssetUseCaseTests
{
    private readonly IsolatedUseCaseTestServices<GetAssetUseCase> _services;
    private readonly AppDbContext _dbContext;
    private readonly GetAssetUseCase _getAssetUseCase;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetAssetUseCaseTests"/> class.
    /// Sets up the isolated services, database context, and use case instance for testing.
    /// </summary>
    public GetAssetUseCaseTests()
    {
        _services = new IsolatedUseCaseTestServices<GetAssetUseCase>("GetAssetUseCaseTests");
        _dbContext = _services.DbContext;

        _getAssetUseCase = new GetAssetUseCase(_dbContext, _services.S3ManagerMock.Object);
    }

    /// <summary>
    /// Tests that the file is returned successfully when the student exists and the file is found in storage.
    /// </summary>
    [Fact(DisplayName = "Return FileStreamResult when file is found")]
    public async Task Handle_ShouldReturnFileStreamResult_WhenStudentExistsAndFileIsFound()
    {
        var existingStudent = new backend.Data.Entities.Student
        {
            UserId = 1,
            Name = "Test Student",
            Cf = "AAABBB00H00A000A"
        };
        _dbContext.Students.Add(existingStudent);
        await _dbContext.SaveChangesAsync();

        var mockFileContent = "Sample PDF Content";
        var mockStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(mockFileContent));

        var fileKey = LoadCvUseCase.GetUniqueFileKey(existingStudent.Id.ToString(), "cv.pdf");

        _services.S3ManagerMock
            .Setup(s3 => s3.DownloadFileAsync(fileKey))
            .ReturnsAsync(mockStream);

        var query = new GetAssetQuery(existingStudent.Id);

        var result = await _getAssetUseCase.Handle(query, CancellationToken.None);

        Assert.NotNull(result);
        Assert.IsType<FileStreamResult>(result);
        Assert.Equal("application/pdf", result.ContentType);
        Assert.Equal("cv.pdf", result.FileDownloadName);

        _services.S3ManagerMock.Verify(
            s3 => s3.DownloadFileAsync(fileKey),
            Times.Once
        );
    }

    /// <summary>
    /// Tests that a <see cref="FileNotFoundException"/> is thrown when the file is not found in storage.
    /// </summary>
    [Fact(DisplayName = "Throw exception when file does not exist in storage")]
    public async Task Handle_ShouldThrowException_WhenFileDoesNotExistInStorage()
    {
        var existingStudent = new backend.Data.Entities.Student
        {
            UserId = 1,
            Name = "Test Student",
            Cf = "AAABBB00H00A000A"
        };
        _dbContext.Students.Add(existingStudent);
        await _dbContext.SaveChangesAsync();

        var fileKey = LoadCvUseCase.GetUniqueFileKey(existingStudent.Id.ToString(), "cv.pdf");

        _services.S3ManagerMock
            .Setup(s3 => s3.DownloadFileAsync(fileKey))
            .ThrowsAsync(new FileNotFoundException());

        var query = new GetAssetQuery(existingStudent.Id);

        await Assert.ThrowsAsync<FileNotFoundException>(() => _getAssetUseCase.Handle(query, CancellationToken.None));

        _services.S3ManagerMock.Verify(
            s3 => s3.DownloadFileAsync(fileKey),
            Times.Once
        );
    }
}
