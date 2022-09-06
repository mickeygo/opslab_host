namespace Ops.Host.Core;

/// <summary>
/// 过站状态枚举
/// </summary>
public enum PassEnum
{
    /// <summary>
    /// 无状态
    /// </summary>
    [Description("无状态")]
    None = 0,

    /// <summary>
    /// OK
    /// </summary>
    [Description("OK")]
    OK = 1,

    /// <summary>
    /// NG
    /// </summary>
    [Description("NG")]
    NG = 2,

    /// <summary>
    /// 强制 OK
    /// </summary>
    [Description("强制 OK")]
    ForceOK = 3,

    /// <summary>
    /// 强制 NG
    /// </summary>
    [Description("强制 NG")] 
    ForceNG = 4,
}
