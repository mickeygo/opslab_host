namespace Ops.Host.Core;

/// <summary>
/// 动态值类型枚举。
/// </summary>
public enum DynamicDataTypeEnum
{
    /// <summary>
    /// 浮点型，包括 float，double 和 decimal。
    /// </summary>
    [Description("浮点型")]
    Float = 1,

    /// <summary>
    /// 整型类型，包括 byte、uint16/int16、uint32/int32 和 uint64/int64。
    /// </summary>
    [Description("整型")]
    Integer,

    /// <summary>。
    /// 布尔类型
    /// </summary>
    [Description("布尔型")]
    Boolean,

    /// <summary>
    /// 字符串类型。
    /// </summary>
    [Description("字符串")]
    String,
}
