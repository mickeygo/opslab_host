using Quartz;

namespace Ops.Host.Common.Job;

public sealed class QuartzScheduleJobManager : IQuartzScheduleJobManager
{
    public Task ScheduleAsync<TJob>(Action<JobBuilder> configureJob, Action<TriggerBuilder> configureTrigger) where TJob : IJob
    {
        throw new NotImplementedException();
    }

    public Task RescheduleAsync(TriggerKey triggerKey, Action<TriggerBuilder> configureTrigger)
    {
        throw new NotImplementedException();
    }

    public Task UnscheduleAsync(TriggerKey triggerKey)
    {
        throw new NotImplementedException();
    }
}
