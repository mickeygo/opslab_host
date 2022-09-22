namespace Ops.Host.Core;

/// <summary>
/// 过站阶段枚举
/// </summary>
public enum TransitStageEnum
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
}
