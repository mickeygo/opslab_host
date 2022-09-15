namespace Ops.Host.App.ViewModels;

public sealed class ProcessRouteViewModel : AsyncSinglePagedViewModelBase<ProcRouteModel, ProcRouteFilter>, IViewModel
{
    private readonly IProcRouteService _routeService;
    private readonly IProcessService _processService;

    public ProcessRouteViewModel(IProcRouteService routeService, IProcessService processService)
    {
        _routeService = routeService;
        _processService = processService;

        AddProcessCommand = new AsyncRelayCommand(AddProcessAsync);
        UpCommand = new RelayCommand<ProcRouteProcessModel>(Up!);
        DownCommand = new RelayCommand<ProcRouteProcessModel>(Down!);
        DelCommand = new RelayCommand<ProcRouteProcessModel>(Del!);
    }

    public List<ProcProcess> ProcessDropdownList => _processService.GetAll();

    private long _processId;
    public long ProcessId
    {
        get => _processId;
        set => SetProperty(ref _processId, value);
    }

    public ICommand AddProcessCommand { get; }

    public ICommand UpCommand { get; }

    public ICommand DownCommand { get; }

    public ICommand DelCommand { get; }

    protected override async Task<PagedList<ProcRouteModel>> OnSearchAsync(int pageIndex, int pageSize)
    {
        var pagedList = await _routeService.GetPagedListAsync(QueryFilter, pageIndex, pageSize);
        return pagedList.Adapt<PagedList<ProcRouteModel>>();
    }

    protected override async Task<(bool ok, string? err)> OnSaveAsync(ProcRouteModel data)
    {
        var route = data.Adapt<ProcRoute>();
        var (ok, err) = await _routeService.InsertOrUpdateAsync(route);
        if (ok)
        {
            // 此处要将实体映射到 model。
            var bom1 = await _routeService.GetAsync(route.Id);
            SelectedItem = bom1.Adapt<ProcRouteModel>();
        }

        return (ok, err);
    }

    private async Task AddProcessAsync()
    {
        if (ProcessId == 0)
        {
            NoticeWarning("请先选择 [工序]");
            return;
        }

        var procs = await _processService.GetByIdAsync(ProcessId);
        //var procs0 = item.Adapt<MdItemModel>();

        var route = SelectedItem;
        route!.Contents ??= new();
        route.Contents.Add(new()
        {
            ProcessId = ProcessId,
            Process = procs,
            Seq = route.Contents.Count + 1,
        });
    }

    private void Up(ProcRouteProcessModel item)
    {
        var index = SelectedItem!.Contents!.IndexOf(item);
        if (index == 0)
        {
            return;
        }

        SelectedItem!.Contents!.RemoveAt(index);
        SelectedItem!.Contents!.Insert(index - 1, item);

        var item0 = SelectedItem!.Contents!.First(s => s.Seq == item.Seq - 1);
        item0.Seq += 1;
        item.Seq -= 1;
    }

    private void Down(ProcRouteProcessModel item)
    {
        var index = SelectedItem!.Contents!.IndexOf(item);
        if (index == SelectedItem!.Contents!.Count - 1)
        {
            return;
        }

        SelectedItem!.Contents!.RemoveAt(index);
        SelectedItem!.Contents!.Insert(index + 1, item);

        var item0 = SelectedItem!.Contents!.First(s => s.Seq == item.Seq + 1);
        item0.Seq -= 1;
        item.Seq += 1;
    }

    private void Del(ProcRouteProcessModel item)
    {
        bool isLast = SelectedItem!.Contents!.Max(s => s.Seq) == item.Seq;

        SelectedItem!.Contents!.Remove(item);
        if (!isLast)
        {
            SelectedItem!.Contents.Where(s => s.Seq > item.Seq).ToList().ForEach(s => s.Seq -= 1);
        }
    }
}
