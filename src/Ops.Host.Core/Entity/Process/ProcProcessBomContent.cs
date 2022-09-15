namespace Ops.Host.Core.Entity;

/// <summary>
/// 工艺BOM内容
/// </summary>
[SugarTable("proc_process_bom_content", "工艺BOM内容表")]
public sealed class ProcProcessBomContent : EntityBase
{
    /// <summary>
    /// 工艺BOM Id。
    /// </summary>
    public long ProcessBomId { get; set; }

    /// <summary>
    /// 物料信息 Id。
    /// </summary>
    public long MaterialId { get; set; }

    /// <summary>
    /// 物料信息。
    /// </summary>
    [Navigate(NavigateType.OneToOne, nameof(MaterialId))]
    public MdItem? Material { get; set; }

    /// <summary>
    /// 使用数量。
    /// </summary>
    public int Qty { get; set; }

    /// <summary>
    /// 上料顺序号。
    /// </summary>
    /// <remarks>用在有顺序的扫码上料流程中校验，当数值大于 0 时有效。</remarks>
    public int Seq { get; set; }
}
