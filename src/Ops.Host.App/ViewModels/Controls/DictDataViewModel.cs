namespace Ops.Host.App.ViewModels;

public sealed class DictDataViewModel : AsyncSinglePagedViewModelBase<SysDictData, SysDictDataFilter>, IViewModel
{
    private readonly ISysDictDataService _dictService;

    public DictDataViewModel(ISysDictDataService dictService)
    {
        _dictService = dictService;
    }

    public List<NameValue> QueryDictCodeDropdownList { get; set; } = EnumExtensions.ToDropdownList<DictCodeEnum>();

    public List<NameValue> DictCodeDropdownList { get; set; } = EnumExtensions.ToDropdownList<DictCodeEnum>(false);

    protected override async Task<PagedList<SysDictData>> OnSearchAsync(int pageIndex, int pageSize)
    {
        return await _dictService.GetPagedListAsync(QueryFilter, pageIndex, pageSize);
    }

    protected override async Task<(bool ok, string? err)> SaveAsync(SysDictData data)
    {
        if (string.IsNullOrWhiteSpace(data.Code))
        {
            return (false, "字典代码不能为空");
        }
        if (string.IsNullOrWhiteSpace(data.Name))
        {
            return (false, "字典名称不能为空");
        }
        if (string.IsNullOrWhiteSpace(data.Value))
        {
            return (false, "字典值不能为空");
        }

        if (IsAdding)
        {
            data.CodeDesc ??= EnumExtensions.GetDesc<DictCodeEnum>(data.Code);
        }

        data.Name = data.Name.Trim();
        data.Value = data.Value.Trim();
        data.Remark = data.Remark?.Trim();

        var ok = await _dictService.InsertOrUpdateDictAsync(data);
        return (ok, "");
    }

    protected override async Task<(bool ok, string? err)> DeleteAsync(SysDictData data)
    {
        var ok = await _dictService.DeleteDictAsync(data.Id);
        return (ok, "");
    }
}
