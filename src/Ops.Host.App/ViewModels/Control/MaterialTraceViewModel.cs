using Mapster;

namespace Ops.Host.App.ViewModels;

public sealed class MaterialTraceViewModel : AsyncSinglePagedViewModelBase<PtSnMaterialModel, MaterialTraceFilter>, IViewModel
{
    private readonly IMaterialTraceService _materialTraceService;

    public MaterialTraceViewModel(IMaterialTraceService materialTraceService)
    {
        _materialTraceService = materialTraceService;

        UnbindCommand = new AsyncRelayCommand<PtSnMaterialModel>(Unbind!);
    }

    public ICommand UnbindCommand { get; }

    protected override async Task<PagedList<PtSnMaterialModel>> OnSearchAsync(int pageIndex, int pageSize)
    {
        var pagedList = await _materialTraceService.GetPagedListAsync(QueryFilter, pageIndex, pageSize);
        return pagedList.Adapt<PagedList<PtSnMaterialModel>>();
    }

    private async Task Unbind(PtSnMaterialModel item)
    {
        var input = item.Adapt<PtSnMaterial>();
        await _materialTraceService.Unbind(input);
        item.BindingStatus = BindingEnum.Unbind;
    }
}

// Observable 模型映射，可通过后端更新 UI。
public class PtSnMaterialModel : ObservableObject
{
    public long Id { get; set; }

    /// <summary>
    /// SN
    /// </summary>
    [NotNull]
    public string? SN { get; set; }

    /// <summary>
    /// Barcode
    /// </summary>
    [NotNull]
    public string? Barcode { get; set; }

    /// <summary>
    /// 产线编码
    /// </summary>
    [NotNull]
    public string? LineCode { get; set; }

    /// <summary>
    /// 工站编码
    /// </summary>
    [NotNull]
    public string? StationCode { get; set; }

    private MaterialAttrEnum _attr;

    /// <summary>
    /// 物料属性。
    /// </summary>
    public MaterialAttrEnum Attr
    {
        get => _attr;
        set => SetProperty(ref _attr, value);
    }

    public BindingEnum _bindingStatus;

    /// <summary>
    /// 物料绑定状态
    /// </summary>
    public BindingEnum BindingStatus
    {
        get => _bindingStatus;
        set => SetProperty(ref _bindingStatus, value);
    }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 更新时间
    /// </summary>
    public DateTime? UpdateTime { get; set; }
}
