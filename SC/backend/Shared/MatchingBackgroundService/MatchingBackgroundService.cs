using System.Collections.Concurrent;

namespace backend.Shared.MatchingBackgroundService;

public class MatchingBackgroundService : BackgroundService, IJobQueue
{
    private readonly ConcurrentQueue<IBackgroundTask> _jobs = new();
    private readonly SemaphoreSlim _signal = new(0);

    public MatchingBackgroundService()
    {
        Console.WriteLine("MatchingBackgroundService instantiated.");
    }
    
    public void EnqueueJob(IBackgroundTask task)
    {
        Console.WriteLine("Starting the job enqueing.");
        _jobs.Enqueue(task);
        _signal.Release();
        Console.WriteLine("Finished the job enqueing.");
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            // Wait until there's a job to process
            await _signal.WaitAsync(stoppingToken);

            if (_jobs.TryDequeue(out var job))
            {
                try
                {
                    Console.WriteLine("Executing");
                    await job.ExecuteAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error processing job: {ex.Message}");
                }
            }
        }
    }
}