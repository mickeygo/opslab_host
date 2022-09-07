namespace Ops.Host.Core.Services;

/// <summary>
/// 出站/存档服务
/// </summary>
public interface IPtArchiveService : IDomainService
{
    Task<PagedList<PtArchive>> GetPagedListAsync(PtArchiveFilter filter, int pageIndex, int pageSize);
}
