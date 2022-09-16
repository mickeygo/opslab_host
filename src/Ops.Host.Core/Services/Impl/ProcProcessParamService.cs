namespace Ops.Host.Core.Services.Impl;

internal sealed class ProcProcessParamService : IProcProcessParamService
{
    private readonly SqlSugarRepository<ProcProcessParam> _paramRep;
    private readonly SqlSugarRepository<ProcProcess> _processRep;
    private readonly SqlSugarRepository<MdItem> _itemRep;
    private readonly StationCacheManager _stationCacheManager;

    public ProcProcessParamService(SqlSugarRepository<ProcProcessParam> paramRep,
        SqlSugarRepository<ProcProcess> processRep,
        SqlSugarRepository<MdItem> itemRep,
        StationCacheManager stationCacheManager)
    {
        _paramRep = paramRep;
        _processRep = processRep;
        _itemRep = itemRep;
        _stationCacheManager = stationCacheManager;
    }

    public async Task<PagedList<ProcProcessParam>> GetPagedListAsync(ProcessParamFilter filter, int pageIndex, int pageSize)
    {
        return await _paramRep.AsQueryable()
                .Includes(s => s.Product)
                .Includes(s => s.Process)
                .Includes(s => s.Contents)
                .WhereIF(!string.IsNullOrWhiteSpace(filter.ProductCode), s => s.Product!.Code.Contains(filter.ProductCode!))
                .WhereIF(!string.IsNullOrWhiteSpace(filter.ProductName), s => s.Product!.Name.Contains(filter.ProductName!))
                .WhereIF(!string.IsNullOrWhiteSpace(filter.ProcessCode), s => s.Process!.Code.Contains(filter.ProcessCode!))
                .ToPagedListAsync(pageIndex, pageSize);
    }

    public async Task<(bool ok, string err)> InsertOrUpdateAsync(ProcProcessParam input)
    {
        // 新增数据，检查产品 BOM 是否已存在（同一产品在同一工序下不能存在多个产品 BOM）。
        if (input.IsTransient() && (await _paramRep.IsAnyAsync(s => s.ProductId == input.ProductId && s.ProcessId == input.ProcessId)))
        {
            return (false, $"工艺参数在该产品、工序中已存在");
        }

        if (input.IsTransient())
        {
            var ok = await _paramRep.AsSugarClient().InsertNav(input).Include(s => s.Contents).ExecuteCommandAsync();
            return (ok, "");
        }

        var ok2 = await _paramRep.AsSugarClient().UpdateNav(input).Include(s => s.Contents).ExecuteCommandAsync();
        return (ok2, "");
    }

    public async Task<(bool ok, ProcProcessParam? content, string? err)> GenerateTemplateAsync(long productId, long processId)
    {
        if (await _paramRep.IsAnyAsync(s => s.ProductId == productId && s.ProcessId == processId))
        {
            return (false, default, "工艺参数已存在，请先删除后再重新生成");
        }

        ProcProcessParam processParam = new()
        {
            ProductId = productId,
            ProcessId = processId,
            Contents = new(),
        };

        var process = await _processRep.GetByIdAsync(processId);
        if (process == null)
        {
            return (false, default, "找不到对应的工序");
        }

        var product = await _itemRep.GetByIdAsync(productId);
        if (product == null)
        {
            return (false, default, "找不到对应的产品");
        }

        var station = _stationCacheManager.GetById(process.StationId);
        if (station == null)
        {
            return (false, default, "找不到对应的工站");
        }

        processParam.Process = process;
        processParam.Product = product;

        var archiveVar = _stationCacheManager.GetDeviceVariable(station.LineCode, station.StationCode, PlcSymbolTag.PLC_Sign_Archive);
        if (archiveVar is null)
        {
            return (false, default, $"工序没有设置 '{PlcSymbolTag.PLC_Sign_Archive}' 变量");
        }
        if (!archiveVar.NormalVariables.Any(s => s.IsAdditional))
        {
            return (false, default, $"工序变量中没有附加的过程数据");
        }

        foreach (var variable in archiveVar.NormalVariables.Where(s => s.IsAdditional))
        {
            // 对于数组，每一项都有工艺参数
            int num = variable.IsArray() ? variable.Length : 1;
            for (int i = 1; i <= num; i++)
            {
                ProcProcessParamContent parameter = new()
                {
                    Tag = variable.Tag,
                    Name = variable.Name,
                    DataType = variable.VarType,
                    Seq = i, // 基于1开始
                    IsCheck = false,
                };
                processParam.Contents.Add(parameter);
            }
        }

        return (true, processParam, "");
    }
}
