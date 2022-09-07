namespace Ops.Host.Core;

/// <summary>
/// 绑定状态枚举
/// </summary>
public enum BindingEnum
{
    /// <summary>
    /// 绑定
    /// </summary>
    [Description("绑定")]
    Bind = 1,

    /// <summary>
    /// 解绑
    /// </summary>
    [Description("已解绑")]
    Unbind = 2,
}
