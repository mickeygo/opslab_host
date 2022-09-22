namespace Ops.Host.Core.Services;

/// <summary>
/// SCADA 警报信息服务。
/// </summary>
public interface IAlarmService : IDomainService
{
    Task HandleAsync(ForwardData data);
}
