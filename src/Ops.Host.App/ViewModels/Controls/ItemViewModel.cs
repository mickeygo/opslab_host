namespace Ops.Host.App.ViewModels;

public sealed class ItemViewModel : AsyncSinglePagedViewModelBase<MdItem, MdItemFilter>, IViewModel
{
    private readonly IMdItemService _itemService;

    public ItemViewModel(IMdItemService itemService)
    {
        _itemService = itemService;
    }

    public List<NameValue> QueryAttrDropdownList { get; set; } = EnumExtensions.ToDropdownList<MaterialAttrEnum>();

    public List<NameValue> AttrDropdownList { get; set; } = EnumExtensions.ToDropdownList<MaterialAttrEnum>(false);

    protected override Task<PagedList<MdItem>> OnSearchAsync(int pageIndex, int pageSize)
    {
        return _itemService.GetPagedListAsync(QueryFilter, pageIndex, pageSize);
    }

    protected override async Task<(bool ok, string? err)> SaveAsync(MdItem data)
    {
        if (string.IsNullOrWhiteSpace(data.Code))
        {
            return (false, "物料代码s不能为空");
        }
        if (string.IsNullOrWhiteSpace(data.Name))
        {
            return (false, "物料名称不能为空");
        }
        if (string.IsNullOrWhiteSpace(data.BarcodeRule))
        {
            return (false, "条码规则不能为空");
        }

        data.Code = data.Code.Trim();
        data.Name = data.Name.Trim();
        data.Spec = data.Spec?.Trim();
        data.BarcodeRule = data.BarcodeRule?.Trim();

        var ok = await _itemService.InsertOrUpdateAsync(data);
        return (ok, "");
    }

    protected override async Task<(bool ok, string? err)> DeleteAsync(MdItem data)
    {
        var ok = await _itemService.DeleteAsync(data.Id);
        return (ok, "");
    }

    protected override void OnExcelCreating(ExcelModelBuilder builder)
    {
        builder.ExcelName = "物料主数据";
        builder.SheetName = "物料";
    }
}
