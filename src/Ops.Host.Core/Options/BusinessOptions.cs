namespace Ops.Host.Core;

/// <summary>
/// 业务选项
/// </summary>
public class BusinessOptions
{
    /// <summary>
    /// 是否使用工单，默认为 true。
    /// </summary>
    public bool UseWo { get; set; } = true;

    /// <summary>
    /// 是否校验物料，默认为 true。
    /// </summary>
    public bool IsMatchMaterial { get; set; } = true;

    /// <summary>
    /// 比较物料规则时是否要长度一致，默认为 true。
    /// </summary>
    public bool IsMatchMaterialEqualLength { get; set; } = true;

    /// <summary>
    /// 是否要校验工艺路线，默认为 false。
    /// </summary>
    /// <remarks>进站时校验。</remarks>
    public bool IsMatchRoute { get; set; }

    /// <summary>
    /// 是否校验工艺参数，默认为 false。
    /// </summary>
    /// <remarks>出站/存档时校验。</remarks>
    public bool IsMatchProcessParam { get; set; }
}
