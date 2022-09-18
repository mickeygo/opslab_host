namespace Ops.Host.Core.Services;

/// <summary>
/// 工单服务
/// </summary>
public interface IProdWoService : IDomainService
{
    Task<PagedList<ProdWo>> GetPagedListAsync(ProdWoFilter filter, int pageIndex, int pageSize);

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
    /// 删除工单。
    /// </summary>
    /// <param name="woId"></param>
    /// <returns></returns>
    Task<(bool ok, string err)> DeleteAsync(long woId);
}
