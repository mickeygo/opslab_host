using Quartz;

namespace Ops.Host.Common.Job;

public class QuartzJobListener : IJobListener
{
    private readonly ILogger _logger;

    public string Name => "OpsHostJobListener";

    public QuartzJobListener(ILogger<QuartzJobListener> logger)
    {
        _logger = logger;
    }

    public Task JobToBeExecuted(IJobExecutionContext context, CancellationToken cancellationToken = default)
    {
        _logger.LogDebug($"Job {context.JobDetail.JobType.Name} executing...");
        return Task.FromResult(0);
    }

    public Task JobExecutionVetoed(IJobExecutionContext context, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation($"Job {context.JobDetail.JobType.Name} executing operation vetoed...");
        return Task.FromResult(0);
    }

    public Task JobWasExecuted(IJobExecutionContext context, JobExecutionException? jobException, CancellationToken cancellationToken = default)
    {
        if (jobException is null)
        {
            _logger.LogDebug($"Job {context.JobDetail.JobType.Name} successfully executed.");
        }
        else
        {
            _logger.LogError(jobException, $"Job {context.JobDetail.JobType.Name} failed with exception: {jobException.Message}");
        }

        return Task.FromResult(0);
    }
}
