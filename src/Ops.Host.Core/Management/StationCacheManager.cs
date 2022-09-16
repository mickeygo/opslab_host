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
    /// 获取所有的产线。
    /// </summary>
    public List<NameValue> Lines => GetStations().Select(s => new NameValue(s.LineName, s.LineCode)).ToList();

    /// <summary>
    /// 获取所有的工站。
    /// </summary>
    public List<NameValue> Stations => GetStations().Select(s => new NameValue(s.StationName, s.StationCode)).ToList();

    /// <summary>
    /// 获取工站。
    /// </summary>
    /// <param name="stationId">工站 Id</param>
    /// <returns></returns>
    public MdStation? GetById(long stationId)
    {
        return GetStations().FirstOrDefault(s => s.Id == stationId);
    }

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
    /// 同步数据到本地（包含工站）。
    /// </summary>
    /// <returns></returns>
    public async Task SyncToLocalAsync()
    {
        var devInfos = await _deviceInfoManager.GetAllAsync();
        await _stationService.SyncToLocalAsync(devInfos);

        _memoryCache.Remove(Key);
    }

    private List<MdStation> GetStations()
    {
        return _memoryCache.GetOrCreate(Key, entry => _stationService.GetStationList());
    }
}
