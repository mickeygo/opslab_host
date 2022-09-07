namespace Ops.Host.Core.Services;

public interface IMdStationService : IDomainService
{
    List<MdStation> GetStationList();

    Task<List<MdStation>> GetStationListAsync();

    /// <summary>
    /// 新增或更新信息。
    /// </summary>
    /// <remarks>没有则新增，存在则更新，不会删除数据。</remarks>
    Task InsertOrUpdateAsync(IEnumerable<DeviceInfo> deviceInfos);
}
