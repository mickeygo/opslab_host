namespace Ops.Host.Core.Services.Impl;

internal sealed class SysCardService : ISysCardService
{
    private readonly SqlSugarRepository<SysCard> _cardRep;

    public SysCardService(SqlSugarRepository<SysCard> cardRep)
	{
        _cardRep = cardRep;
    }

    public async Task<SysCard> GetAsync(int id)
    {
        return await _cardRep.GetByIdAsync(id);
    }

    public async Task<PagedList<SysCard>> GetPagedListAsync(SysCardFilter filter, int pageIndex, int pageSize)
    {
        return await _cardRep.AsQueryable()
                .WhereIF(!string.IsNullOrWhiteSpace(filter.CardNo), s => s.CardNo!.Contains(filter.CardNo!))
                .WhereIF(!string.IsNullOrWhiteSpace(filter.Owner), s => s.Owner!.Contains(filter.Owner!))
                .WhereIF(filter.CardLevel != null, s => s.CardLevel == filter.CardLevel)
                .ToPagedListAsync(pageIndex, pageSize);
    }

    public async Task<(bool ok, string err)> InsertOrUpdateAsync(SysCard input)
    {
        // 新增数据，检查用户是否已存在
        if (input.IsTransient() && _cardRep.IsAny(s => s.CardNo == input.CardNo))
        {
            return (false, $"卡号 '{input.CardNo}' 已存在");
        }

        var ok = await _cardRep.InsertOrUpdateAsync(input);
        return (ok, "");
    }

    public async Task<bool> EnableAsync(long id)
    {
        return await _cardRep.UpdateAsync(s => new SysCard { Status = StatusEnum.Enable }, s => s.Id == id);
    }

    public async Task<bool> DisableAsync(long id)
    {
        return await _cardRep.UpdateAsync(s => new SysCard { Status = StatusEnum.Disable }, s => s.Id == id);
    }

    public async Task<bool> DeleteAsync(long id)
    {
        return await _cardRep.DeleteByIdAsync(id);
    }
}
