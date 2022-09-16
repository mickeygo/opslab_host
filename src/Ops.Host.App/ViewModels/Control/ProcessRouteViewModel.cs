namespace Ops.Host.App.ViewModels;

public sealed class ProcessRouteViewModel : AsyncSinglePagedViewModelBase<ProcRouteModel, ProcRouteFilter>, IViewModel
{
    private readonly IProcRouteService _routeService;
    private readonly IProcessService _processService;
    private readonly IMdItemService _itemService;

    public ProcessRouteViewModel(IProcRouteService routeService, 
        IProcessService processService, 
        IMdItemService itemService)
    {
        _routeService = routeService;
        _processService = processService;
        _itemService = itemService;

        AddProcessCommand = new RelayCommand(AddProcess);
        UpCommand = new RelayCommand<ProcRouteProcessModel>(Up!);
        DownCommand = new RelayCommand<ProcRouteProcessModel>(Down!);
        DelCommand = new RelayCommand<ProcRouteProcessModel>(Del!);

        LinkProductCommand = new AsyncRelayCommand(LinkProductAsync);
        DelLinkProductCommand = new AsyncRelayCommand<ProcRouteProduct>(DelLinkProductAsync!);
    }

    public List<ProcProcess> ProcessDropdownList => _processService.GetAll();

    public List<MdItem> ProductDropdownList => _itemService.GetProducts();

    private ProcProcess? _selectedProcProcess;
    public ProcProcess? SelectedProcProcess
    {
        get => _selectedProcProcess;
        set => SetProperty(ref _selectedProcProcess, value);
    }

    private MdItem? _selectedLinkProduct;
    public MdItem? SelectedLinkProduct
    {
        get => _selectedLinkProduct;
        set => SetProperty(ref _selectedLinkProduct, value);
    }

    private int _formulaNo;
    public int FormulaNo
    {
        get => _formulaNo;
        set => SetProperty(ref _formulaNo, value);
    }

    public ICommand AddProcessCommand { get; }

    public ICommand UpCommand { get; }

    public ICommand DownCommand { get; }

    public ICommand DelCommand { get; }

    public ICommand LinkProductCommand { get; }

    public ICommand DelLinkProductCommand { get; }

    protected override async Task<PagedList<ProcRouteModel>> OnSearchAsync(int pageIndex, int pageSize)
    {
        var pagedList = await _routeService.GetPagedListAsync(QueryFilter, pageIndex, pageSize);
        return pagedList.Adapt<PagedList<ProcRouteModel>>();
    }

    protected override async Task<(bool ok, string? err)> OnSaveAsync(ProcRouteModel data)
    {
        var route = data.Adapt<ProcRoute>();
        return await _routeService.InsertOrUpdateAsync(route);
    }

    protected override async Task<(bool ok, string? err)> OnDeleteAsync(ProcRouteModel data)
    {
        return await _routeService.DeleteAsync(data.Id);
    }

    private void AddProcess()
    {
        if (SelectedProcProcess is null)
        {
            NoticeWarning("请先选择 [工序]");
            return;
        }

        var route = SelectedItem;
        route!.Contents ??= new();
        route.Contents.Add(new()
        {
            ProcessId = SelectedProcProcess.Id,
            Process = SelectedProcProcess,
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

    private async Task LinkProductAsync()
    {
        if (SelectedItem?.Id == 0)
        {
            NoticeWarning("请选择要关联的产品");
            return;
        }

        if (SelectedLinkProduct is null)
        {
            NoticeWarning("请选择要关联的产品");
            return;
        }

        ProcRouteProduct input = new()
        {
            RouteId = SelectedItem!.Id,
            ProductId = SelectedLinkProduct.Id,
            Product = SelectedLinkProduct,
            FormulaNo = FormulaNo,
        };
        var (ok, err) = await _routeService.LinkProductAsync(input);
        if (!ok)
        {
            NoticeWarning($"关联产品失败，原因：{err}");
            return;
        }

        SelectedItem!.LinkProducts?.Add(input);
    }

    private async Task DelLinkProductAsync(ProcRouteProduct item)
    {
        var (ok, err) = await _routeService.DelLinkProductAsync(item.RouteId, item.ProductId);
        if (!ok)
        {
            NoticeWarning($"删除关联产品失败，原因：{err}");
            return;
        }

        SelectedItem!.LinkProducts?.Remove(item);
    }
}
