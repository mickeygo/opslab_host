namespace Ops.Host.Core.Entity;

/// <summary>
/// 工艺BOM
/// </summary>
[SugarTable("proc_process_bom", "工艺BOM表")]
public sealed class ProcProcessBom : EntityBase
{
    /// <summary>
    /// 产品 Id
    /// </summary>
    [SugarColumn(ColumnDescription = "产品Id")]
    public long ProductId { get; set; }

    /// <summary>
    /// 产品信息
    /// </summary>
    [Navigate(NavigateType.OneToOne, nameof(ProductId))]
    public MdItem? Product { get; set; }

    /// <summary>
    /// 工序 Id
    /// </summary>
    public long ProcessId { get; set; }

    /// <summary>
    /// 工序
    /// </summary>
    [Navigate(NavigateType.OneToOne, nameof(ProcessId))]
    public ProcProcess? Process { get; set; }

    /// <summary>
    /// BOM 详细项集合
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(ProcProcessBomContent.ProcessBomId))]
    public List<ProcProcessBomContent>? Contents { get; set; }
}
