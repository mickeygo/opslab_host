namespace Ops.Host.Core.Services.Impl;

internal sealed class MdItemService : IMdItemService
{
    private readonly SqlSugarRepository<MdItem> _itemRep;

    public MdItemService(SqlSugarRepository<MdItem> itemRep)
    {
        _itemRep = itemRep;
    }

    public async Task<MdItem> GetAsync(int id)
    {
        return await _itemRep.GetByIdAsync(id);
    }

    public async Task<PagedList<MdItem>> GetPagedListAsync(MdItemFilter filter, int pageIndex, int pageSize)
    {
        return await _itemRep.AsQueryable()
            .WhereIF(!string.IsNullOrWhiteSpace(filter.Code), s => s.Code.Contains(filter.Code!))
            .WhereIF(!string.IsNullOrWhiteSpace(filter.Name), s => s.Name.Contains(filter.Name!))
            .WhereIF(filter.Attr != null, s => s.Attr == filter.Attr)
            .ToPagedListAsync(pageIndex, pageSize);
    }
   
    public async Task<bool> InsertOrUpdateAsync(MdItem input)
    {
        return await _itemRep.InsertOrUpdateAsync(input);
    }

    public async Task<bool> DeleteAsync(long id)
    {
        return await _itemRep.DeleteByIdAsync(id);
    }
}
