namespace Ops.Host.App.ViewModels;

public sealed class WoScheduleViewModel : ObservableRecipient, IViewModel
{
    private readonly IProdScheduleService _scheduleService;

    public WoScheduleViewModel(IProdScheduleService scheduleService)
    {
        _scheduleService = scheduleService;

        RefreshCommand = new RelayCommand(Refresh);
        ScheduleCommand = new AsyncRelayCommand<ProdWo>(ScheduleAsync!);
        DisScheduleCommand = new AsyncRelayCommand<ProdSchedule>(DisScheduleAsync!);
        UpCommand = new RelayCommand<ProdSchedule>(Up!);
        DownCommand = new RelayCommand<ProdSchedule>(Down!);
    }

    public ObservableCollection<ProdWo> IssueWoSourceList => new(_scheduleService.GetAllIssue());

    public ObservableCollection<ProdSchedule> ScheduleSourceList => new(); // _scheduleService.GetAllSchedule();

    public ICommand RefreshCommand { get; }

    public ICommand ScheduleCommand { get; }

    public ICommand DisScheduleCommand { get; }

    public ICommand UpCommand { get; }

    public ICommand DownCommand { get; }

    private void Refresh()
    {

    }

    private Task ScheduleAsync(ProdWo item)
    {
        return Task.CompletedTask;
    }

    private Task DisScheduleAsync(ProdSchedule item)
    {
        return Task.CompletedTask;
    }

    private void Up(ProdSchedule item)
    {
        var index = ScheduleSourceList.IndexOf(item);
        if (index == 0)
        {
            return;
        }

        ScheduleSourceList.RemoveAt(index);
        ScheduleSourceList.Insert(index - 1, item);

        var item0 = ScheduleSourceList.First(s => s.Seq == item.Seq - 1);
        item0.Seq += 1;
        item.Seq -= 1;
    }

    private void Down(ProdSchedule item)
    {
        var index = ScheduleSourceList.IndexOf(item);
        if (index == ScheduleSourceList.Count - 1)
        {
            return;
        }

        ScheduleSourceList.RemoveAt(index);
        ScheduleSourceList.Insert(index + 1, item);

        var item0 = ScheduleSourceList.First(s => s.Seq == item.Seq + 1);
        item0.Seq -= 1;
        item.Seq += 1;
    }
}
