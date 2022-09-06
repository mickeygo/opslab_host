namespace Ops.Host.Core.Entity;

/// <summary>
/// 产品出站/存档信息
/// </summary>
[SugarTable("pt_archive", "产品出站存档信息表")]
[SugarIndex("index_pt_archive_sn", nameof(SN), OrderByType.Asc)]
public class PtArchive : EntityBaseId
{
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
    /// 物料号
    /// </summary>
    [SugarColumn(ColumnDescription = "物料号", Length = 64)]
    [Required, MaxLength(64)]
    [NotNull]
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
    /// 过站状态
    /// </summary>
    [SugarColumn(ColumnDescription = "过站状态")]
    public PassEnum Pass { get; set; }

    /// <summary>
    /// CT 时长
    /// </summary>
    [SugarColumn(ColumnDescription = "CT 时长")]
    public int? Cycletime { get; set; }

    /// <summary>
    /// 操作人
    /// </summary>
    [SugarColumn(ColumnDescription = "操作人", Length = 64)]
    [MaxLength(64)]
    public string? Operator { get; set; }

    /// <summary>
    /// 班次
    /// </summary>
    [SugarColumn(ColumnDescription = "班次", Length = 16)]
    [MaxLength(16)]
    public string? Shift { get; set; }

    /// <summary>
    /// 托盘号
    /// </summary>
    [SugarColumn(ColumnDescription = "托盘号", Length = 64)]
    [MaxLength(64)]
    public string? Pallet { get; set; }

    /// <summary>
    /// 存档时间
    /// </summary>
    [SugarColumn(ColumnDescription = "存档时间")]
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 产品出站/存档明细信息集合
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(PtArchiveItem.ArchiveId))]
    public List<PtArchiveItem>? ArchiveItems { get; set; }
}
