namespace Ops.Host.Core;

/// <summary>
/// 过站状态
/// </summary>
public enum TransitModeEnum
{
    /// <summary>
    /// 已进站
    /// </summary>
    [Description("已进站")]
    Inbound = 10,

    /// <summary>
    /// 已出站
    /// </summary>
    [Description("已出站")]
    Outbound = 20,

    /// <summary>
    /// 已完工
    /// </summary>
    [Description("已完工")]
    Completed = 30,
}
