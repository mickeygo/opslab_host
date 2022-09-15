namespace Ops.Host.App.ViewModels;

public sealed class ProcessViewModel : AsyncSinglePagedViewModelBase<ProcProcess, ProcProcessFilter>, IViewModel
{
    private readonly IProcessService _processService;

    public ProcessViewModel(IProcessService processService)
    {
        _processService = processService;

        SyncCommand = new AsyncRelayCommand(SyncAsync);
    }

    public ICommand SyncCommand { get; }

    protected override async Task<PagedList<ProcProcess>> OnSearchAsync(int pageIndex, int pageSize)
    {
        return await _processService.GetPagedListAsync(QueryFilter, pageIndex, pageSize);
    }

    public async Task SyncAsync()
    {
        var (ok, err) = await _processService.SyncStationToProcessAsync();
        if (!ok)
        {
            NoticeWarning($"同步失败，原因：{err}");
            return;
        }

        NoticeInfo("数据同步成功");
    }
}
