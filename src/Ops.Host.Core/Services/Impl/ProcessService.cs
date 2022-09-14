namespace Ops.Host.Core.Services.Impl;

internal sealed class ProcessService : IProcessService
{
	private readonly SqlSugarRepository<ProcProcess> _processRep;
    private readonly SqlSugarRepository<MdStation> _stationRep;

    public ProcessService(SqlSugarRepository<ProcProcess> processRep, SqlSugarRepository<MdStation> stationRep)
	{
		_processRep = processRep;
        _stationRep = stationRep;
    }

    public async Task<PagedList<ProcProcess>> GetPagedListAsync(ProcProcessFilter filter, int pageIndex, int pageSize)
    {
        return await _processRep.AsQueryable().Includes(t => t.Parameters)
                .WhereIF(!string.IsNullOrWhiteSpace(filter.Code), s => s.Code.Contains(filter.Code!))
                .WhereIF(!string.IsNullOrWhiteSpace(filter.Name), s => s.Name.Contains(filter.Name!))
                .ToPagedListAsync(pageIndex, pageSize);
    }

    public async Task<(bool ok, string err)> SyncStationToProcessAsync()
    {
        var stations = await _stationRep.GetListAsync();
        foreach (var station in stations)
        {
            var devInfo = station.DeviceInfoExt;
            var archiveVar = devInfo?.Variables.FirstOrDefault(s => s.Tag == PlcSymbolTag.PLC_Sign_Archive); // 仅 Archive 变量才设置工艺参数。

            var process = await _processRep.GetFirstAsync(s => s.Code == station.StationCode);
            if (process == null)
            {
                process = new()
                {
                    Code = station.StationCode,
                    Name = station.StationName,
                    Owner = station.Owner,
                    Type = station.Type,
                    Parameters = new(),
                };

                // 设置工艺参数占位符。
                if (archiveVar != null)
                {
                    foreach (var variable in archiveVar.NormalVariables.Where(s => s.IsAdditional))
                    {
                        // 对于数组，每一项都有工艺参数
                        int num = variable.IsArray() ? variable.Length : 1;
                        for (int i = 1; i <= num; i++)
                        {
                            ProcProcessParameter parameter = new()
                            {
                                Tag = variable.Tag,
                                Name = variable.Name,
                                DataType = variable.VarType,
                                Seq = i, // 基于1开始
                            };
                            process.Parameters.Add(parameter);
                        }
                    }
                }

                await _processRep.AsSugarClient()
                    .InsertNav(process)
                    .Include(s => s.Parameters)
                    .ExecuteCommandAsync();
            }
            else
            {
                process.Name = station.StationName;
                process.Owner = station.Owner;
                process.Type = station.Type;

                var oldParams = process.Parameters;
                process.Parameters = new();

                // 更新工艺参数占位符。
                if (archiveVar != null)
                {
                    foreach (var variable in archiveVar.NormalVariables.Where(s => s.IsAdditional))
                    {
                        // 对于数组，每一项都有工艺参数
                        int num = variable.IsArray() ? variable.Length : 1;
                        for (int i = 1; i <= num; i++)
                        {
                            var parameter0 = oldParams?.FirstOrDefault(s => s.Tag == variable.Tag && s.Seq == i);
                            ProcProcessParameter parameter = new()
                            {
                                Tag = variable.Tag,
                                Name = variable.Name,
                                DataType = variable.VarType,
                                Seq = i,
                                Higher = parameter0?.Higher,
                                Lower = parameter0?.Lower,
                            };
                            process.Parameters.Add(parameter);
                        }
                    }
                }

                await _processRep.AsSugarClient()
                    .UpdateNav(process)
                    .Include(s => s.Parameters)
                    .ExecuteCommandAsync();
            }
        }

        return (true, "");
    }

    public async Task<(bool ok, string err)> UpdateParameterAsync(ProcProcess input)
    {
        var ok = await _processRep.AsSugarClient()
               .UpdateNav(input)
               .Include(s => s.Parameters)
               .ExecuteCommandAsync();

        return (ok, "");
    }
}
