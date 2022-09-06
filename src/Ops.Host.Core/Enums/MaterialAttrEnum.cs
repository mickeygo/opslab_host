namespace Ops.Host.Core;

/// <summary>
/// 物料属性枚举。
/// </summary>
public enum MaterialAttrEnum
{
    /// <summary>
    /// 产品。
    /// <para>表示该物料是某种产品。</para>
    /// </summary>
    [Description("产品")]
    Product = 1,

    /// <summary>
    /// 关键物料。
    /// </summary>
    [Description("关键物料")]
    Critical = 2,

    /// <summary>
    /// 批次料。
    /// </summary>
    [Description("批次料")]
    Batch = 3,
}
