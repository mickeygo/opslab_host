namespace Ops.Host.Core;

/// <summary>
/// 工站归属枚举。
/// </summary>
public enum StationOwnerEnum
{
    /// <summary>
    /// 线上站。
    /// </summary>
    [Description("线上站")]
    Inline = 1,

    /// <summary>
    /// 线外站。
    /// </summary>
    [Description("线外站")]
    Outside,
}