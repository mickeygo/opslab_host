namespace Ops.Host.Core;

/// <summary>
/// 工站位置（表示首站、尾站等）
/// </summary>
public enum StationFlowEnum
{
    /// <summary>
    /// 未设置
    /// </summary>
    [Description("未设置")]
    None = 0,

    /// <summary>
    /// 首站
    /// </summary>
    [Description("首站")]
    Header = 1,

    /// <summary>
    /// 中间站
    /// </summary>
    [Description("中间站")]
    Middle = 2,

    /// </summary>
    [Description("尾站")]
    Tail = 3,
}
