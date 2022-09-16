namespace Ops.Host.App.ViewModels;

public sealed class ProcessParamViewModel : AsyncSinglePagedViewModelBase<ProcProcessParamModel, ProcessParamFilter>, IViewModel
{
    private readonly IProcProcessParamService _paramService;
    private readonly IProcessService _processService;
    private readonly IMdItemService _itemService;

    public ProcessParamViewModel(IProcProcessParamService paramService, 
        IProcessService processService, 
        IMdItemService itemService)
    {
        _paramService = paramService;
        _processService = processService;
        _itemService = itemService;

        GenerateTemplateCommand = new AsyncRelayCommand(GenerateTemplateAsync);
    }

    public List<ProcProcess> ProcessDropdownList => _processService.GetAll();

    public List<MdItem> ProductDropdownList => _itemService.GetProducts();

    public ICommand GenerateTemplateCommand { get; }

    protected override async Task<PagedList<ProcProcessParamModel>> OnSearchAsync(int pageIndex, int pageSize)
    {
        var items = await _paramService.GetPagedListAsync(QueryFilter, pageIndex, pageSize);
        return items.Adapt<PagedList<ProcProcessParamModel>>();
    }

    protected override async Task<(bool ok, string? err)> OnSaveAsync(ProcProcessParamModel data)
    {
        var input = data.Adapt<ProcProcessParam>();
        return await _paramService.InsertOrUpdateAsync(input);
    }

    private async Task GenerateTemplateAsync()
    {
        if (SelectedItem!.ProductId == 0)
        {
            NoticeWarning("请先选择 [产品]");
            return;
        }

        if (SelectedItem!.ProcessId == 0)
        {
            NoticeWarning("请先选择 [工序]");
            return;
        }

        var (ok, content, err) = await _paramService.GenerateTemplateAsync(SelectedItem!.ProductId, SelectedItem!.ProcessId);
        if (!ok)
        {
            NoticeWarning($"生成模板出错，原因：{err}");
            return;
        }

        SelectedItem.Product = content!.Product;
        SelectedItem.Process = content!.Process;
        SelectedItem.Contents = new(content!.Contents!);
    }
}
