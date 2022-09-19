namespace Ops.Host.Core.Services.Impl;

internal sealed class ProdWoService : IProdWoService
{
    private readonly SqlSugarRepository<ProdWo> _woRep;

    public ProdWoService(SqlSugarRepository<ProdWo> woRep)
    {
        _woRep = woRep;
    }

    public async Task<ProdWo> GetWoAsync(long woId)
    {
        return await _woRep.GetByIdAsync(woId);
    }

    public async Task<PagedList<ProdWo>> GetPagedListAsync(ProdWoFilter filter, int pageIndex, int pageSize)
    {
        return await _woRep.AsQueryable()
                .Includes(s => s.Product)
                .WhereIF(!string.IsNullOrWhiteSpace(filter.WoCode), s => s.Code.Contains(filter.WoCode!))
                .WhereIF(!string.IsNullOrWhiteSpace(filter.WoName), s => s.Name.Contains(filter.WoName!))
                .WhereIF(filter.ProductId > 0, s => s.ProductId == filter.ProductId)
                .OrderBy(s => s.CreateTime, OrderByType.Desc)
                .ToPagedListAsync(pageIndex, pageSize);
    }

    public async Task<(bool ok, string err)> InsertOrUpdateAsync(ProdWo input)
    {
        // 新增数据，检查用户是否已存在
        if (input.IsTransient() && _woRep.IsAny(s => s.Code == input.Code))
        {
            return (false, $"工单编码 '{input.Code}' 已存在");
        }

        // 修改
        // 非创建状态，不能再更改工单物料相关数据
        if (input.Status != WoStatusEnum.Created)
        {
            return (false, $"工单已 {input.Status.Desc()}，不能进行更改");
        }

        var ok = await _woRep.InsertOrUpdateAsync(input);
        return (ok, "");
    }

    public async Task<(bool ok, string err)> IssueAsync(long woId)
    {
        var wo = await _woRep.GetByIdAsync(woId);
        if (wo?.Status != WoStatusEnum.Created)
        {
            return (false, "工单只有创建状态才能下发");
        }

        var ok = await _woRep.UpdateAsync(s => new()
        {
            LastStatus = s.Status,
            Status = WoStatusEnum.Issued,
        }, s => s.Id == woId);

        return (ok, "");
    }

    public async Task<(bool ok, string err)> PauseAsync(long woId)
    {
        var wo = await _woRep.GetByIdAsync(woId);
        if (wo?.Status != WoStatusEnum.Producing)
        {
            return (false, "工单只有处于生产状态才能暂停");
        }

        var ok = await _woRep.UpdateAsync(s => new()
        {
            LastStatus = s.Status,
            Status = WoStatusEnum.Paused,
        }, s => s.Id == woId);

        return (ok, "");
    }

    public async Task<(bool ok, string err)> ResumeAsync(long woId)
    {
        var wo = await _woRep.GetByIdAsync(woId);
        if (wo?.Status != WoStatusEnum.Paused)
        {
            return (false, "工单只有处于暂停状态才能恢复");
        }

        var ok = await _woRep.UpdateAsync(s => new()
        {
            LastStatus = s.Status,
            Status = WoStatusEnum.Producing,
        }, s => s.Id == woId);

        return (ok, "");
    }

    public async Task<(bool ok, string err)> DeleteAsync(long woId)
    {
        var wo = await _woRep.GetByIdAsync(woId);
        if (wo?.Status > WoStatusEnum.Created)
        {
            return (false, "工单只有处于创建状态才能删除");
        }

        var ok = await _woRep.DeleteByIdAsync(woId);
        return (ok, "");
    }
}
