namespace Ops.Host.Core.Entity;

/// <summary>
/// SN 物料扫码记录，包含关键物料和批次料。
/// <para>注：数据不会硬删除，可手动更改属性。</para>
/// </summary>
[SugarTable("pt_sn_material", "物料追溯信息表")]
public sealed class PtSnMaterial : EntityBase
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
    /// 物料属性。
    /// </summary>
    [SugarColumn(ColumnDescription = "产品物料属性")]
    public MaterialAttrEnum Attr { get; set; }

    /// <summary>
    /// 物料绑定状态
    /// </summary>
    [SugarColumn(ColumnDescription = "物料绑定状态")]
    public BindingEnum BindingStatus { get; set; } = BindingEnum.Bind;
}
