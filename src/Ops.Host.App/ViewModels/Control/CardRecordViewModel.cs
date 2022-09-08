namespace Ops.Host.App.ViewModels;

public sealed class CardRecordViewModel : AsyncSinglePagedViewModelBase<SysCardRecord, SysCardRecordFilter>, IViewModel
{
    private readonly ISysCardRecordService _cardRecordService;

    public CardRecordViewModel(ISysCardRecordService cardRecordService)
    {
        _cardRecordService = cardRecordService;
    }

    protected override void OnExcelModelCreating(ExcelModelBuilder builder)
    {
        builder.ExcelName = "刷卡记录";
    }

    protected override async Task<PagedList<SysCardRecord>> OnSearchAsync(int pageIndex, int pageSize)
    {
        return await _cardRecordService.GetPagedListAsync(QueryFilter, pageIndex, pageSize);
    }
}
