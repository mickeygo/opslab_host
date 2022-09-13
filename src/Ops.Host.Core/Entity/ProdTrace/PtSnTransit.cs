namespace Ops.Host.Core.Entity;

/// <summary>
/// SN 当前过站状态信息。
/// </summary>
/// <remarks>此表中 SN 数据应该唯一。</remarks>
[SugarTable("pt_sn_transit", "SN当前过站状态表")]
public sealed class PtSnTransit : EntityBaseId
{
    /// <summary>
    /// SN
    /// </summary>
    [SugarColumn(ColumnDescription = "SN", Length = 64)]
    [Required, MaxLength(64)]
    [NotNull]
    public string? SN { get; set; }

    /// <summary>
    /// 产品
    /// </summary>
    [SugarColumn(ColumnDescription = "产品", Length = 64)]
    [Description("产品")]
    [MaxLength(64)]
    public string? ProductCode { get; set; }

    /// <summary>
    /// 工单号
    /// <para>工单可以不存在。</para>
    /// </summary>
    [SugarColumn(ColumnDescription = "工单号", Length = 64)]
    [MaxLength(64)]
    public string? WO { get; set; }

    /// <summary>
    /// 产线代码
    /// </summary>
    [SugarColumn(ColumnDescription = "产线代码", Length = 64)]
    [Required, MaxLength(64)]
    [NotNull]
    public string? LineCode { get; set; }

    /// <summary>
    /// 工站代码
    /// </summary>
    [SugarColumn(ColumnDescription = "工站代码", Length = 64)]
    [Required, MaxLength(64)]
    [NotNull]
    public string? StationCode { get; set; }

    /// <summary>
    /// 过站状态
    /// </summary>
    [DisplayName("过站状态")]
    public TransitModeEnum TransitMode { get; set; }

    /// <summary>
    /// 过站状态
    /// </summary>
    [SugarColumn(ColumnDescription = "过站状态")]
    public PassEnum? Pass { get; set; }

    /// <summary>
    /// 进站时间
    /// </summary>
    [SugarColumn(ColumnDescription = "进站时间")]
    public DateTime? InboundTime { get; set; }

    /// <summary>
    /// 出站时间
    /// </summary>
    [SugarColumn(ColumnDescription = "出站时间")]
    public DateTime? OutboundTime { get; set; }
}
