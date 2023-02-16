namespace Ops.Host.Core.Services.Impl;

internal sealed class MdStationService : IMdStationService
{
    private readonly SqlSugarRepository<MdStation> _stationRep;

    public MdStationService(SqlSugarRepository<MdStation> stationRep)
    {
        _stationRep = stationRep;
    }

    public List<MdStation> GetStationList()
    {
        return _stationRep.GetList();
    }

    public async Task<List<MdStation>> GetStationListAsync()
    {
        return await _stationRep.GetListAsync();
    }

    public async Task<PagedList<MdStation>> GetPagedListAsync(StationFilter filter, int pageIndex, int pageSize)
    {
        return await _stationRep.AsQueryable()
            .WhereIF(!string.IsNullOrWhiteSpace(filter.LineCode), s => s.LineCode == filter.LineCode)
            .WhereIF(!string.IsNullOrWhiteSpace(filter.StationCode), s => s.StationCode.Contains(filter.StationCode!))
            .ToPagedListAsync(pageIndex, pageSize);
    }

    public async Task SyncToLocalAsync(IEnumerable<DeviceInfo> deviceInfos)
    {
        var stations = deviceInfos.Select(s => new MdStation
        {
            LineCode = s.Schema.Line,
            LineName = s.Schema.LineName,
            StationCode = s.Schema.Station,
            StationName = s.Schema.StationName,
            DeviceInfoExt = s,
        });

        foreach (var station in stations)
        {
            // 同步工站
            var station0 = await _stationRep.GetFirstAsync(s => s.LineCode == station.LineCode && s.StationCode == station.StationCode);
            if (station0 == null)
            {
                station.Type = StationTypeEnum.Assembly;
                station.Owner = StationOwnerEnum.Inline;
                await _stationRep.InsertAsync(station);
            }
            else
            {
                station0.LineName = station.LineName;
                station0.StationName = station.StationName;
                station0.DeviceInfoExt = station.DeviceInfoExt;
                
                await _stationRep.AsUpdateable(station0).UpdateColumns(s => new { s.LineName, s.StationName, s.DeviceInfoExt, s.UpdateTime }).ExecuteCommandAsync();
            }
        }
    }

    public async Task<(bool ok, string err)> UpdateTypeAndOwnerAsync(MdStation input)
    {
        var station = await _stationRep.GetByIdAsync(input.Id);
        if (station is not null)
        {
            station.Type = input.Type;
            station.Owner = input.Owner;
            await _stationRep.AsUpdateable(station).UpdateColumns(s => new { s.Type, s.Owner, s.UpdateTime }).ExecuteCommandAsync();
        }

        return (true, "");
    }
}
