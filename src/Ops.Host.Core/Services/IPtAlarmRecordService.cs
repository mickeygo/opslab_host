namespace Ops.Host.Core.Services;

/// <summary>
/// 警报记录服务。
/// </summary>
public interface IPtAlarmRecordService : IDomainService
{
    Task<PagedList<PtAlarmRecord>> GetPagedListAsync(AlarmRecordFilter filter, int pageIndex, int pageSize);
}
