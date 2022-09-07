namespace Ops.Host.Core.Services.Impl;

public sealed class PtArchiveService : IPtArchiveService
{
	private readonly SqlSugarRepository<PtArchive> _archiveRep;

	public PtArchiveService(SqlSugarRepository<PtArchive> archiveRep)
	{
		_archiveRep = archiveRep;
	}

	public async Task<PagedList<PtArchive>> GetPagedListAsync(PtArchiveFilter filter, int pageIndex, int pageSize)
	{
		return await _archiveRep.AsQueryable().Includes(t => t.ArchiveItems, item => item.ArchiveItemLines)
				.WhereIF(!string.IsNullOrWhiteSpace(filter.StationCode), s => s.StationCode == filter.StationCode)
                .WhereIF(!string.IsNullOrWhiteSpace(filter.SN), s => s.SN.Contains(filter.SN!))
                .WhereIF(filter.CreateTimeStart != null, s => s.CreateTime >= filter.CreateTimeStart.ToDayMin())
                .WhereIF(filter.CreateTimeEnd != null, s => s.CreateTime <= filter.CreateTimeEnd.ToDayMax())
                .ToPagedListAsync(pageIndex, pageSize);
    }

	public async Task<DataTable> GetDataTable(int pageIndex, int pageSize)
	{
		var archives = await _archiveRep.AsQueryable().Includes(t => t.ArchiveItems, item => item.ArchiveItemLines).ToListAsync();

		// 内存中数据转换
		var dt = new DataTable();
		foreach (var pi in typeof(PtArchive).GetProperties())
		{
            var descAttr = pi.GetCustomAttribute<DescriptionAttribute>();
			if (descAttr != null)
			{
                dt.Columns.Add(new DataColumn(descAttr.Description, pi.PropertyType));
            }
        }

        DeviceVariable devVariable = null;
		var normalVars = devVariable.NormalVariables;
		foreach (var normalVar in normalVars)
		{
            dt.Columns.Add(new DataColumn(normalVar.Name, typeof(string)));
		}

        foreach (var archive in archives)
		{
			var row = dt.NewRow();
		}

        return dt;
	}
}
