namespace Ops.Host.App.ViewModels;

public sealed class WoScheduleViewModel : ObservableViewModelBase, IViewModel
{
    private readonly IProdScheduleService _scheduleService;

    public WoScheduleViewModel(IProdScheduleService scheduleService)
    {
        _scheduleService = scheduleService;

        RefreshCommand = new AsyncRelayCommand(RefreshAsync);
        ScheduleCommand = new AsyncRelayCommand<ProdWo>(ScheduleAsync!);
        DisScheduleCommand = new AsyncRelayCommand<ProdSchedule>(DisScheduleAsync!);
        UpCommand = new AsyncRelayCommand<ProdSchedule>(UpAsync!);
        DownCommand = new AsyncRelayCommand<ProdSchedule>(DownAsync!);
    }

    private ObservableCollection<ProdWo>? _issueWoSourceList;
    public ObservableCollection<ProdWo>? IssueWoSourceList
    {
        get => _issueWoSourceList;
        set => SetProperty(ref _issueWoSourceList, value);
    }

    private ObservableCollection<ProdSchedule>? _scheduleSourceList;
    public ObservableCollection<ProdSchedule>? ScheduleSourceList
    {
        get => _scheduleSourceList;
        set => SetProperty(ref _scheduleSourceList, value);
    }

    public ICommand RefreshCommand { get; }

    public ICommand ScheduleCommand { get; }

    public ICommand DisScheduleCommand { get; }

    public ICommand UpCommand { get; }

    public ICommand DownCommand { get; }

    /// <summary>
    /// 刷新界面数据
    /// </summary>
    /// <returns></returns>
    private async Task RefreshAsync()
    {
        IssueWoSourceList = new(await _scheduleService.GetAllIssueAsync());
        ScheduleSourceList = new(await _scheduleService.GetAllScheduleAsync());
    }

    private async Task ScheduleAsync(ProdWo item)
    {
        var (ok, schedule, err) = await _scheduleService.ScheduleAsync(item);
        if (!ok)
        {
            NoticeInfo(err);
            return;
        }

        IssueWoSourceList?.Remove(item);
        ScheduleSourceList?.Add(schedule!);
    }

    private async Task DisScheduleAsync(ProdSchedule item)
    {
        var (ok, wo, err) = await _scheduleService.DisScheduleAsync(item);
        if (!ok)
        {
            NoticeInfo(err);
            return;
        }

        ScheduleSourceList?.Remove(item);
        IssueWoSourceList?.Add(wo!);
    }

    private async Task UpAsync(ProdSchedule item)
    {
        var index = ScheduleSourceList!.IndexOf(item);
        if (index == 0)
        {
            return;
        }

        var prev = ScheduleSourceList[index - 1];
        var (ok, err) = await _scheduleService.UpScheduleAsync(item, prev);
        if (!ok)
        {
            NoticeInfo(err);
            return;
        }

        ScheduleSourceList.RemoveAt(index);
        ScheduleSourceList.Insert(index - 1, item);
    }

    private async Task DownAsync(ProdSchedule item)
    {
        var index = ScheduleSourceList!.IndexOf(item);
        if (index == ScheduleSourceList.Count - 1)
        {
            return;
        }

        var next = ScheduleSourceList[index + 1];
        var (ok, err) = await _scheduleService.DownScheduleAsync(item, next);
        if (!ok)
        {
            NoticeInfo(err);
            return;
        }

        ScheduleSourceList.RemoveAt(index);
        ScheduleSourceList.Insert(index + 1, item);
    }
}
