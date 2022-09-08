namespace Ops.Host.App.ViewModels;

public sealed class ItemViewModel : AsyncSinglePagedViewModelBase<MdItem, MdItemFilter>, IViewModel
{
    private readonly IMdItemService _itemService;

    public ItemViewModel(IMdItemService itemService)
    {
        _itemService = itemService;
    }

    public List<NameValue> QueryAttrDropdownList => EnumExtensions.ToDropdownList<MaterialAttrEnum>();

    public List<NameValue> AttrDropdownList => EnumExtensions.ToDropdownList<MaterialAttrEnum>(false);

    protected override Task<PagedList<MdItem>> OnSearchAsync(int pageIndex, int pageSize)
    {
        return _itemService.GetPagedListAsync(QueryFilter, pageIndex, pageSize);
    }

    protected override async Task<(bool ok, string? err)> OnSaveAsync(MdItem data)
    {
        return await _itemService.InsertOrUpdateAsync(data);
    }

    protected override async Task<(bool ok, string? err)> OnDeleteAsync(MdItem data)
    {
        var ok = await _itemService.DeleteAsync(data.Id);
        return (ok, "");
    }

    protected override void OnExcelModelCreating(ExcelModelBuilder builder)
    {
        builder.ExcelName = "物料主数据";
        builder.SheetName = "物料";
    }
}
