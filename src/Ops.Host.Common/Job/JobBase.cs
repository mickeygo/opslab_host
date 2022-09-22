using Quartz;

namespace Ops.Host.Common.Job;

public abstract class JobBase : IJob
{
    public abstract Task Execute(IJobExecutionContext context);
}
