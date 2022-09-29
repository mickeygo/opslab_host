namespace Ops.Host.Core.Services;

/// <summary>
/// 工单服务
/// </summary>
public interface IProdWoService : IDomainService
{
    Task<PagedList<ProdWo>> GetPagedListAsync(ProdWoFilter filter, int pageIndex, int pageSize);

    Task<(bool ok, string err)> InsertOrUpdateAsync(ProdWo input);

    /// <summary>
    /// 下发工单。
    /// </summary>
    /// <param name="woId">工单 Id</param>
    /// <returns></returns>
    Task<(bool ok, string err)> IssueAsync(long woId);

    /// <summary>
    /// 暂停工单。
    /// </summary>
    /// <param name="woId">工单 Id</param>
    /// <returns></returns>
    Task<(bool ok, string err)> PauseAsync(long woId);

    /// <summary>
    /// 恢复工单。
    /// </summary>
    /// <param name="woId">工单 Id</param>
    /// <returns></returns>
    Task<(bool ok, string err)> ResumeAsync(long woId);

    /// <summary>
    /// 工单完工。
    /// </summary>
    /// <param name="woId"></param>
    /// <remarks>注：完工同时会删除该工单排程。</remarks>
    /// <returns></returns>
    Task<(bool ok, string err)> CompleteAsync(long woId);

    /// <summary>
    /// 删除工单。
    /// </summary>
    /// <param name="woId">工单 Id</param>
    /// <remarks>注：同时也会删除该工单排程。</remarks>
    /// <returns></returns>
    Task<(bool ok, string err)> DeleteAsync(long woId);
}
