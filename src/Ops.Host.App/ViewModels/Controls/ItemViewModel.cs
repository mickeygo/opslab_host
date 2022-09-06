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

    protected override Task<bool> SaveAsync(MdItem data)
    {
        return _itemService.InsertOrUpdateAsync(data);
    }

    protected override Task<bool> DeleteAsync(MdItem data)
    {
        return _itemService.DeleteAsync(data.Id);
    }

    protected override void OnExcelCreating(ExcelModelBuilder builder)
    {
        builder.ExcelName = "物料主数据";
        builder.SheetName = "物料";
    }
}
