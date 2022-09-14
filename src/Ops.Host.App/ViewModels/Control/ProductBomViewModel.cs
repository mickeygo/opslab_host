namespace Ops.Host.App.ViewModels;

public sealed class ProductBomViewModel : AsyncSinglePagedViewModelBase<MdProductBomModel, ProductBomFilter>, IViewModel
{
    private readonly IMdProductBomService _bomService;
    private readonly IMdItemService _itemService;
    private readonly StationCacheManager _stationManager;

    public ProductBomViewModel(IMdProductBomService bomService, IMdItemService itemService, StationCacheManager stationManager)
    {
        _bomService = bomService;
        _itemService = itemService;
        _stationManager = stationManager;

        ProdAddCommand = new AsyncRelayCommand(AddMaterialAsync);
        ProdCopyCommand = new AsyncRelayCommand(CopyProductAsync);
        UpCommand = new RelayCommand<MdProductBomItemModel>(Up!);
        DownCommand = new RelayCommand<MdProductBomItemModel>(Down!);
        DelCommand = new RelayCommand<MdProductBomItemModel>(Del!);
    }

    public List<NameValue> LineDropdownList => _stationManager.Lines;

    public List<NameValue> StationDropdownList => _stationManager.Stations;

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

    public ICommand ProdAddCommand { get; }

    public ICommand ProdCopyCommand { get; }

    public ICommand UpCommand { get; }

    public ICommand DownCommand { get; }

    public ICommand DelCommand { get; }

    protected override async Task<PagedList<MdProductBomModel>> OnSearchAsync(int pageIndex, int pageSize)
    {
        var pagedList = await _bomService.GetPagedListAsync(QueryFilter, pageIndex, pageSize);
        return pagedList.Adapt<PagedList<MdProductBomModel>>();
    }

    protected override async Task<(bool ok, string? err)> OnSaveAsync(MdProductBomModel data)
    {
        var bom = data.Adapt<MdProductBom>();
        var (ok, err) = await _bomService.InsertOrUpdateAsync(bom);
        if (ok)
        {
            // 此处要将实体映射到 model。
            var bom1 = await _bomService.GetBomByIdAsync(bom.Id);
            SelectedItem = bom1.Adapt<MdProductBomModel>();
        }

        return (ok, err);
    }

    protected override async Task<(bool ok, string? err)> OnDeleteAsync(MdProductBomModel data)
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
        bom.Items ??= new();
        bom.Items.Add(new()
        {
            MaterialId = AddedMaterialId,
            Material = item0,
            Qty = 1,
            Seq = bom.Items.Count + 1,
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

        var bom = await _bomService.GetBomByProductIdAsync(CopyProductId);
        var bom0 = bom.Adapt<MdProductBomModel>();
        SelectedItem!.Items = bom0.Items;
    }

    private void Up(MdProductBomItemModel item)
    {
        var index = SelectedItem!.Items!.IndexOf(item);
        if (index == 0)
        {
            return;
        }

        SelectedItem!.Items!.RemoveAt(index);
        SelectedItem!.Items!.Insert(index - 1, item);

        var item0 = SelectedItem!.Items!.First(s => s.Seq == item.Seq - 1);
        item0.Seq += 1;
        item.Seq -= 1;
    }

    private void Down(MdProductBomItemModel item)
    {
        var index = SelectedItem!.Items!.IndexOf(item);
        if (index == SelectedItem!.Items!.Count - 1)
        {
            return;
        }

        SelectedItem!.Items!.RemoveAt(index);
        SelectedItem!.Items!.Insert(index + 1, item);

        var item0 = SelectedItem!.Items!.First(s => s.Seq == item.Seq + 1);
        item0.Seq -= 1;
        item.Seq += 1;
    }

    private void Del(MdProductBomItemModel item)
    {
        bool isLast = SelectedItem!.Items!.Max(s => s.Seq) == item.Seq;

        SelectedItem!.Items!.Remove(item);
        if (!isLast)
        {
            SelectedItem!.Items.Where(s => s.Seq > item.Seq).ToList().ForEach(s => s.Seq -= 1);
        }
    }
}
