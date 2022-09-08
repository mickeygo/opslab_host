namespace Ops.Host.Shared.ViewModel;


/// <summary>
/// 打印模型构建器。
/// </summary>
public sealed class PrintModelBuilder
{
    /// <summary>
    /// 打印模式。
    /// </summary>
    public PrintMode Mode { get; set; } = PrintMode.Preview;

    /// <summary>
    /// 要打印的模板路径。
    /// </summary>
    public string? TemplateUrl { get; set; }

    /// <summary>
    /// 文档描述
    /// </summary>
    public string DocumentDescription { get; set; } = "Document";

    /// <summary>
    /// 数据上下文
    /// </summary>
    public object? DataContext { get; set; }

    /// <summary>
    /// 文档呈现器。
    /// </summary>
    public IDocumentRenderer? Render { get; set; }

    /// <summary>
    /// 打印模式。
    /// </summary>
    public enum PrintMode
    {
        /// <summary>
        /// 打印预览。
        /// </summary>
        Preview,

        /// <summary>
        /// 弹出打印框。
        /// </summary>
        Dialog,

        /// <summary>
        /// 直接打印。
        /// </summary>
        Direct,
    }
}