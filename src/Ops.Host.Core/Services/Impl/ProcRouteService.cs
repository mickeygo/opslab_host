namespace Ops.Host.Core.Services.Impl;

internal sealed class ProcRouteService : IProcRouteService
{
    private readonly SqlSugarRepository<ProcRoute> _routeRep;
    private readonly SqlSugarRepository<ProcRouteProduct> _routeProductRep;

    public ProcRouteService(SqlSugarRepository<ProcRoute> routeRep, SqlSugarRepository<ProcRouteProduct> routeProductRep)
    {
        _routeRep = routeRep;
        _routeProductRep = routeProductRep;
    }

    public async Task<List<ProcRoute>> GetAsync(long id)
    {
        return await _routeRep.AsQueryable()
                .Includes(s => s.Contents)
                .Where(s => s.Id == id)
                .ToListAsync();
    }

    public List<ProcRoute> GetAll()
    {
        return _routeRep.AsQueryable()
            .Includes(s => s.Contents)
            .ToList();
    }

    public async Task<List<ProcRoute>> GetAllAsync()
    {
        return await _routeRep.AsQueryable()
                .Includes(s => s.Contents)
                .ToListAsync();
    }

    public async Task<PagedList<ProcRoute>> GetPagedListAsync(ProcRouteFilter filter, int pageIndex, int pageSize)
    {
        return await _routeRep.AsQueryable()
                .Includes(s => s.Contents, it => it.Process)
                .Includes(s => s.Contents, it => it.NextProcess)
                .Includes(s => s.LinkProducts, it => it.Product)
                .WhereIF(!string.IsNullOrWhiteSpace(filter.Code), s => s.Code.Contains(filter.Code!))
                .WhereIF(!string.IsNullOrWhiteSpace(filter.Name), s => s.Name.Contains(filter.Name!))
                .ToPagedListAsync(pageIndex, pageSize);
    }

    public async Task<(bool ok, string err)> InsertOrUpdateAsync(ProcRoute input)
    {
        // 新增数据，
        if (input.IsTransient() && _routeRep.IsAny(s => s.Code == input.Code))
        {
            return (false, $"工艺路线已存在此编码");
        }

        // 设置下一道工序
        if (input.Contents?.Count > 1)
        {
            for (int i = 0; i < input.Contents.Count - 1; i++)
            {
                input.Contents[i].NextProcessId = input.Contents[i + 1].ProcessId;
            }
        }

        if (input.IsTransient())
        {
            var ok = await _routeRep.AsSugarClient().InsertNav(input).Include(s => s.Contents).ExecuteCommandAsync();
            return (ok, "");
        }

        var ok2 = await _routeRep.AsSugarClient().UpdateNav(input).Include(s => s.Contents).ExecuteCommandAsync();
        return (ok2, "");
    }

    public async Task<(bool ok, string err)> DeleteAsync(long id)
    {
        var ok = await _routeRep.AsSugarClient()
            .DeleteNav<ProcRoute>(s => s.Id == id)
            .Include(s => s.Contents)
            .Include(s => s.LinkProducts)
            .ExecuteCommandAsync();

        return (ok, "");
    }

    public async Task<(bool ok, string err)> LinkProductAsync(ProcRouteProduct input)
    {
        if (await _routeProductRep.IsAnyAsync(s => s.RouteId == input.RouteId && s.ProductId == input.ProductId))
        {
            return (false, "工艺路线中已关联此产品");
        }

        var ok = await _routeProductRep.InsertAsync(input);
        return (ok, "");
    }

    public async Task<(bool ok, string err)> DelLinkProductAsync(long routeId, long productId)
    {
        var ok = await _routeProductRep.DeleteAsync(s => s.RouteId == routeId && s.ProductId == productId);
        return (ok, "");
    }
}
