namespace Ops.Host.Core.Entity;

/// <summary>
/// 产品进站信息
/// </summary>
[SugarTable("pt_inbound", "产品进站信息表")]
[SugarIndex("index_pt_inbound_sn", nameof(SN), OrderByType.Asc)]
public sealed class PtInbound : EntityBaseId
{
    /// <summary>
    /// 产线编码
    /// </summary>
    [SugarColumn(ColumnDescription = "产线编码", Length = 64)]
    [Required, MaxLength(64)]
    [NotNull]
    public string? LineCode { get; set; }

    /// <summary>
    /// 工站编码
    /// </summary>
    [SugarColumn(ColumnDescription = "工站编码", Length = 64)]
    [Required, MaxLength(64)]
    [NotNull]
    public string? StationCode { get; set; }

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
    /// SN
    /// </summary>
    [SugarColumn(ColumnDescription = "SN", Length = 64)]
    [Required, MaxLength(64)]
    [NotNull]
    public string? SN { get; set; }

    /// <summary>
    /// 程序配方号
    /// </summary>
    [SugarColumn(ColumnDescription = "程序配方号")]
    public int FormualNo { get; set; }

    /// <summary>
    /// 进站时间
    /// </summary>
    [SugarColumn(ColumnDescription = "进站时间")]
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 产品进站明细信息集合
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(PtInboundItem.InboundId))]
    public List<PtInboundItem>? InboundItems { get; set; }
}
