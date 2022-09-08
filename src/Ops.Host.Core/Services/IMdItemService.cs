namespace Ops.Host.Core.Services;

/// <summary>
/// 物料主数据服务。
/// </summary>
public interface IMdItemService : IDomainService
{
    Task<MdItem> GetAsync(int id);

    Task<PagedList<MdItem>> GetPagedListAsync(MdItemFilter filter, int pageIndex, int pageSize);

    Task<(bool ok, string err)> InsertOrUpdateAsync(MdItem input);

    Task<bool> DeleteAsync(long id);
}
