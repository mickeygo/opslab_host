namespace Ops.Host.Core.Services.Impl;

internal sealed class SysDictDataService : ISysDictDataService
{
    private const string CacheKey = nameof(SysDictDataService);

    private readonly SqlSugarRepository<SysDictData> _dictRep;
    private readonly IMemoryCache _cache;

    public SysDictDataService(SqlSugarRepository<SysDictData> dictRep, IMemoryCache cache)
    {
        _dictRep = dictRep;
        _cache = cache;
    }

    public async Task<List<SysDictData>> GetDicAllAsync()
    {
        return await _cache.GetOrCreateAsync(CacheKey, _ => _dictRep.GetListAsync());
    }

    public async Task<List<SysDictData>> GetDicsByCodeAsync(string code)
    {
        return (await GetDicAllAsync()).Where(s => s.Code == code).ToList();
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

    public async Task<(bool ok, string err)> InsertOrUpdateDictAsync(SysDictData input)
    {
        var dictData = await _dictRep.GetFirstAsync(s => s.Code == input.Code && s.Name == input.Name);

        // 校验字典类型和名称是否有重复
        if (dictData != null && (input.IsTransient() || dictData.Id != input.Id))
        {
            return (false, "字典类型中已存在此名称");
        }

        var ok = await _dictRep.InsertOrUpdateAsync(input);
        if (ok)
        {
            _cache.Remove(CacheKey);
        }
        return (ok, "");
    }

    public async Task<bool> DeleteDictAsync(long id)
    {
        var ok = await _dictRep.DeleteByIdAsync(id);
        if (ok)
        {
            _cache.Remove(CacheKey);
        }
        return ok;
    }
}
