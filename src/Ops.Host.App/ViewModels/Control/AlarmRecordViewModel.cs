namespace Ops.Host.App.ViewModels;

public sealed class AlarmRecordViewModel : AsyncSinglePagedViewModelBase<PtAlarmRecord, AlarmRecordFilter>, IViewModel
{
    private readonly IPtAlarmRecordService _alarmRecordService;
    private readonly StationCacheManager _stationManager;

    public AlarmRecordViewModel(IPtAlarmRecordService alarmRecordService, StationCacheManager stationManager)
    {
        _alarmRecordService = alarmRecordService;
        _stationManager = stationManager;
    }

    public List<NameValue> StationDropdownList => _stationManager.Stations.MakeDropdownList();

    protected override Task<PagedList<PtAlarmRecord>> OnSearchAsync(int pageIndex, int pageSize)
    {
        return _alarmRecordService.GetPagedListAsync(QueryFilter, pageIndex, pageSize);
    }

    protected override void OnExcelModelCreating(ExcelModelBuilder builder)
    {
        builder.ExcelName = "警报记录";
    }
}
