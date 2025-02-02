namespace backend.Shared.MatchingBackgroundService;

public interface IStudentMatchingTaskFactory
{
    StudentMatchingTask Create(int studentId);
}