﻿namespace Ops.Host.Core.Services;

public interface IMdStationService : IDomainService
{
    List<MdStation> GetStationList();

    Task<List<MdStation>> GetStationListAsync();

    Task<PagedList<MdStation>> GetPagedListAsync(StationFilter filter, int pageIndex, int pageSize);

    /// <summary>
    /// 新增或更新信息。将地址变量配置信息同步到本地工站表中。
    /// </summary>
    /// <remarks>没有则新增，存在则更新，不会删除数据。</remarks>
    Task SyncToLocalAsync(IEnumerable<DeviceInfo> deviceInfos);

    Task<(bool ok, string err)> UpdateTypeAndOwnerAsync(MdStation input);
}
