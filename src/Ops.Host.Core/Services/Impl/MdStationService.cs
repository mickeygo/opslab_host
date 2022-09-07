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

    public async Task InsertOrUpdateAsync(IEnumerable<DeviceInfo> deviceInfos)
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
            var station0 = await _stationRep.GetFirstAsync(s => s.LineCode == station.LineCode && s.StationCode == station.StationCode);
            if (station0 == null)
            {
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
}
