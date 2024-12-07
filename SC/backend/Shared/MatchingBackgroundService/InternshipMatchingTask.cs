namespace backend.Shared.MatchingBackgroundService;

public class InternshipMatchingTask : IBackgroundTask
{
    private readonly int _internshipId;

    public InternshipMatchingTask(int internshipId)
    {
        _internshipId = internshipId;
    } 
    
    public async Task ExecuteAsync()
    {
        await Task.Delay(15000);

        Console.WriteLine($"Performed statistical analysis for internship ID: {_internshipId}");
    }
}