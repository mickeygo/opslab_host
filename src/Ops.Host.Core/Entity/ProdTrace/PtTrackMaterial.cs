namespace Ops.Host.Core.Entity;

/// <summary>
/// SN 关键物料追溯信息。
/// <para>注：物料解绑时会删除相关数据。</para>
/// </summary>
[SugarTable("pt_track_material", "物料追溯信息表")]
[SugarIndex("index_pt_track_material_sn", nameof(SN), OrderByType.Asc)]
[SugarIndex("index_pt_track_material_barcode", nameof(Barcode), OrderByType.Asc)]
public sealed class PtTrackMaterial : EntityBaseId
{
    /// <summary>
    /// SN
    /// </summary>
    [SugarColumn(ColumnDescription = "SN", Length = 64)]
    [Required, MaxLength(64)]
    [NotNull]
    public string? SN { get; set; }

    /// <summary>
    /// Barcode
    /// </summary>
    [SugarColumn(ColumnDescription = "Barcode", Length = 64)]
    [Required, MaxLength(64)]
    [NotNull]
    public string? Barcode { get; set; }

    /// <summary>
    /// 扫码时间
    /// </summary>
    [SugarColumn(ColumnDescription = "扫码时间")]
    public DateTime CreateTime { get; set; }
}
