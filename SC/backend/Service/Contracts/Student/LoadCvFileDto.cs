namespace backend.Service.Contracts.Student;

public class LoadCvFileDto
{
    public required IFormFile File { get; set; }
}