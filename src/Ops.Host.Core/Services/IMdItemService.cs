namespace Ops.Host.Core.Services;

/// <summary>
/// 物料主数据服务。
/// </summary>
public interface IMdItemService : IDomainService
{
    Task<MdItem> GetAsync(long id);

    /// <summary>
    /// 获取产品集合。
    /// </summary>
    /// <returns></returns>
    List<MdItem> GetProducts();

    /// <summary>
    /// 获取关键物料集合。
    /// </summary>
    /// <returns></returns>
    List<MdItem> GetCriticalMaterials();

    Task<PagedList<MdItem>> GetPagedListAsync(MdItemFilter filter, int pageIndex, int pageSize);

    Task<(bool ok, string err)> InsertOrUpdateAsync(MdItem input);

    Task<bool> DeleteAsync(long id);
}
