namespace backend.Service.Contracts.Student;

public class UploadCvFileDto
{
    public required IFormFile File { get; set; }
}