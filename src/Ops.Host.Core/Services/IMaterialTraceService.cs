namespace Ops.Host.Core.Services;

/// <summary>
/// 物料追溯信息服务
/// </summary>
public interface IMaterialTraceService : IDomainService
{
    Task<PagedList<PtSnMaterial>> GetPagedListAsync(MaterialTraceFilter filter, int pageIndex, int pageSize);

    /// <summary>
    /// 解绑
    /// </summary>
    Task Unbind(PtSnMaterial input);
}
