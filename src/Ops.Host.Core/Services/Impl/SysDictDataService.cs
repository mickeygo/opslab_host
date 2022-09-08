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

    public async Task<(bool ok, string err)> InsertOrUpdateDictAsync(SysDictData input)
    {
        var dictData = await _dictRep.GetFirstAsync(s => s.Code == input.Code && s.Name == input.Name);

        // 校验字典类型和名称是否有重复
        if (input.IsTransient())
        {
            if (dictData != null)
            {
                return (false, "字典类型中已存在此名称");
            }
        }
        else
        {
            if (dictData.Id != input.Id)
            {
                return (false, "字典类型中已存在此名称");
            }
        }

        var ok = await _dictRep.InsertOrUpdateAsync(input);
        return (ok, "");
    }

    public async Task<bool> DeleteDictAsync(long id)
    {
        return await _dictRep.DeleteByIdAsync(id);
    }
}
