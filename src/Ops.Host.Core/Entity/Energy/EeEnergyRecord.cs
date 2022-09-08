namespace Ops.Host.Core.Entity;

/// <summary>
/// 能耗记录
/// </summary>
[SugarTable("ee_energy_record", "能耗记录表")]
[SugarIndex("index_ee_energy_record_createtime", nameof(CreateTime), OrderByType.Desc)]
public sealed class EeEnergyRecord : EntityBaseId
{
    /// <summary>
    /// 产线代码
    /// </summary>
    [DisplayName("产线")]
    [SugarColumn(ColumnDescription = "产线代码", Length = 64)]
    [Required, MaxLength(64)]
    [NotNull]
    public string? LineCode { get; set; }

    /// <summary>
    /// 工站代码
    /// </summary>
    [DisplayName("工站")]
    [SugarColumn(ColumnDescription = "工站代码", Length = 64)]
    [Required, MaxLength(64)]
    [NotNull]
    public string? StationCode { get; set; }

    /// <summary>
    /// 分类
    /// </summary>
    [DisplayName("分类")]
    [SugarColumn(ColumnDescription = "分类", Length = 32)]
    [Required, MaxLength(32)]
    [NotNull]
    public string? Category { get; set; }

    /// <summary>
    /// 能耗值
    /// </summary>
    [DisplayName("能耗值")]
    [SugarColumn(ColumnDescription = "能耗值", Length = 12, DecimalDigits = 2)]
    public decimal Value { get; set; }

    /// <summary>
    /// 记录时间
    /// </summary>
    [DisplayName("记录时间")]
    [SugarColumn(ColumnDescription = "记录时间")]
    public DateTime CreateTime { get; set; }
}
