using Ops.Host.Common;
using System.Windows.Markup;

namespace Ops.Host.App.ViewModels;

internal sealed class WorkOrderViewModel : AsyncSinglePagedViewModelBase<ProdWoModel, ProdWoFilter>, IViewModel
{
    private readonly IProdWoService _woService;
    private readonly IMdItemService _itemService;

    public WorkOrderViewModel(IProdWoService woService, IMdItemService itemService)
    {
        _woService = woService;
        _itemService = itemService;

        IssueCommand = new AsyncRelayCommand<ProdWoModel>(IssueAsync!);
    }

    public List<MdItem> ProductDropdownList => _itemService.GetProducts();

    public List<MdItem> QueryProductDropdownList => DropdownListHelper.Make(ProductDropdownList);

    public List<NameValue> WoTypeDropdownList => EnumExtensions.ToNameValueList2<WoTypeEnum>();

    public ICommand IssueCommand { get; }

    protected override async Task<PagedList<ProdWoModel>> OnSearchAsync(int pageIndex, int pageSize)
    {
        var items = await _woService.GetPagedListAsync(QueryFilter, pageIndex, pageSize);
        return items.Adapt<PagedList<ProdWoModel>>();
    }

    protected override async Task<(bool ok, string? err)> OnSaveAsync(ProdWoModel data)
    {
        if (data.Product == null)
        {
            return (false, "必须选择 [产品]");
        }

        data.ProductId = data.Product.Id;
        var data0 = data.Adapt<ProdWo>();
        return await _woService.InsertOrUpdateAsync(data0);
    }

    protected override async Task<(bool ok, string? err)> OnDeleteAsync(ProdWoModel data)
    {
        return await _woService.DeleteAsync(data.Id);
    }

    private async Task IssueAsync(ProdWoModel input)
    {
        var (ok, err) = await _woService.IssueAsync(input.Id);
        if (!ok)
        {
            NoticeWarning(err);
            return;
        }

        input.Status = WoStatusEnum.Issued;
        NoticeInfo("下发成功");
    }
}
