namespace Ops.Host.Core.Entity;

/// <summary>
/// 产品进站明细信息
/// </summary>
[SugarTable("pt_inbound_item", "产品进站信息表")]
[SugarIndex("index_pt_inbound_item_inboundid", nameof(InboundId), OrderByType.Asc)]
public sealed class PtInboundItem : EntityBaseId
{
    /// <summary>
    /// 产品进站信息 Id。
    /// </summary>
    [SugarColumn(ColumnDescription = "产品进站信息Id")]
    public long InboundId { get; set; }

    /// <summary>
    /// 产品进站信息
    /// </summary>
    [Navigate(NavigateType.OneToOne, nameof(InboundId))]
    public PtInbound? Inbound { get; set; }

    /// <summary>
    /// 标签
    /// </summary>
    [SugarColumn(ColumnDescription = "标签", Length = 64)]
    [Required, MaxLength(64)]
    [NotNull]
    public string? Tag { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    [SugarColumn(ColumnDescription = "名称", Length = 64)]
    [Required, MaxLength(64)]
    [NotNull]
    public string? Name { get; set; }

    /// <summary>
    /// 值
    /// </summary>
    [SugarColumn(ColumnDescription = "值", Length = 1024)]
    [Required, MaxLength(1024)]
    [NotNull]
    public string? Value { get; set; }

    /// <summary>
    /// 值是否是数组
    /// </summary>
    [SugarColumn(ColumnDescription = "值是否是数组")]
    public bool IsArray { get; set; }
}
