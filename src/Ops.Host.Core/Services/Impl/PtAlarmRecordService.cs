namespace Ops.Host.Core.Services.Impl;

internal sealed class PtAlarmRecordService : IPtAlarmRecordService
{
    private readonly SqlSugarRepository<PtAlarmRecord> _alarmRecordRep;

    public PtAlarmRecordService(SqlSugarRepository<PtAlarmRecord> alarmRecordRep)
    {
        _alarmRecordRep = alarmRecordRep;
    }

    public async Task<PagedList<PtAlarmRecord>> GetPagedListAsync(AlarmRecordFilter filter, int pageIndex, int pageSize)
    {
        return await _alarmRecordRep.AsQueryable()
                .WhereIF(!string.IsNullOrWhiteSpace(filter.StationCode), s => s.LineCode == filter.StationCode)
                .WhereIF(!string.IsNullOrWhiteSpace(filter.Descirption), s => s.Descirption!.Contains(filter.Descirption!))
                .WhereIF(filter.CreateTimeStart != null, s => s.CreateTime >= filter.CreateTimeStart.ToDayMin())
                .WhereIF(filter.CreateTimeEnd != null, s => s.CreateTime <= filter.CreateTimeEnd.ToDayMax())
                .OrderBy(s => s.CreateTime, OrderByType.Desc)
                .ToPagedListAsync(pageIndex, pageSize);
    }
}
