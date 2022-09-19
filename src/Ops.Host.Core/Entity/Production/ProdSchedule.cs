namespace Ops.Host.Core.Entity;

/// <summary>
/// 生产排产
/// </summary>
[SugarTable("prod_schedule", "生产排产表")]
public sealed class ProdSchedule : EntityBase
{
    /// <summary>
    /// 工单Id
    /// </summary>
    [SugarColumn(ColumnDescription = "工单Id")]
    public long WoId { get; set; }

    /// <summary>
    /// 工单信息
    /// </summary>
    [Navigate(NavigateType.OneToOne, nameof(WoId))]
    public ProdWo? WorkOrder { get; set; }

    /// <summary>
    /// 工单编号
    /// </summary>
    [SugarColumn(ColumnDescription = "工单编号", Length = 64)]
    [Required, MaxLength(64)]
    [NotNull]
    public string? WO { get; set; }

    /// <summary>
    /// 顺序号
    /// </summary>
    [SugarColumn(ColumnDescription = "顺序号")]
    public int Seq { get; set; }
}
