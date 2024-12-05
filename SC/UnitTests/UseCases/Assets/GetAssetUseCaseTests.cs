using backend.Business.Assets.GetAssetUseCase;
using backend.Business.Student.LoadCvUseCase;
using backend.Data;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace UnitTests.UseCases.Assets;

public class GetAssetUseCaseTests
{
    private readonly IsolatedUseCaseTestServices<GetAssetUseCase> _services;
    private readonly AppDbContext _dbContext;
    private readonly GetAssetUseCase _getAssetUseCase;

    public GetAssetUseCaseTests()
    {
        _services = new IsolatedUseCaseTestServices<GetAssetUseCase>("GetAssetUseCaseTests");
        _dbContext = _services.DbContext;

        _getAssetUseCase = new GetAssetUseCase(_dbContext, _services.S3ManagerMock.Object);
    }

    [Fact]
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

    [Fact]
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
