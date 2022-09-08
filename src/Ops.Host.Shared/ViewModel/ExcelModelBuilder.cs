namespace Ops.Host.Shared.ViewModel;

/// <summary>
/// 导出的 Excel 模型构建器。
/// </summary>
public sealed class ExcelModelBuilder
{
    /// <summary>
    /// 保存的 Excel 默认名称。
    /// </summary>
    public string? ExcelName { get; set; }

    /// <summary>
    /// Excel Sheet 名称。
    /// </summary>
    public string? SheetName { get; set; }

    public ExcelSettings Settings { get; } = new();

    /// <summary>
    /// 导出的 Excel 头部。
    /// </summary>
    public List<RowCustom>? Header { get; set; }

    /// <summary>
    /// 导出的 Excel 尾部。
    /// </summary>
    public List<RowCustom>? Footer { get; set; }
}
