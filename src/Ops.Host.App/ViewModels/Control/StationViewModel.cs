namespace Ops.Host.App.ViewModels;

public sealed class StationViewModel : AsyncSinglePagedViewModelBase<MdStation, StationFilter>, IViewModel
{
    private readonly IMdStationService _stationService;
    private readonly StationCacheManager _stationManager;

    public StationViewModel(IMdStationService stationService, StationCacheManager stationManager)
    {
        _stationService = stationService;
        _stationManager = stationManager;
    }

    public List<NameValue> LineDropdownList => _stationManager.Lines;

    public List<NameValue> TypeDropdownList { get; } = EnumExtensions.ToDropdownList<StationTypeEnum>(false);

    public List<NameValue> OwnerDropdownList { get; } = EnumExtensions.ToDropdownList<StationOwnerEnum>(false);

    protected override async Task<PagedList<MdStation>> OnSearchAsync(int pageIndex, int pageSize)
    {
        return await _stationService.GetPagedListAsync(QueryFilter, pageIndex, pageSize);
    }

    protected override async Task<(bool ok, string? err)> OnSaveAsync(MdStation data)
    {
        return await _stationService.UpdateTypeAndOwnerAsync(data);
    }
}
