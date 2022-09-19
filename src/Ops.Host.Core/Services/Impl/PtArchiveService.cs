namespace Ops.Host.Core.Services.Impl;

public sealed class PtArchiveService : IPtArchiveService
{
	private readonly SqlSugarRepository<PtArchive> _archiveRep;
	private readonly StationCacheManager _stationCacheManager;

    public PtArchiveService(SqlSugarRepository<PtArchive> archiveRep, StationCacheManager stationCacheManager)
	{
		_archiveRep = archiveRep;
		_stationCacheManager = stationCacheManager;
    }

	public async Task<PagedList<PtArchive>> GetPagedListAsync(PtArchiveFilter filter, int pageIndex, int pageSize)
	{
		return await _archiveRep.AsQueryable().Includes(t => t.ArchiveItems, item => item.ArchiveItemLines)
				.WhereIF(!string.IsNullOrWhiteSpace(filter.LineCode), s => s.LineCode == filter.LineCode)
                .WhereIF(!string.IsNullOrWhiteSpace(filter.StationCode), s => s.StationCode == filter.StationCode)
                .WhereIF(!string.IsNullOrWhiteSpace(filter.SN), s => s.SN.Contains(filter.SN!))
                .WhereIF(filter.CreateTimeStart != null, s => s.CreateTime >= filter.CreateTimeStart.ToDayMin())
                .WhereIF(filter.CreateTimeEnd != null, s => s.CreateTime <= filter.CreateTimeEnd.ToDayMax())
                .ToPagedListAsync(pageIndex, pageSize);
    }

	public async Task<DataTable> GetDataTable(PtArchiveFilter filter, bool extendArray = false, bool showLimit = false)
	{
		var archives = await _archiveRep.AsQueryable().Includes(t => t.ArchiveItems, item => item.ArchiveItemLines)
            .WhereIF(!string.IsNullOrWhiteSpace(filter.LineCode), s => s.LineCode == filter.LineCode)
            .WhereIF(!string.IsNullOrWhiteSpace(filter.StationCode), s => s.StationCode == filter.StationCode)
            .WhereIF(!string.IsNullOrWhiteSpace(filter.SN), s => s.SN.Contains(filter.SN!))
            .WhereIF(filter.CreateTimeStart != null, s => s.CreateTime >= filter.CreateTimeStart.ToDayMin())
            .WhereIF(filter.CreateTimeEnd != null, s => s.CreateTime <= filter.CreateTimeEnd.ToDayMax())
            .ToListAsync();

		// 内存中数据转换
		var dt = new DataTable();

        // 主数据列
		foreach (var pi in typeof(PtArchive).GetProperties())
		{
            var attr = pi.GetCustomAttribute<DisplayNameAttribute>(); // 只导出标记了 DisplayNameAttribute 特性的数据。
            if (attr != null)
			{
                // 枚举类型使用字符串类型；
                // 可空类型使用其具体返修参数。
                Type propType = pi.PropertyType;

                if (pi.PropertyType.IsEnum)
                {
                    propType = typeof(string);
                }
                else if (pi.PropertyType.IsGenericType)
                {
                    var propType0 = pi.PropertyType.GetGenericArguments()[0];
                    propType = propType0.IsEnum ? typeof(string) : propType0;
                }

                dt.Columns.Add(new DataColumn(attr.DisplayName, propType));
            }
        }

		var devVariable = _stationCacheManager.GetDeviceVariable(filter.LineCode, filter.StationCode, PlcSymbolTag.PLC_Sign_Archive);
		var additionalVars = devVariable?.NormalVariables.Where(s => s.IsAdditional);

        // 附加数据列
        if (additionalVars is not null)
        {
            foreach (var normalVar in additionalVars)
            {
                if (extendArray && normalVar.IsArray())
                {
                    for (int i = 1; i <= normalVar.Length; i++)
                    {
                        dt.Columns.Add(new DataColumn($"{normalVar.Name}-{i}", typeof(string)));
                    }

                    continue;
                }

                dt.Columns.Add(new DataColumn(normalVar.Name, typeof(string)));
            }
        }
        
        // 数据行
        foreach (var archive in archives)
		{
			var row = dt.NewRow();

            // 主数据
            foreach (var pi in typeof(PtArchive).GetProperties())
            {
                var attr = pi.GetCustomAttribute<DisplayNameAttribute>();
                if (attr != null)
                {
                    if (EnumExtensions.IsEnum(pi.PropertyType))
                    {
                        row[attr.DisplayName] = EnumExtensions.Desc((Enum)pi.GetValue(archive)!);
                    }
                    else
                    {
                        row[attr.DisplayName] = pi.GetValue(archive);
                    }
                }
            }

            // 附加数据，从变量中获取，防止地址后续变更
            if (additionalVars is not null)
            {
                foreach (var normalVar in additionalVars)
                {
                    var item = archive.ArchiveItems!.FirstOrDefault(s => s.Name == normalVar.Name);
                    if (item != null)
                    {
                        // 数组展开数据
                        if (extendArray && item.IsArray)
                        {
                            foreach (var itemLine in item.ArchiveItemLines!.OrderBy(s => s.Seq))
                            {
                                row[$"{item.Name}-{itemLine.Seq}"] = showLimit ? $"{itemLine.Value} ({itemLine.Lower}-{itemLine.Higher})" : itemLine.Value;
                            }

                            continue;
                        }

                        row[item.Name] = showLimit ? $"{item.Value} ({item.Lower}-{item.Higher})" : item.Value;
                    }
                }
            }

            dt.Rows.Add(row);
        }

        return dt;
	}
}
