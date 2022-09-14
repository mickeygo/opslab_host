namespace Ops.Host.Core.Entity;

/// <summary>
/// 返工项，同一个 SN 可能会出现多次返修。
/// </summary>
[SugarTable("re_rework_item", "返工项")]
public class ReReworkItem : EntityBase
{
    /// <summary>
    /// 返工 SN
    /// </summary>
    [SugarColumn(ColumnDescription = "SN", Length = 64)]
    [Required, MaxLength(64)]
    [NotNull]
    public string? SN { get; set; }

    /// <summary>
    /// NG 产线
    /// </summary>
    [SugarColumn(ColumnDescription = "NG产线", Length = 64)]
    [MaxLength(64)]
    public string? LineCode { get; set; }

    /// <summary>
    /// NG 工站
    /// </summary>
    [SugarColumn(ColumnDescription = "NG工站", Length = 64)]
    [MaxLength(64)]
    public string? StationCode { get; set; }

    /// <summary>
    /// 返修项来源工单
    /// </summary>
    [SugarColumn(ColumnDescription = "来源工单", Length = 64)]
    [Required, MaxLength(64)]
    [NotNull]
    public string? SourceOrder { get; set; }

    /// <summary>
    /// 返修状态。0->待返修；1->已返修。
    /// </summary>
    public int Status { get; set; }

    /// <summary>
    /// 下发日期
    /// </summary>
    public DateTime IssueDate { get; set; }

    /// <summary>
    /// 下发次数
    /// </summary>
    public int IssueCount { get; set; }

    /// <summary>
    /// 返修工序，表示从哪一步开始返修。
    /// </summary>
    public int ProcedureSeq { get; set; }
}
