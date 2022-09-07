namespace Ops.Host.App.ViewModels;

public sealed class ArchiveViewModel : AsyncSinglePagedViewModelBase<PtArchive, PtArchiveFilter>, IViewModel
{
    private readonly IPtArchiveService _archiveService;

    public ArchiveViewModel(IPtArchiveService archiveService)
    {
        _archiveService = archiveService;
    }

    protected override async Task<PagedList<PtArchive>> OnSearchAsync(int pageIndex, int pageSize)
    {
        return await _archiveService.GetPagedListAsync(QueryFilter, pageIndex, pageSize);
    }
}
