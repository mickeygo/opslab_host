namespace Ops.Host.Core.Services.Impl;

internal sealed class MaterialTraceService : IMaterialTraceService
{
    private readonly SqlSugarRepository<PtSnMaterial> _materialRep;
    private readonly SqlSugarRepository<PtTrackMaterial> _traceRep;

    public MaterialTraceService(SqlSugarRepository<PtSnMaterial> materialRep, 
        SqlSugarRepository<PtTrackMaterial> traceRep)
	{
        _materialRep = materialRep;
        _traceRep = traceRep;
    }

    public async Task<PagedList<PtSnMaterial>> GetPagedListAsync(MaterialTraceFilter filter, int pageIndex, int pageSize)
    {
        return await _materialRep.AsQueryable()
            .WhereIF(!string.IsNullOrWhiteSpace(filter.SN), s => s.SN.Contains(filter.SN!))
            .WhereIF(!string.IsNullOrWhiteSpace(filter.Barcode), s => s.Barcode.Contains(filter.Barcode!))
            .ToPagedListAsync(pageIndex, pageSize);
    }

    public async Task Unbind(PtSnMaterial input)
    {
        // 标记物料已解绑
        input.BindingStatus = BindingEnum.Unbind;
        await _materialRep.AsUpdateable(input).UpdateColumns(s => new { s.BindingStatus, s.UpdateTime }).ExecuteCommandAsync();

        // 删除关键物料
        await _traceRep.DeleteAsync(s => s.SN == input.SN && s.Barcode == input.Barcode);
    }
}
