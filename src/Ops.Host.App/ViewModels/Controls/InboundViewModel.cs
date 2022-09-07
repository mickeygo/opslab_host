namespace Ops.Host.App.ViewModels;

public sealed class InboundViewModel : AsyncSinglePagedViewModelBase<PtInbound, PtInboundFilter>, IViewModel
{
    private readonly IPtInboundService _inboundService;
    private readonly StationCacheManager _stationManager;

    public InboundViewModel(IPtInboundService inboundService, StationCacheManager stationManager)
    {
        _inboundService = inboundService;
        _stationManager = stationManager;
    }

    public List<NameValue> StationDropdownList => _stationManager.Stations;

    protected override async Task<PagedList<PtInbound>> OnSearchAsync(int pageIndex, int pageSize)
    {
        return await _inboundService.GetPagedListAsync(QueryFilter, pageIndex, pageSize);
    }
}
