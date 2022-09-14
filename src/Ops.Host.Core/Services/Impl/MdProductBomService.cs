namespace Ops.Host.Core.Services.Impl;

internal sealed class MdProductBomService : IMdProductBomService
{
    private readonly SqlSugarRepository<MdProductBom> _bomRep;

    public MdProductBomService(SqlSugarRepository<MdProductBom> bomRep)
	{
		_bomRep = bomRep;
	}

    public async Task<MdProductBom> GetBomByIdAsync(long id)
    {
        return await _bomRep.AsQueryable()
            .Includes(s => s.Product)
            .Includes(s => s.Items, it => it.Material)
            .FirstAsync(s => s.Id == id);
    }

    public async Task<MdProductBom> GetBomByProductIdAsync(long productId)
    {
        return await _bomRep.AsQueryable()
            .Includes(s => s.Product)
            .Includes(s => s.Items, it => it.Material)
            .FirstAsync(s => s.ProductId == productId);
    }

    public async Task<MdProductBom> GetBomByProductCodeAsync(string productCode)
    {
        return await _bomRep.AsQueryable()
            .Includes(s => s.Product!.Code == productCode)
            .Includes(s => s.Items, it => it.Material)
            .FirstAsync();
    }

    public async Task<PagedList<MdProductBom>> GetPagedListAsync(ProductBomFilter filter, int pageIndex, int pageSize)
    {
        return await _bomRep.AsQueryable()
            .Includes(s => s.Product)
            .Includes(s => s.Items, it => it.Material)
            .WhereIF(!string.IsNullOrWhiteSpace(filter.Code), s => s.Product!.Code.Contains(filter.Code!))
            .WhereIF(!string.IsNullOrWhiteSpace(filter.Name), s => s.Product!.Name.Contains(filter.Name!))
            .WhereIF(!string.IsNullOrWhiteSpace(filter.LineCode), s => s.LineCode == filter.LineCode)
            .WhereIF(!string.IsNullOrWhiteSpace(filter.StationCode), s => s.StationCode == filter.StationCode)
            .ToPagedListAsync(pageIndex, pageSize);
    }

    public async Task<(bool ok, string err)> InsertOrUpdateAsync(MdProductBom input)
    {
        // 新增数据，检查产品 BOM 是否已存在（同一产线和工站不能存在多个产品 BOM）。
        if (input.IsTransient() && _bomRep.IsAny(s => s.ProductId == input.ProductId && s.LineCode == input.LineCode && s.StationCode == input.StationCode))
        {
            return (false, $"产品 BOM 已存在");
        }

        if (input.IsTransient())
        {
            var ok = await _bomRep.AsSugarClient().InsertNav(input).Include(s => s.Items).ExecuteCommandAsync();
            return (ok, "");
        }

        var ok2 = await _bomRep.AsSugarClient().UpdateNav(input).Include(s => s.Items).ExecuteCommandAsync();
        return (ok2, "");
    }

    public async Task<(bool ok, string err)> DeleteAsync(long id)
    {
        var ok = await _bomRep.DeleteByIdAsync(id);
        return (ok, "");
    }
}
