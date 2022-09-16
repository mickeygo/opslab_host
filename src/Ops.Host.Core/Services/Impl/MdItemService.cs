namespace Ops.Host.Core.Services.Impl;

internal sealed class MdItemService : IMdItemService
{
    private readonly SqlSugarRepository<MdItem> _itemRep;

    public MdItemService(SqlSugarRepository<MdItem> itemRep)
    {
        _itemRep = itemRep;
    }

    public async Task<MdItem> GetAsync(long id)
    {
        return await _itemRep.GetByIdAsync(id);
    }

    public List<MdItem> GetProducts()
    {
        return _itemRep.GetList(s => s.Attr == MaterialAttrEnum.Product);
    }

    public List<MdItem> GetCriticalMaterials()
    {
        return _itemRep.GetList(s => s.Attr == MaterialAttrEnum.Critical);
    }

    public async Task<PagedList<MdItem>> GetPagedListAsync(MdItemFilter filter, int pageIndex, int pageSize)
    {
        return await _itemRep.AsQueryable()
            .WhereIF(!string.IsNullOrWhiteSpace(filter.Code), s => s.Code.Contains(filter.Code!))
            .WhereIF(!string.IsNullOrWhiteSpace(filter.Name), s => s.Name.Contains(filter.Name!))
            .WhereIF(filter.Attr != null, s => s.Attr == filter.Attr)
            .ToPagedListAsync(pageIndex, pageSize);
    }
   
    public async Task<(bool ok, string err)> InsertOrUpdateAsync(MdItem input)
    {
        // 新增数据，检查用户是否已存在
        if (input.IsTransient() && _itemRep.IsAny(s => s.Code == input.Code))
        {
            return (false, $"物料编码 '{input.Code}' 已存在");
        }

        var ok = await _itemRep.InsertOrUpdateAsync(input);
        return (ok, "");
    }

    public async Task<bool> DeleteAsync(long id)
    {
        return await _itemRep.DeleteByIdAsync(id);
    }
}
