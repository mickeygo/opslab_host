namespace Ops.Host.Core;

/// <summary>
/// 业务选项
/// </summary>
public class BusinessOptions
{
    /// <summary>
    /// 是否使用工单。
    /// </summary>
    public bool UseWo { get; set; } = true;

    /// <summary>
    /// 是否校验物料。
    /// </summary>
    public bool IsMatchMaterial { get; set; } = true;

    /// <summary>
    /// 比较物料规则时是否要长度一致。
    /// </summary>
    public bool IsMatchMaterialEqualLength { get; set; } = true;

    /// <summary>
    /// 是否校验工艺参数。
    /// </summary>
    public bool IsMatchProcessParam { get; set; } = false;
}
