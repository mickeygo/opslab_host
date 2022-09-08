namespace Ops.Host.App.ViewModels;

public sealed class CardViewModel : AsyncSinglePagedViewModelBase<SysCard, SysCardFilter>, IViewModel
{
    private readonly ISysCardService _cardService;

    public CardViewModel(ISysCardService cardService)
    {
        _cardService = cardService;
    }

    public List<NameValue> QueryLevelDropdownList => EnumExtensions.ToDropdownList<CardLevelEnum>();

    public List<NameValue> LevelDropdownList => EnumExtensions.ToDropdownList<CardLevelEnum>(false);

    protected override async Task<PagedList<SysCard>> OnSearchAsync(int pageIndex, int pageSize)
    {
        return await _cardService.GetPagedListAsync(QueryFilter, pageIndex, pageSize);
    }

    protected override async Task<(bool ok, string? err)> OnSaveAsync(SysCard data)
    {
        return await _cardService.InsertOrUpdateAsync(data);
    }

    protected override async Task<(bool ok, string? err)> OnDeleteAsync(SysCard data)
    {
        var ok = await _cardService.DeleteAsync(data.Id);
        return (ok, "");
    }
}
