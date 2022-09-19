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
}
