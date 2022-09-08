using Ops.Exchange.Management;
using Ops.Host.Core.Services;

namespace Ops.Host.Core.Management;

/// <summary>
/// 工站信息缓存管理
/// </summary>
public sealed class StationCacheManager : IManager
{
    const string Key = "ops.host.cache.mdstation";

    private readonly IMemoryCache _memoryCache;
    private readonly IMdStationService _stationService;
    private readonly DeviceInfoManager _deviceInfoManager;

    public StationCacheManager(IMemoryCache cache, IMdStationService stationService, DeviceInfoManager deviceInfoManager)
    {
        _memoryCache = cache;
        _stationService = stationService;
        _deviceInfoManager = deviceInfoManager;
    }

    /// <summary>
    /// 获取所有的工站。
    /// </summary>
    public List<NameValue> Stations => GetStations().Select(s => new NameValue(s.StationName, s.StationCode)).ToList();

    /// <summary>
    /// 获取指定设备指定标签的变量。
    /// </summary>
    /// <returns></returns>
    public DeviceVariable? GetDeviceVariable(string lineCode, string stationCode, string tag)
    {
        var station = GetStations().FirstOrDefault(s => s.LineCode == lineCode && s.StationCode == stationCode);
        if (station != null)
        {
            return station.DeviceInfoExt?.GetVariable(tag);
        }

        return default;
    }

    /// <summary>
    /// 刷新数据。
    /// </summary>
    /// <returns></returns>
    public async Task RefreshAsync()
    {
        var devInfos = await _deviceInfoManager.GetAllAsync();
        await _stationService.InsertOrUpdateAsync(devInfos);

        _memoryCache.Remove(Key);
    }

    private List<MdStation> GetStations()
    {
        var devInfos = _stationService.GetStationList();
        return _memoryCache.GetOrCreate(Key, entry => devInfos);
    }
}
