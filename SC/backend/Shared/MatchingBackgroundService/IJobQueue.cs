namespace backend.Shared.MatchingBackgroundService;

public interface IJobQueue
{
    void EnqueueJob(IBackgroundTask task);
}