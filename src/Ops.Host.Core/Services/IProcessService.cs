namespace Ops.Host.Core.Services;

/// <summary>
/// 工艺服务。
/// </summary>
public interface IProcessService : IDomainService
{
    Task<PagedList<ProcProcess>> GetPagedListAsync(ProcProcessFilter filter, int pageIndex, int pageSize);

    /// <summary>
    /// 同步工站信息到工序。
    /// </summary>
    /// <remarks>没有则新增，存在则更新（同时会同步已设定的工艺参数），不会删除数据。</remarks>
    /// <returns></returns>
    Task<(bool ok, string err)> SyncStationToProcessAsync();

    /// <summary>
    /// 更新工艺参数。
    /// </summary>
    /// <returns></returns>
    Task<(bool ok, string err)> UpdateParameterAsync(ProcProcess input);
}
