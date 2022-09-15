namespace Ops.Host.Core.Services.Impl;

internal sealed class ProcRouteService : IProcRouteService
{
    private readonly SqlSugarRepository<ProcRoute> _routeRep;

    public ProcRouteService(SqlSugarRepository<ProcRoute> routeRep)
    {
        _routeRep = routeRep;
    }

    public async Task<List<ProcRoute>> GetAsync(long id)
    {
        return await _routeRep.AsQueryable()
                .Includes(s => s.Items)
                .Where(s => s.Id == id)
                .ToListAsync();
    }

    public List<ProcRoute> GetAll()
    {
        return _routeRep.AsQueryable()
            .Includes(s => s.Items)
            .ToList();
    }

    public async Task<List<ProcRoute>> GetAllAsync()
    {
        return await _routeRep.AsQueryable()
                .Includes(s => s.Items)
                .ToListAsync();
    }

    public async Task<PagedList<ProcRoute>> GetPagedListAsync(ProcRouteFilter filter, int pageIndex, int pageSize)
    {
        return await _routeRep.AsQueryable()
                .Includes(s => s.Items)
                .WhereIF(!string.IsNullOrWhiteSpace(filter.Code), s => s.Code.Contains(filter.Code!))
                .WhereIF(!string.IsNullOrWhiteSpace(filter.Name), s => s.Name.Contains(filter.Name!))
                .ToPagedListAsync(pageIndex, pageSize);
    }
}
