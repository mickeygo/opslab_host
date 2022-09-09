namespace Ops.Host.Shared.ViewModel;

/// <summary>
/// 导出的 Excel 模型构建器。
/// </summary>
public sealed class ExcelModelBuilder
{
    /// <summary>
    /// 保存的 Excel 默认名称。
    /// </summary>
    /// <remarks>不设置会默认使用 yyyyMMddHHmm 格式。</remarks>
    public string? ExcelName { get; set; }

    /// <summary>
    /// Excel Sheet 名称。
    /// </summary>
    /// <remarks>默认使用 ExcelName 名称，若 ExcelName 为空，会设置为 'Sheet1'。</remarks>
    public string? SheetName { get; set; }

    /// <summary>
    /// Excel 名称是否添加日期（yyyyMMdd）后缀，默认的 Sheet 名称不会添加。
    /// </summary>
    /// <remarks>若没有设置 Excel 名称，此时也不会添加后缀。</remarks>
    public bool HasExcelNameDatePostfix { get; set; }

    /// <summary>
    /// 是否只导出设置了 <see cref="DisplayNameAttribute"/> 特性的列，默认为 true。
    /// </summary>
    public bool IsOnlyDisplayNameColomn { get; set; } = true;

    /// <summary>
    /// Excel 设置。
    /// </summary>
    public ExcelSettings Settings { get; } = new();

    /// <summary>
    /// 设置要导出的 Excel 头部块，空表示没有。
    /// </summary>
    public List<RowCustom>? Header { get; set; }

    /// <summary>
    /// 设置要导出的 Excel 尾部块，空表示没有。
    /// </summary>
    public List<RowCustom>? Footer { get; set; }
}
