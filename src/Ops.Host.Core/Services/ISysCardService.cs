namespace Ops.Host.Core.Services;

/// <summary>
/// 卡片信息
/// </summary>
public interface ISysCardService : IDomainService
{
    Task<SysCard> GetAsync(int id);

    Task<PagedList<SysCard>> GetPagedListAsync(SysCardFilter filter, int pageIndex, int pageSize);

    Task<(bool ok, string err)> InsertOrUpdateAsync(SysCard input);

    Task<bool> EnableAsync(long id);

    Task<bool> DisableAsync(long id);

    Task<bool> DeleteAsync(long id);
}
