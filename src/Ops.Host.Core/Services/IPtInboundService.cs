namespace Ops.Host.Core.Services;

/// <summary>
/// 产品进站服务
/// </summary>
public interface IPtInboundService : IDomainService
{
    Task<PagedList<PtInbound>> GetPagedListAsync(PtInboundFilter filter, int pageIndex, int pageSize);
}
