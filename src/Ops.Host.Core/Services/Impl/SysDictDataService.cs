namespace Ops.Host.Core.Services.Impl;

internal sealed class SysDictDataService : ISysDictDataService
{
    private readonly SqlSugarRepository<SysDictData> _dictRep;

    public SysDictDataService(SqlSugarRepository<SysDictData> dictRep)
    {
        _dictRep = dictRep;
    }

    public async Task<SysDictData> GetDictByIdAsync(long id)
    {
        return await _dictRep.GetByIdAsync(id);
    }

    public async Task<PagedList<SysDictData>> GetPagedListAsync(SysDictDataFilter filter, int pageIndex, int pageSize)
    {
        return await _dictRep.AsQueryable()
            .WhereIF(!string.IsNullOrWhiteSpace(filter.Code), s => s.Code == filter.Code)
            .ToPagedListAsync(pageIndex, pageSize);
    }

    public async Task<bool> InsertOrUpdateDictAsync(SysDictData input)
    {
        return await _dictRep.InsertOrUpdateAsync(input);
    }

    public async Task<bool> DeleteDictAsync(long id)
    {
        return await _dictRep.DeleteByIdAsync(id);
    }
}
