namespace Ops.Host.Core.Services;

/// <summary>
/// 数据字典服务。
/// </summary>
public interface ISysDictDataService : IDomainService
{
    Task<SysDictData> GetDictByIdAsync(long id);

    Task<PagedList<SysDictData>> GetPagedListAsync(SysDictDataFilter filter, int pageIndex, int pageSize);

    Task<bool> InsertOrUpdateDictAsync(SysDictData input);

    Task<bool> DeleteDictAsync(long id);
}
