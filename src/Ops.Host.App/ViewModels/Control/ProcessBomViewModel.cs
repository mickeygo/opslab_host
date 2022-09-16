namespace Ops.Host.App.ViewModels;

public sealed class ProcessBomViewModel : AsyncSinglePagedViewModelBase<ProcProcessBomModel, ProcessBomFilter>, IViewModel
{
    private readonly IProcProcessBomService _bomService;
    private readonly IProcessService _processService;
    private readonly IMdItemService _itemService;

    public ProcessBomViewModel(IProcProcessBomService bomService, 
        IProcessService processService, 
        IMdItemService itemService)
    {
        _bomService = bomService;
        _processService = processService;
        _itemService = itemService;

        ProdAddCommand = new AsyncRelayCommand(AddMaterialAsync);
        ProdCopyCommand = new AsyncRelayCommand(CopyProductAsync);
        UpCommand = new RelayCommand<ProcProcessBomContentModel>(Up!);
        DownCommand = new RelayCommand<ProcProcessBomContentModel>(Down!);
        DelCommand = new RelayCommand<ProcProcessBomContentModel>(Del!);
        CopyCommand = new AsyncRelayCommand(CopyAsync);
    }

    public List<ProcProcess> ProcessDropdownList => _processService.GetAll();

    public List<MdItem> ProductDropdownList => _itemService.GetProducts();

    public List<MdItem> CriticalMaterialDropdownList => _itemService.GetCriticalMaterials();

    private long _addedMaterialId;
    public long AddedMaterialId
    {
        get => _addedMaterialId;
        set => SetProperty(ref _addedMaterialId, value);
    }

    private long _copyProductId;
    public long CopyProductId
    {
        get => _copyProductId;
        set => SetProperty(ref _copyProductId, value);
    }

    private long _sourceProductId;
    public long SourceProductId
    {
        get => _sourceProductId;
        set => SetProperty(ref _sourceProductId, value);
    }

    private long _targetProductId;
    public long TargetProductId
    {
        get => _targetProductId;
        set => SetProperty(ref _targetProductId, value);
    }

    public ICommand ProdAddCommand { get; }

    public ICommand ProdCopyCommand { get; }

    public ICommand UpCommand { get; }

    public ICommand DownCommand { get; }

    public ICommand DelCommand { get; }

    public ICommand CopyCommand { get; }

    protected override async Task<PagedList<ProcProcessBomModel>> OnSearchAsync(int pageIndex, int pageSize)
    {
        var pagedList = await _bomService.GetPagedListAsync(QueryFilter, pageIndex, pageSize);
        return pagedList.Adapt<PagedList<ProcProcessBomModel>>();
    }

    protected override async Task<(bool ok, string? err)> OnSaveAsync(ProcProcessBomModel data)
    {
        var bom = data.Adapt<ProcProcessBom>();
        return await _bomService.InsertOrUpdateAsync(bom);
    }

    protected override async Task<(bool ok, string? err)> OnDeleteAsync(ProcProcessBomModel data)
    {
        return await _bomService.DeleteAsync(data.Id);
    }

    private async Task AddMaterialAsync()
    {
        if (SelectedItem!.ProductId == 0)
        {
            NoticeWarning("请先选择 [产品]");
            return;
        }

        if (AddedMaterialId == 0)
        {
            NoticeWarning("请先选择 [物料]");
            return;
        }

        var item = await _itemService.GetAsync(AddedMaterialId);
        var item0 = item.Adapt<MdItemModel>();

        var bom = SelectedItem;
        bom.Contents ??= new();
        bom.Contents.Add(new()
        {
            MaterialId = AddedMaterialId,
            Material = item0,
            Qty = 1,
            Seq = bom.Contents.Count + 1,
        });
    }

    private async Task CopyProductAsync()
    {
        if (SelectedItem!.ProductId == 0)
        {
            NoticeWarning("请先选择 [产品]");
            return;
        }

        if (CopyProductId == 0)
        {
            NoticeWarning("请先选择要复制的产品");
            return;
        }

        var bom = await _bomService.GetBomAsync(CopyProductId, SelectedItem.ProcessId);
        if (bom == null)
        {
            NoticeWarning("复制的产品该工序中没有工艺 BOM");
            return;
        }

        var bom0 = bom.Adapt<ProcProcessBomModel>();
        SelectedItem!.Contents = bom0.Contents;
    }

    private void Up(ProcProcessBomContentModel item)
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

    private void Down(ProcProcessBomContentModel item)
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

    private void Del(ProcProcessBomContentModel item)
    {
        bool isLast = SelectedItem!.Contents!.Max(s => s.Seq) == item.Seq;

        SelectedItem!.Contents!.Remove(item);
        if (!isLast)
        {
            SelectedItem!.Contents.Where(s => s.Seq > item.Seq).ToList().ForEach(s => s.Seq -= 1);
        }
    }

    private async Task CopyAsync()
    {
        if (SourceProductId == 0)
        {
            NoticeWarning("请选择要复制的源产品");
            return;
        }

        if (TargetProductId == 0)
        {
            NoticeWarning("请选择要复制到的目标产品");
            return;
        }

        if (TargetProductId == SourceProductId)
        {
            NoticeWarning("要复制的源产品与目标产品不能相同");
            return;
        }

        var (ok, err) = await _bomService.CopyAsync(SourceProductId, TargetProductId);
        if (!ok)
        {
            NoticeWarning($"复制源产品BOM到新产品失败，原因：{err}");
            return;
        }

        NoticeInfo("已复制成功，请刷新页面查看。");
        CloseSidebar();
    }
}
