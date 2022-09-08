namespace Ops.Host.Core.Services;

/// <summary>
/// 刷卡记录服务。
/// </summary>
public interface ISysCardRecordService : IDomainService
{
    Task<PagedList<SysCardRecord>> GetPagedListAsync(SysCardRecordFilter filter, int pageIndex, int pageSize);
}
