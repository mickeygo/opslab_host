namespace Ops.Host.Core.Services.Impl;

internal sealed class ProcProcessBomService : IProcProcessBomService
{
    private readonly SqlSugarRepository<ProcProcessBom> _bomRep;

    public ProcProcessBomService(SqlSugarRepository<ProcProcessBom> bomRep)
    {
        _bomRep = bomRep;
    }

    public async Task<ProcProcessBom> GetBomByIdAsync(long id)
    {
        return await _bomRep.AsQueryable()
            .Includes(s => s.Process)
            .Includes(s => s.Product)
            .Includes(s => s.Contents, it => it.Material)
            .FirstAsync(s => s.Id == id);
    }

    public async Task<ProcProcessBom> GetBomAsync(long productId, long processId)
    {
        return await _bomRep.AsQueryable()
            .Includes(s => s.Process)
            .Includes(s => s.Product)
            .Includes(s => s.Contents, it => it.Material)
            .FirstAsync(s => s.ProductId == productId && s.ProcessId == processId);
    }

    public async Task<ProcProcessBom> GetBomAsync(string productCode, string lineCode, string stationCode)
    {
        return await _bomRep.AsQueryable()
            .Includes(s => s.Process, it => it!.Station!.LineCode == lineCode && it.Station.StationCode == stationCode)
            .Includes(s => s.Product!.Code == productCode)
            .Includes(s => s.Contents, it => it.Material)
            .FirstAsync();
    }

    public async Task<ProcProcessBom> GetBomByProductCodeAsync(string productCode, long stationId)
    {
        return await _bomRep.AsQueryable()
            .Includes(s => s.Process!.StationId == stationId)
            .Includes(s => s.Product!.Code == productCode)
            .Includes(s => s.Contents, it => it.Material)
            .FirstAsync();
    }

    public async Task<PagedList<ProcProcessBom>> GetPagedListAsync(ProcessBomFilter filter, int pageIndex, int pageSize)
    {
        return await _bomRep.AsQueryable()
            .Includes(s => s.Process)
            .Includes(s => s.Product)
            .Includes(s => s.Contents, it => it.Material)
            .WhereIF(!string.IsNullOrWhiteSpace(filter.ProductCode), s => s.Product!.Code.Contains(filter.ProductCode!))
            .WhereIF(!string.IsNullOrWhiteSpace(filter.ProductName), s => s.Product!.Name.Contains(filter.ProductName!))
            .ToPagedListAsync(pageIndex, pageSize);
    }

    public async Task<(bool ok, string err)> InsertOrUpdateAsync(ProcProcessBom input)
    {
        // 新增数据，检查产品 BOM 是否已存在（同一产品在同一工序下不能存在多个产品 BOM）。
        if (input.IsTransient() && (await _bomRep.IsAnyAsync(s => s.ProductId == input.ProductId && s.ProcessId == input.ProcessId)))
        {
            return (false, $"工艺BOM 在该工序中已存在");
        }

        if (input.IsTransient())
        {
            var ok = await _bomRep.AsSugarClient().InsertNav(input).Include(s => s.Contents).ExecuteCommandAsync();
            return (ok, "");
        }

        var ok2 = await _bomRep.AsSugarClient().UpdateNav(input).Include(s => s.Contents).ExecuteCommandAsync();
        return (ok2, "");
    }

    public async Task<(bool ok, string err)> DeleteAsync(long id)
    {
        var ok = await _bomRep.AsSugarClient()
           .DeleteNav<ProcProcessBom>(s => s.Id == id)
           .Include(s => s.Contents)
           .ExecuteCommandAsync();
        return (ok, "");
    }

    public async Task<(bool ok, string err)> CopyAsync(long sourceProductId, long targetProductId)
    {
        if (sourceProductId == targetProductId)
        {
            return (false, "要复制的源产品与目标产品不能相同");
        }

        if (!await _bomRep.IsAnyAsync(s => s.ProductId == sourceProductId))
        {
            return (false, "源产品的工艺BOM不存在");
        }

        if (await _bomRep.IsAnyAsync(s => s.ProductId == targetProductId))
        {
            return (false, "目标产品的工艺BOM已存在");
        }

        var sources = await _bomRep.AsQueryable()
            .Includes(s => s.Contents)
            .Where(s => s.ProductId == sourceProductId)
            .ToListAsync();
        sources.ForEach(s =>
        {
            s.Id = 0;
            s.ProductId = targetProductId;
        });

        var ok = await _bomRep.AsSugarClient()
            .InsertNav(sources)
            .Include(s => s.Contents)
            .ExecuteCommandAsync();
        return (ok, "");
    }
}
