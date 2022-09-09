namespace Ops.Host.App.ViewModels;

public sealed class ArchiveViewModel : AsyncSinglePagedViewModelBase<PtArchive, PtArchiveFilter>, IViewModel
{
    private readonly IPtArchiveService _archiveService;
    private readonly StationCacheManager _stationManager;

    public ArchiveViewModel(IPtArchiveService archiveService, StationCacheManager stationManager)
    {
        _archiveService = archiveService;
        _stationManager = stationManager;
    }

    public List<NameValue> StationDropdownList => _stationManager.Stations;

    protected override async Task<PagedList<PtArchive>> OnSearchAsync(int pageIndex, int pageSize)
    {
        return await _archiveService.GetPagedListAsync(QueryFilter, pageIndex, pageSize);
    }

    protected override void OnExcelModelCreating(ExcelModelBuilder builder)
    {
        builder.ExcelName = $"{QueryFilter.LineCode}-{QueryFilter.StationCode}过站信息";

        // TODO: 存档数据导出复杂的 DataTable 数据。
    }
}
