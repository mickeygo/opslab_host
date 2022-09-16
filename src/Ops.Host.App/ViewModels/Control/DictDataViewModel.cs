namespace Ops.Host.App.ViewModels;

public sealed class DictDataViewModel : AsyncSinglePagedViewModelBase<SysDictData, SysDictDataFilter>, IViewModel
{
    private readonly ISysDictDataService _dictService;

    public DictDataViewModel(ISysDictDataService dictService)
    {
        _dictService = dictService;
    }

    public List<NameValue> QueryDictCodeDropdownList => EnumExtensions.ToDropdownList<DictCodeEnum>();

    public List<NameValue> DictCodeDropdownList => EnumExtensions.ToDropdownList<DictCodeEnum>(false);

    protected override async Task<PagedList<SysDictData>> OnSearchAsync(int pageIndex, int pageSize)
    {
        return await _dictService.GetPagedListAsync(QueryFilter, pageIndex, pageSize);
    }

    protected override void OnBeforeSave(SysDictData data)
    {
        base.OnBeforeSave(data);
        data.CodeDesc ??= EnumExtensions.GetDesc<DictCodeEnum>(data.Code); // 补上编码描述
    }

    protected override async Task<(bool ok, string? err)> OnSaveAsync(SysDictData data)
    {
        return await _dictService.InsertOrUpdateDictAsync(data);
    }

    protected override async Task<(bool ok, string? err)> OnDeleteAsync(SysDictData data)
    {
        var ok = await _dictService.DeleteDictAsync(data.Id);
        return (ok, "");
    }
}
