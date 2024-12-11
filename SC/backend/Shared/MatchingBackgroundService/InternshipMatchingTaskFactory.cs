using backend.Data;

namespace backend.Shared.MatchingBackgroundService;

public class InternshipMatchingTaskFactory : IInternshipMatchingTaskFactory
{
    private readonly IServiceProvider _serviceProvider;

    public InternshipMatchingTaskFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public InternshipMatchingTask Create(int internshipId)
    {
        return new InternshipMatchingTask(internshipId, _serviceProvider);
    }
}
