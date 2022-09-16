namespace Ops.Host.Core.Entity;

/// <summary>
/// 工艺参数
/// </summary>
[SugarTable("proc_process_param", "工艺参数表")]
public sealed class ProcProcessParam : EntityBase
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
    /// 工艺参数详细项集合。
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(ProcProcessParamContent.ProcessParamId))]
    public List<ProcProcessParamContent>? Contents { get; set; }
}
