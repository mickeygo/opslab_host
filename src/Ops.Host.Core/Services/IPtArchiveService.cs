namespace Ops.Host.Core.Services;

/// <summary>
/// 出站/存档服务
/// </summary>
public interface IPtArchiveService : IDomainService
{
    Task<PagedList<PtArchive>> GetPagedListAsync(PtArchiveFilter filter, int pageIndex, int pageSize);

    /// <summary>
    /// 将数据转换为 DataTable，用于 Excel 导出。只会导出标记了 <see cref="DisplayNameAttribute"/> 的数据。
    /// </summary>
    /// <param name="filter"></param>
    /// <param name="extendArray">是否将数组展开</param>
    /// <param name="showLimit">是否显示数据上下限</param>
    /// <returns></returns>
    Task<DataTable> GetDataTable(PtArchiveFilter filter, bool extendArray = false, bool showLimit = false);
}
