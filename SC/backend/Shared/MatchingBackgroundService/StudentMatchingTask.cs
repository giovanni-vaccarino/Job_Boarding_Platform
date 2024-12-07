namespace backend.Shared.MatchingBackgroundService;

public class StudentMatchingTask : IBackgroundTask
{
    private readonly int _studentId;

    public StudentMatchingTask(int studentId)
    {
        _studentId = studentId;
    }
    
    public async Task ExecuteAsync()
    {
        await Task.Delay(15000);

        Console.WriteLine($"Performed statistical analysis for student {_studentId}");;
        
    }
}