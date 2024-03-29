﻿namespace Ops.Host.Core.Services.Impl;

internal sealed class ProcessService : IProcessService
{
	private readonly SqlSugarRepository<ProcProcess> _processRep;
    private readonly SqlSugarRepository<MdStation> _stationRep;

    public ProcessService(SqlSugarRepository<ProcProcess> processRep, SqlSugarRepository<MdStation> stationRep)
	{
		_processRep = processRep;
        _stationRep = stationRep;
    }

    public async Task<ProcProcess> GetByIdAsync(long id)
    {
        return await _processRep.GetByIdAsync(id);
    }

    public List<ProcProcess> GetAll()
    {
        return _processRep.GetList(s => !s.IsDelete && !s.IsDisable);
    }

    public async Task<List<ProcProcess>> GetAllAsyns()
    {
        return await _processRep.GetListAsync(s => !s.IsDelete && !s.IsDisable);
    }

    public async Task<PagedList<ProcProcess>> GetPagedListAsync(ProcProcessFilter filter, int pageIndex, int pageSize)
    {
        return await _processRep.AsQueryable()
                .WhereIF(!string.IsNullOrWhiteSpace(filter.Code), s => s.Code.Contains(filter.Code!))
                .WhereIF(!string.IsNullOrWhiteSpace(filter.Name), s => s.Name.Contains(filter.Name!))
                .ToPagedListAsync(pageIndex, pageSize);
    }

    public async Task<(bool ok, string err)> SyncStationToProcessAsync()
    {
        var stations = await _stationRep.GetListAsync();
        foreach (var station in stations)
        {
            // 工艺与工站对应。
            var process = await _processRep.GetFirstAsync(s => s.StationId == station.Id);
            if (process == null)
            {
                process = new()
                {
                    Code = station.StationCode,
                    Name = station.StationName,
                    StationId = station.Id,
                };
                await _processRep.InsertAsync(process);
            }
            else
            {
                process.Code = station.StationCode; // TODO: 可考虑 '{产线编码}_{工站编码}' 格式
                process.Name = station.StationName;
                await _processRep.UpdateAsync(process);
            }
        }

        return (true, "");
    }
}
