namespace Ops.Host.App.ViewModels;

internal sealed class WorkOrderViewModel : AsyncSinglePagedViewModelBase<ProdWo, ProdWoFilter>, IViewModel
{
    private readonly IProdWoService _woService;
    private readonly IMdItemService _itemService;

    public WorkOrderViewModel(IProdWoService woService, IMdItemService itemService)
    {
        _woService = woService;
        _itemService = itemService;

        IssueCommand = new AsyncRelayCommand<ProdWo>(IssueAsync!);
    }

    public List<MdItem> ProductDropdownList => _itemService.GetProducts();

    public List<MdItem> QueryProductDropdownList => DropdownListHelper.Make(ProductDropdownList);

    public List<NameValue> WoTypeDropdownList => EnumExtensions.ToNameValueList2<WoTypeEnum>();

    public ICommand IssueCommand { get; }

    protected override async Task<PagedList<ProdWo>> OnSearchAsync(int pageIndex, int pageSize)
    {
        return await _woService.GetPagedListAsync(QueryFilter, pageIndex, pageSize);
    }

    protected override async Task<(bool ok, string? err)> OnSaveAsync(ProdWo data)
    {
        return await _woService.InsertOrUpdateAsync(data);
    }

    private async Task IssueAsync(ProdWo input)
    {
        var (ok, err) = await _woService.IssueAsync(input.Id);
        if (!ok)
        {
            NoticeWarning(err);
            return;
        }

        NoticeInfo("下发成功");
    }
}
