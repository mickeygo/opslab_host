namespace Ops.Host.Core.Entity;

/// <summary>
/// 产品出站/存档明细信息
/// </summary>
[SugarTable("pt_archive_item", "产品出站存档明细信息表")]
[SugarIndex("index_pt_archive_item_archiveid", nameof(ArchiveId), OrderByType.Asc)]
public class PtArchiveItem : EntityBaseId
{
    /// <summary>
    /// 产品数据存档信息 Id。
    /// </summary>
    [SugarColumn(ColumnDescription = "产品存档信息Id")]
    public long ArchiveId { get; set; }

    /// <summary>
    /// 产品数据存档信息
    /// </summary>
    [Navigate(NavigateType.OneToOne, nameof(ArchiveId))]
    public PtArchive? Archive { get; set; }

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
    /// 值是否是数组（非字符串类型）
    /// </summary>
    [SugarColumn(ColumnDescription = "值是否是数组")]
    public bool IsArray { get; set; }

    /// <summary>
    /// 上限值
    /// </summary>
    [SugarColumn(ColumnDescription = "上限值", Length = 12, DecimalDigits = 2)]
    public decimal? Higher { get; set; }

    /// <summary>
    /// 下限值
    /// </summary>
    [SugarColumn(ColumnDescription = "下限值", Length = 12, DecimalDigits = 2)]
    public decimal? Lower { get; set; }

    /// <summary>
    /// 是否合格。
    /// </summary>
    /// <remarks>当有设置上限或下限且在相应范围内时合格</remarks>
    [SugarColumn(ColumnDescription = "是否合格")]
    public bool IsPass { get; set; }

    /// <summary>
    /// 产品出站存档明细项数组值明细信息集合
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(PtArchiveItemLine.ArchiveItemId))]
    public List<PtArchiveItemLine>? ArchiveItemLines { get; set; }
}
