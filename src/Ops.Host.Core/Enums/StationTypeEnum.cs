namespace Ops.Host.Core;

/// <summary>
/// 工站类型枚举。
/// </summary>
public enum StationTypeEnum
{
    /// <summary>
    /// 装配站。
    /// </summary>
    [Description("装配")]
    Assembly = 1,

    /// <summary>
    /// 检验站。
    /// </summary>
    [Description("检验")]
    Test,
}
