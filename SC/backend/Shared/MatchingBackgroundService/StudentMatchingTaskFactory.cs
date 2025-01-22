namespace backend.Shared.MatchingBackgroundService;

public class StudentMatchingTaskFactory : IStudentMatchingTaskFactory
{
    private readonly IServiceProvider _serviceProvider;
    
    public StudentMatchingTaskFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    
    public StudentMatchingTask Create(int studentId)
    {
        return new StudentMatchingTask(studentId, _serviceProvider);
    }
}