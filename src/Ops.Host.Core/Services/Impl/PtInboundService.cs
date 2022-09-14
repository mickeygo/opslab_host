namespace Ops.Host.Core.Services.Impl;

public sealed class PtInboundService : IPtInboundService
{
    private readonly SqlSugarRepository<PtInbound> _inboundRep;

    public PtInboundService(SqlSugarRepository<PtInbound> inboundRep)
    {
        _inboundRep = inboundRep;
    }

    public async Task<PagedList<PtInbound>> GetPagedListAsync(PtInboundFilter filter, int pageIndex, int pageSize)
    {
        return await _inboundRep.AsQueryable().Includes(t => t.InboundItems)
                .WhereIF(!string.IsNullOrWhiteSpace(filter.LineCode), s => s.LineCode == filter.LineCode)
                .WhereIF(!string.IsNullOrWhiteSpace(filter.StationCode), s => s.StationCode == filter.StationCode)
                .WhereIF(!string.IsNullOrWhiteSpace(filter.SN), s => s.SN.Contains(filter.SN!))
                .WhereIF(filter.CreateTimeStart != null, s => s.CreateTime >= filter.CreateTimeStart.ToDayMin())
                .WhereIF(filter.CreateTimeEnd != null, s => s.CreateTime <= filter.CreateTimeEnd.ToDayMax())
                .ToPagedListAsync(pageIndex, pageSize);
    }
}
