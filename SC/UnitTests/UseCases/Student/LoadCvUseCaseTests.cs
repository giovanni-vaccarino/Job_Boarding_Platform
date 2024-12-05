using backend.Business.Student.LoadCvUseCase;
using backend.Data;
using backend.Service.Contracts.Student;
using Microsoft.AspNetCore.Http;
using Moq;

namespace UnitTests.UseCases.Student;

/// <summary>
/// Unit tests for the <see cref="LoadCvUseCase"/>.
/// </summary>
public class LoadCvUseCaseTests
{
    private readonly IsolatedUseCaseTestServices<LoadCvUseCase> _services;
    private readonly AppDbContext _dbContext;
    private readonly LoadCvUseCase _loadCvUseCase;

    /// <summary>
    /// Initializes a new instance of the <see cref="LoadCvUseCaseTests"/> class.
    /// Sets up the isolated services, database context, and use case instance for testing.
    /// </summary>
    public LoadCvUseCaseTests()
    {
        _services = new IsolatedUseCaseTestServices<LoadCvUseCase>("LoadCvUseCaseTests");
        _dbContext = _services.DbContext;

        _loadCvUseCase = new LoadCvUseCase(_dbContext, _services.S3ManagerMock.Object);
    }

    /// <summary>
    /// Tests that the CV file is successfully uploaded when the provided file is valid.
    /// </summary>
    [Fact(DisplayName = "Upload file successfully when file is valid")]
    public async Task Handle_ShouldUploadFile_WhenFileIsValid()
    {
        var existingStudent = new backend.Data.Entities.Student
        {
            UserId = 1,
            Name = "Test Student",
            Cf = "AAABBB00H00A000A"
        };
        _dbContext.Students.Add(existingStudent);
        await _dbContext.SaveChangesAsync();

        var fileMock = new Mock<IFormFile>();
        fileMock.Setup(f => f.Length).Returns(1024);
        fileMock.Setup(f => f.FileName).Returns("cv.pdf");
        fileMock.Setup(f => f.OpenReadStream()).Returns(new MemoryStream());

        var command = new LoadCvCommand( new LoadCvFileDto{File = fileMock.Object}, existingStudent.Id);
        
        _services.S3ManagerMock
            .Setup(s3 => s3.UploadFileAsync(It.IsAny<Stream>(), It.IsAny<string>()))
            .Returns(Task.CompletedTask); 

        var result = await _loadCvUseCase.Handle(command, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Contains($"students/{existingStudent.Id}/cv", result);

        _services.S3ManagerMock.Verify(
            s3 => s3.UploadFileAsync(
                It.Is<Stream>(stream => stream != null),
                It.Is<string>(key => key.Contains("students"))
            ),
            Times.Once
        );
    }
    
    /// <summary>
    /// Tests that an <see cref="ArgumentException"/> is thrown when the provided file is empty.
    /// </summary>
    [Fact(DisplayName = "Throw exception when file is empty")]
    public async Task Handle_ShouldThrowArgumentException_WhenFileIsEmpty()
    {
        var existingStudent = new backend.Data.Entities.Student
        {
            UserId = 1,
            Name = "Test Student",
            Cf = "AAABBB00H00A000A"
        };
        _dbContext.Students.Add(existingStudent);
        await _dbContext.SaveChangesAsync();

        var fileMock = new Mock<IFormFile>();
        fileMock.Setup(f => f.Length).Returns(0);
        fileMock.Setup(f => f.FileName).Returns("cv.pdf");

        var command = new LoadCvCommand(new LoadCvFileDto { File = fileMock.Object }, existingStudent.Id);

        await Assert.ThrowsAsync<ArgumentException>(() => _loadCvUseCase.Handle(command, CancellationToken.None));

        _services.S3ManagerMock.Verify(
            s3 => s3.UploadFileAsync(It.IsAny<Stream>(), It.IsAny<string>()),
            Times.Never
        );
    }

}
