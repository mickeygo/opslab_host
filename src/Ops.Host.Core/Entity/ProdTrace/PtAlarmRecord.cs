namespace Ops.Host.Core.Entity;

/// <summary>
/// 警报记录
/// </summary>
[SugarTable("pt_alarm_record", "警报记录表")]
public class PtAlarmRecord : EntityBaseId
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
    /// 分类
    /// </summary>
    [SugarColumn(ColumnDescription = "分类", Length = 32)]
    [Required, MaxLength(32)]
    [NotNull]
    public string? Category { get; set; }

    /// <summary>
    /// 描述
    /// </summary>
    [SugarColumn(ColumnDescription = "描述", Length = 64)]
    [MaxLength(64)]
    public string? Descirption { get; set; }

    /// <summary>
    /// 警报时间
    /// </summary>
    [SugarColumn(ColumnDescription = "警报时间")]
    public DateTime CreateTime { get; set; }
}
