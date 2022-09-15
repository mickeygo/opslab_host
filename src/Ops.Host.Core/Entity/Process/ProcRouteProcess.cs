namespace Ops.Host.Core.Entity;

/// <summary>
/// 工艺组成
/// </summary>
[SugarTable("proc_route_process", "工艺组成表")]
public class ProcRouteProcess : EntityBase
{
    /// <summary>
    /// 工艺路线 Id
    /// </summary>
    public long RouteId { get; set; }

    /// <summary>
    /// 工序 Id
    /// </summary>
    [SugarColumn(ColumnDescription = "工序Id")]
    public long ProcessId { get; set; }

    /// <summary>
    /// 当前工序
    /// </summary>
    [Navigate(NavigateType.OneToOne, nameof(ProcessId))]
    public ProcProcess? Process { get; set; }

    /// <summary>
    /// 下一道工序 Id。
    /// </summary>
    /// <remarks>null 表示没有下一道工序。</remarks>
    [SugarColumn(ColumnDescription = "下一道工序Id")]
    public long? NextProcessId { get; set; }

    /// <summary>
    /// 下一道工序
    /// </summary>
    [Navigate(NavigateType.OneToOne, nameof(NextProcessId))]
    public ProcProcess? NextProcess { get; set; }

    /// <summary>
    /// 顺序。
    /// </summary>
    public int Seq { get; set; }

    /// <summary>
    /// 与下一工序的关系。
    /// </summary>
    public FlowRelationshipEnum Relationship { get; set; }
}
