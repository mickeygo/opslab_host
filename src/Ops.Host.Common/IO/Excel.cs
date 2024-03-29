﻿namespace Ops.Host.Common.IO;

/// <summary>
/// Excel 导出设置。
/// </summary>
public sealed class ExcelSettings
{
    /// <summary>
    /// 全局设置的时间显示格式，默认为 "yyyy-MM-dd HH:mm:ss";
    /// </summary>
    public string DateTimeFormat { get; set; } = "yyyy-MM-dd HH:mm:ss";

    /// <summary>
    /// 小数类型显示格式，默认为 "0.00"。
    /// </summary>
    public string RealFormat { get; set; } = "0.00";

    /// <summary>
    /// 单元格是否有边框, 默认为 true。
    /// </summary>
    public bool IstBorderStyleTine { get; set; } = true;

    /// <summary>
    /// 排除据源中要导出的列名，列名与类型的属性名一致；若数据源是 DataTable，需与列名一致。
    /// </summary>
    public List<string> Excludes { get; } = new(0);
}

/// <summary>
/// 自定义导出行。
/// </summary>
public sealed class RowCustom
{
    /// <summary>
    /// 与顶部的间隔行。
    /// </summary>
    public int PaddingTop { get; set; }

    /// <summary>
    /// 与左边的区域间隔。
    /// </summary>
    public int PaddingLeft { get; set; }

    /// <summary>
    /// 与底部的间隔行。
    /// </summary>
    public int PaddingButtom { get; set; }

    /// <summary>
    /// 行跨度，默认为 1。
    /// </summary>
    public int RowSpan { get; set; } = 1;

    /// <summary>
    /// 列跨度，默认为 1。
    /// </summary>
    public int ColunmSpan { get; set; } = 1;

    /// <summary>
    /// 文本内容。
    /// </summary>
    public string? Text { get; set; }

    /// <summary>
    /// 文本水平方向对齐方式。
    /// </summary>
    public HorizontalAlignment HorizontalAlignment { get; set; } = HorizontalAlignment.Left;
}

/// <summary>
/// 文本水平方向对齐方式。
/// </summary>
public enum HorizontalAlignment
{
    Left,
    Center,
    Right,
}

/// <summary>
/// 要导出的数据。
/// </summary>
public sealed class ExcelExportData<T>
{
    public List<RowCustom>? Header { get; set; }

    /// <summary>
    /// 注数据
    /// </summary>
    [NotNull]
    public List<T>? Body { get; set; }

    public List<RowCustom>? Footer { get; set; }
}

/// <summary>
/// Excel 类。
/// </summary>
public sealed class Excel
{
    static Excel()
    {
        ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial; // 非商业版
    }

    /// <summary>
    /// 导出 Excel。
    /// <para>导出 Excel 的 Header 优先使用导出类型的 <see cref="DisplayNameAttribute"/> 名称，若没有会使用类型的属性名。Excel 列顺序与属性顺序一致。</para>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="path">路径</param>
    /// <param name="sheetName">sheet 名称</param>
    /// <param name="data">要导出的数据</param>
    /// <param name="settings">设置</param>
    public static void Export<T>(string path, string sheetName, IEnumerable<T> data, ExcelSettings? settings = default)
    {
        if (File.Exists(path))
        {
            File.Delete(path);
        }

        using var package = new ExcelPackage(path);
        var sheet = package.Workbook.Worksheets.Add(sheetName);
        ExportToSheet(sheet, data, 1, settings);
        package.Save();
    }

    /// <summary>
    /// 导出 Excel。
    /// <para>导出 Excel 的 Header 优先使用导出类型的 <see cref="DisplayNameAttribute"/> 名称，若没有会使用类型的属性名。Excel 列顺序与属性顺序一致。</para>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="path">路径</param>
    /// <param name="sheetName">sheet 名称</param>
    /// <param name="data">要导出的数据</param>
    /// <param name="settings">设置</param>
    public static async Task ExportAsync<T>(string path, string sheetName, IEnumerable<T> data, ExcelSettings? settings = default)
    {
        if (File.Exists(path))
        {
            File.Delete(path);
        }

        using var package = new ExcelPackage(path);
        var sheet = package.Workbook.Worksheets.Add(sheetName);
        ExportToSheet(sheet, data, 1, settings);
        await package.SaveAsync();
    }

    /// <summary>
    /// 导出 Excel。
    /// <para>导出 Excel 的 Header 优先使用导出类型的 <see cref="DisplayNameAttribute"/> 名称，若没有会使用类型的属性名。Excel 列顺序与属性顺序一致。</para>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="fileStream">文件流</param>
    /// <param name="sheetName">sheet 名称</param>
    /// <param name="data">要导出的数据</param>
    /// <param name="settings">设置</param>
    public static void Export<T>(Stream fileStream, string sheetName, IEnumerable<T> data, ExcelSettings? settings = default)
    {
        using var package = new ExcelPackage(fileStream);
        var sheet = package.Workbook.Worksheets.Add(sheetName);
        ExportToSheet(sheet, data, 1, settings);
        package.Save();
    }

    /// <summary>
    /// 导出 Excel。
    /// <para>导出 Excel 的 Header 优先使用导出类型的 <see cref="DisplayNameAttribute"/> 名称，若没有会使用类型的属性名。Excel 列顺序与属性顺序一致。</para>
    /// </summary>
    public static async Task ExportAsync<T>(Stream fileStream, string sheetName, IEnumerable<T> data, ExcelSettings? settings = default)
    {
        using var package = new ExcelPackage(fileStream);
        var sheet = package.Workbook.Worksheets.Add(sheetName);
        ExportToSheet(sheet, data, 1, settings);
        await package.SaveAsync();
    }

    /// <summary>
    /// 导出 Excel。
    /// <para>导出 Excel 的 Header 优先使用导出类型的 <see cref="DisplayNameAttribute"/> 名称，若没有会使用类型的属性名。Excel 列顺序与属性顺序一致。</para>
    /// </summary>
    public static void Export<T>(string path, string sheetName, ExcelExportData<T> data, ExcelSettings? settings = default)
    {
        if (File.Exists(path))
        {
            File.Delete(path);
        }

        using var package = new ExcelPackage(path);
        var sheet = package.Workbook.Worksheets.Add(sheetName);
        SetMainData(sheet, data, settings);
        package.Save();
    }

    /// <summary>
    /// 导出 Excel。
    /// <para>导出 Excel 的 Header 优先使用导出类型的 <see cref="DisplayNameAttribute"/> 名称，若没有会使用类型的属性名。Excel 列顺序与属性顺序一致。</para>
    /// </summary>
    public static async Task ExportAsync<T>(string path, string sheetName, ExcelExportData<T> data, ExcelSettings? settings = default)
    {
        if (File.Exists(path))
        {
            File.Delete(path);
        }

        using var package = new ExcelPackage(path);
        var sheet = package.Workbook.Worksheets.Add(sheetName);
        SetMainData(sheet, data, settings);
        await package.SaveAsync();
    }

    /// <summary>
    /// 导出 Excel。
    /// <para>导出 Excel 的 Header 优先使用导出类型的 <see cref="DisplayNameAttribute"/> 名称，若没有会使用类型的属性名。Excel 列顺序与属性顺序一致。</para>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="fileStream">文件流</param>
    /// <param name="sheetName">sheet 名称</param>
    /// <param name="dataTable">要导出的数据</param>
    /// <param name="settings">设置</param>
    public static void Export(Stream fileStream, string sheetName, DataTable dataTable, ExcelSettings? settings = default)
    {
        using var package = new ExcelPackage(fileStream);
        var sheet = package.Workbook.Worksheets.Add(sheetName);
        ExportToSheet(sheet, dataTable, 1, settings);
        package.Save();
    }

    /// <summary>
    /// 导出 Excel。
    /// <para>导出 Excel 的 Header 优先使用导出类型的 <see cref="DisplayNameAttribute"/> 名称，若没有会使用类型的属性名。Excel 列顺序与属性顺序一致。</para>
    /// </summary>
    /// <param name="fileStream">文件流</param>
    /// <param name="sheetName">sheet 名称</param>
    /// <param name="dataTable">要导出的数据</param>
    /// <param name="settings">设置</param>
    public static async Task ExportAsync<T>(Stream fileStream, string sheetName, DataTable dataTable, ExcelSettings? settings = default)
    {
        using var package = new ExcelPackage(fileStream);
        var sheet = package.Workbook.Worksheets.Add(sheetName);
        ExportToSheet(sheet, dataTable, 1, settings);
        await package.SaveAsync();
    }

    /// <summary>
    /// 导出 Excel。
    /// <para>导出 Excel 的 Header 优先使用导出类型的 <see cref="DisplayNameAttribute"/> 名称，若没有会使用类型的属性名。Excel 列顺序与属性顺序一致。</para>
    /// </summary>
    /// <param name="path">路径</param>
    /// <param name="sheetName">sheet 名称</param>
    /// <param name="dataTable">要导出的数据</param>
    /// <param name="settings">设置</param>
    public static void Export(string path, string sheetName, DataTable dataTable, ExcelSettings? settings = default)
    {
        if (File.Exists(path))
        {
            File.Delete(path);
        }

        using var package = new ExcelPackage(path);
        var sheet = package.Workbook.Worksheets.Add(sheetName);
        SetMainData(sheet, dataTable, settings);
        package.Save();
    }

    /// <summary>
    /// 导出 Excel。
    /// <para>导出 Excel 的 Header 优先使用导出类型的 <see cref="DisplayNameAttribute"/> 名称，若没有会使用类型的属性名。Excel 列顺序与属性顺序一致。</para>
    /// </summary>
    /// <param name="path">路径</param>
    /// <param name="sheetName">sheet 名称</param>
    /// <param name="dataTable">要导出的数据</param>
    /// <param name="settings">设置</param>
    public static async Task ExportAsync(string path, string sheetName, DataTable dataTable, ExcelSettings? settings = default)
    {
        if (File.Exists(path))
        {
            File.Delete(path);
        }

        using var package = new ExcelPackage(path);
        var sheet = package.Workbook.Worksheets.Add(sheetName);
        SetMainData(sheet, dataTable, settings);
        await package.SaveAsync();
    }

    private static void SetMainData<T>(ExcelWorksheet sheet, ExcelExportData<T> data, ExcelSettings? settings = default)
    {
        int rowIndex = 0; // Row/Col 起始为 1。

        // 设置 Header
        if (data.Header != null)
        {
            rowIndex += SetCustomRow(sheet, rowIndex, data.Header);
        }

        // 设置主体数据
        rowIndex++;
        ExportToSheet(sheet, data.Body!, rowIndex, settings);

        rowIndex += data.Body!.Count; // 重置行索引位置

        // 设置 Footer
        if (data.Footer != null)
        {
            SetCustomRow(sheet, rowIndex, data.Footer);
        }
    }

    private static void SetMainData(ExcelWorksheet sheet, DataTable dataTable, ExcelSettings? settings = default)
    {
        ExportToSheet(sheet, dataTable, 1, settings);
    }

    private static int SetCustomRow(ExcelWorksheet sheet, int rowIndex, IEnumerable<RowCustom> rows)
    {
        foreach (var row in rows)
        {
            rowIndex += row.PaddingTop + 1;

            int fromRow = rowIndex;
            int fromCol = 1 + row.PaddingLeft;
            int toRow = fromRow + (row.RowSpan - 1);
            int toCol = fromCol + (row.ColunmSpan - 1);

            var cell = sheet.Cells[fromRow, fromCol, toRow, toCol];
            if (row.RowSpan > 1 || row.ColunmSpan > 1)
            {
                cell.Merge = true;
            }

            cell.Value = row.Text;

            cell.Style.HorizontalAlignment = row.HorizontalAlignment switch
            {
                HorizontalAlignment.Left => ExcelHorizontalAlignment.Left,
                HorizontalAlignment.Center => ExcelHorizontalAlignment.Center,
                HorizontalAlignment.Right => ExcelHorizontalAlignment.Right,
                _ => ExcelHorizontalAlignment.Left,
            };

            rowIndex += (row.RowSpan - 1) + row.PaddingButtom;
        }

        return rowIndex;
    }

    private static void ExportToSheet<T>(ExcelWorksheet sheet, IEnumerable<T> data, int startRow, ExcelSettings? settings = default)
    {
        settings ??= new ExcelSettings();

        // 考虑导出功能使用频率低，因此不使用缓存机制。
        List<(string DisplayName, PropertyInfo PropInfo, bool IsEnum)> columns = new();
        var props = typeof(T).GetProperties();
        if (settings.Excludes.Any())
        {
            props = props.Where(s => !settings.Excludes.Contains(s.Name)).ToArray();
        }

        foreach (var prop in props)
        {
            var attr = prop.GetCustomAttribute<DisplayNameAttribute>();
            var isEnum = EnumExtensions.IsEnum(prop!.PropertyType);
            columns.Add((attr?.DisplayName ?? prop.Name, prop, isEnum));
        }

        // 构建头
        for (int i = 0; i < columns.Count; i++)
        {
            var cell = sheet.Cells[startRow, i + 1];
            cell.Value = columns[i].DisplayName;
            cell.Style.Font.Bold = true;
            if (settings.IstBorderStyleTine)
            {
                cell.Style.Border.BorderAround(ExcelBorderStyle.Thin);
            }
            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        }

        int n = startRow + 1;
        // 构建主体内容
        foreach (var item in data)
        {
            for (int j = 0; j < columns.Count; j++)
            {
                var isEnum = columns[j].IsEnum;
                var propInfo = columns[j].PropInfo;
                var cell = sheet.Cells[n, j + 1];

                var v = propInfo!.GetValue(item);
                if (isEnum)
                {
                    // 每条数据都进行反射，后续可提取枚举值。
                    cell.Value = ((Enum)v!).Desc() ?? v;
                }
                else
                {
                    cell.Value = v;
                }

                if (propInfo!.PropertyType == typeof(DateTime)
                    || propInfo!.PropertyType == typeof(DateTime?))
                {
                    cell.Style.Numberformat.Format = settings.DateTimeFormat;
                }
                else if (propInfo!.PropertyType == typeof(decimal) || propInfo!.PropertyType == typeof(decimal?)
                    || propInfo!.PropertyType == typeof(double) || propInfo!.PropertyType == typeof(double?)
                    || propInfo!.PropertyType == typeof(float) || propInfo!.PropertyType == typeof(float?))
                {
                    cell.Style.Numberformat.Format = settings.RealFormat;
                }

                if (settings.IstBorderStyleTine)
                {
                    cell.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                }

                cell.AutoFitColumns();
            }

            n++;
        }
    }

    private static void ExportToSheet(ExcelWorksheet sheet, DataTable dataTable, int startRow, ExcelSettings? settings = default)
    {
        settings ??= new ExcelSettings();

        List<(string Name, Type DataType)> columns = new();
        foreach (DataColumn col in dataTable.Columns)
        {
            if (settings.Excludes.Contains(col.ColumnName))
            {
                continue;
            }

            columns.Add((col.ColumnName, col.DataType));
        }

        // 构建头
        for (int i = 0; i < columns.Count; i++)
        {
            var cell = sheet.Cells[startRow, i + 1];
            cell.Value = columns[i].Name;
            cell.Style.Font.Bold = true;
            if (settings.IstBorderStyleTine)
            {
                cell.Style.Border.BorderAround(ExcelBorderStyle.Thin);
            }
            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        }

        int n = startRow + 1;
        // 构建主体内容
        foreach (DataRow row in dataTable.Rows)
        {
            for (int j = 0; j < columns.Count; j++)
            {
                var name = columns[j].Name;
                var dataType = columns[j].DataType;

                var cell = sheet.Cells[n, j + 1];
                cell.Value = row[name];

                if (dataType == typeof(DateTime) || dataType == typeof(DateTime?))
                {
                    cell.Style.Numberformat.Format = settings.DateTimeFormat;
                }
                else if (dataType == typeof(decimal) || dataType == typeof(decimal?)
                    || dataType == typeof(double) || dataType == typeof(double?)
                    || dataType == typeof(float) || dataType == typeof(float?))
                {
                    cell.Style.Numberformat.Format = settings.RealFormat;
                }

                if (settings.IstBorderStyleTine)
                {
                    cell.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                }

                cell.AutoFitColumns();
            }

            n++;
        }
    }
}
