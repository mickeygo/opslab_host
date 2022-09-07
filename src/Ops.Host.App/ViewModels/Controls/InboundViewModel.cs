namespace Ops.Host.App.ViewModels;

public sealed class InboundViewModel : AsyncSinglePagedViewModelBase<PtInbound, PtInboundFilter>, IViewModel
{
    private readonly IPtInboundService _inboundService;

    public InboundViewModel(IPtInboundService inboundService)
    {
        _inboundService = inboundService;
    }

    protected override async Task<PagedList<PtInbound>> OnSearchAsync(int pageIndex, int pageSize)
    {
        return await _inboundService.GetPagedListAsync(QueryFilter, pageIndex, pageSize);
    }
}
