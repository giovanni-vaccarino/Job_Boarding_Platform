namespace backend.Shared.MatchingBackgroundService;

public interface IInternshipMatchingTaskFactory
{
    InternshipMatchingTask Create(int internshipId);
}