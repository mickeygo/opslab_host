namespace Ops.Host.Core.Services.Impl;

internal sealed class ProdScheduleService : IProdScheduleService
{
    private readonly SqlSugarRepository<ProdWo> _woRep;
    private readonly SqlSugarRepository<ProdSchedule> _scheduleRep;

    public ProdScheduleService(SqlSugarRepository<ProdWo> woRep, SqlSugarRepository<ProdSchedule> scheduleRep)
	{
        _woRep = woRep;
        _scheduleRep = scheduleRep;
    }

    public List<ProdWo> GetAllIssue()
    {
        return _woRep.GetList(s => s.Status == WoStatusEnum.Issued);
    }

    public Task<List<ProdWo>> GetAllIssueAsync()
    {
        return _woRep.GetListAsync(s => s.Status == WoStatusEnum.Issued);
    }

    public List<ProdSchedule> GetAllSchedule()
    {
        return _scheduleRep.AsQueryable()
            .Includes(s => s.WorkOrder, it => it!.Product)
            .Where(s => s.WorkOrder!.Status >= WoStatusEnum.Issued && s.WorkOrder!.Status < WoStatusEnum.Completed)
            .OrderBy(s => s.Seq)
            .ToList();
    }

    public async Task<List<ProdSchedule>> GetAllScheduleAsync()
    {
        return await _scheduleRep.AsQueryable()
            .Includes(s => s.WorkOrder, it => it!.Product)
            .Where(s => s.WorkOrder!.Status >= WoStatusEnum.Issued && s.WorkOrder!.Status < WoStatusEnum.Completed)
            .OrderBy(s => s.Seq)
            .ToListAsync();
    }

    public async Task<ProdSchedule?> GetToDoWoAsync()
    {
        // 1.工单中状态处于生产中或已排产
        // 2.上线数量 < 工单数量
        // 3.按排程顺序（正序），取第一条
        var schedule = await _scheduleRep.AsQueryable()
            .Includes(s => (s.WorkOrder!.Status == WoStatusEnum.Scheduled || s.WorkOrder.Status == WoStatusEnum.Producing) 
                        && s.WorkOrder.OnlineQty < s.WorkOrder.Qty)
            .OrderBy(s => s.Seq, OrderByType.Asc)
            .FirstAsync();

        return schedule;
    }

    public async Task<(bool ok, ProdSchedule? schedule, string? err)> ScheduleAsync(ProdWo input)
    {
        var wo = await _woRep.GetByIdAsync(input.Id);
        if (wo == null)
        {
            return (false, default, "没有找到工单");
        }

        if (wo?.Status != WoStatusEnum.Issued)
        {
            return (false, default, "工单只有处于下发状态才能排产");
        }

        var ok = await _woRep.UpdateAsync(s => new()
        {
            LastStatus = s.Status,
            Status = WoStatusEnum.Scheduled,
        }, s => s.Id == input.Id);

        if (!ok)
        {
            return (false, default, "排产失败");
        }

        ProdSchedule schedule = new()
        {
            WoId = wo.Id,
            WO = wo.Code,
            Seq = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(), // 采用时间戳标记顺序号
        };
        await _scheduleRep.InsertAsync(schedule);

        var schedule0 = await _scheduleRep.AsQueryable()
            .Includes(s => s.WorkOrder, it => it!.Product)
            .FirstAsync(s => s.Id == schedule.Id);

        return (ok, schedule0, "");
    }

    public async Task<(bool ok, ProdWo? wo, string? err)> DisScheduleAsync(ProdSchedule input)
    {
        var wo = await _woRep.GetByIdAsync(input.WoId);
        if (wo == null)
        {
            return (false, default, "没有找到工单");
        }

        if (wo.Status != WoStatusEnum.Scheduled)
        {
            return (false, default, "工单只有处于排产状态才能反排产");
        }

        var ok = await _woRep.UpdateAsync(s => new()
        {
            LastStatus = s.Status,
            Status = WoStatusEnum.Issued,
        }, s => s.Id == input.WoId);

        if (!ok)
        {
            return (ok, default, "反排产失败");
        }

        await _scheduleRep.DeleteAsync(s => s.WoId == wo.Id);
        var wo1 = await _woRep.GetByIdAsync(input.WoId);
        return (true, wo1, "");
    }

    public async Task<(bool ok, string? err)> UpScheduleAsync(ProdSchedule current, ProdSchedule prev)
    {
        var currentWo = await _woRep.GetByIdAsync(current.WoId);
        if (currentWo == null)
        {
            return (false, "没有找到当前工单");
        }

        var prevWo = await _woRep.GetByIdAsync(prev.WoId);
        if (prevWo == null)
        {
            return (false, "没有找到上一工单");
        }

        if (currentWo.Status != WoStatusEnum.Scheduled)
        {
            return (false, "当前工单不处于排产状态，不能移动");
        }

        if (prevWo.Status != WoStatusEnum.Scheduled)
        {
            return (false, "上一工单不处于排产状态，不能上移");
        }

        await _scheduleRep.UpdateAsync(s => new ProdSchedule { Seq = prev.Seq }, s => s.Id == current.Id);
        await _scheduleRep.UpdateAsync(s => new ProdSchedule { Seq = current.Seq }, s => s.Id == prev.Id);

        return (true, "");
    }

    public async Task<(bool ok, string? err)> DownScheduleAsync(ProdSchedule current, ProdSchedule next)
    {
        var currentWo = await _woRep.GetByIdAsync(current.WoId);
        if (currentWo == null)
        {
            return (false, "没有找到当前工单");
        }

        var nextWo = await _woRep.GetByIdAsync(next.WoId);
        if (nextWo == null)
        {
            return (false, "没有找到下一工单");
        }

        if (currentWo.Status != WoStatusEnum.Scheduled)
        {
            return (false, "当前工单不处于排产状态，不能移动");
        }

        await _scheduleRep.UpdateAsync(s => new ProdSchedule { Seq = next.Seq }, s => s.Id == current.Id);
        await _scheduleRep.UpdateAsync(s => new ProdSchedule { Seq = current.Seq }, s => s.Id == next.Id);

        return (true, "");
    }
}
