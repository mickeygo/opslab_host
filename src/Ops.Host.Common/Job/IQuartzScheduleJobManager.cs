﻿using Quartz;

namespace Ops.Host.Common.Job;

public interface IQuartzScheduleJobManager
{
    /// <summary>
    /// Schedules a job to be executed.
    /// </summary>
    /// <typeparam name="TJob">Type of the job</typeparam>
    /// <param name="configureJob">Job specific definitions to build.</param>
    /// <param name="configureTrigger">Job specific trigger options which means calendar or time interval.</param>
    /// <returns></returns>
    Task ScheduleAsync<TJob>(Action<JobBuilder> configureJob, Action<TriggerBuilder> configureTrigger) where TJob : IJob;

    /// <summary>
    /// Reschedules a job.
    /// </summary>
    /// <param name="triggerKey">Key that identifies job's previous trigger.</param>
    /// <param name="configureTrigger">Job specific trigger options which means calendar or time interval.</param>
    /// <returns></returns>
    Task RescheduleAsync(TriggerKey triggerKey, Action<TriggerBuilder> configureTrigger);

    /// <summary>
    /// Unschedules a job.
    /// </summary>
    /// <param name="triggerKey">Key that identifies job's trigger.</param>
    /// <returns></returns>
    Task UnscheduleAsync(TriggerKey triggerKey);
}
