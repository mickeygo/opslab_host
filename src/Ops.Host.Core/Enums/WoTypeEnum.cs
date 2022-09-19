namespace Ops.Host.Core;

/// <summary>
/// 工单类型枚举
/// </summary>
public enum WoTypeEnum
{
    /// <summary>
    /// 正式
    /// </summary>
    [Description("正式")]
    Official = 1,

    /// <summary>
    /// 试制
    /// </summary>
    [Description("试制")]
    Trial,
}
