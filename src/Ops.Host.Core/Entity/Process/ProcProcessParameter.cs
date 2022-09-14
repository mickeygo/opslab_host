namespace Ops.Host.Core.Entity;

/// <summary>
/// 工艺参数信息
/// </summary>
[SugarTable("proc_process_param", "工艺参数表")]
public sealed class ProcProcessParameter : EntityBase
{
    /// <summary>
    /// 工序 Id
    /// </summary>
    public long ProcessId { get; set; }

    /// <summary>
    /// 工序
    /// </summary>
    [Navigate(NavigateType.OneToOne, nameof(ProcessId))]
    public ProcProcess? Process { get; set; }

    /// <summary>
    /// 标签。
    /// <para>若源于 PLC，需与 PLC Tag 一致。对于数组可重复。</para>
    /// </summary>
    [DisplayName("标签")]
    [Required, MaxLength(64)]
    [NotNull]
    public string? Tag { get; set; }

    /// <summary>
    /// 参数名称。
    /// <para>对于数组可重复。</para>
    /// </summary>
    [DisplayName("参数名称")]
    [Required, MaxLength(64)]
    [NotNull]
    public string? Name { get; set; }

    /// <summary>
    /// 序号。
    /// <para>仅用于值为数组的标签，大于 0 表示为数组。</para>
    /// </summary>
    [SugarColumn(ColumnDescription = "序号")]
    public int Seq { get; set; }

    /// <summary>
    /// 数据类型
    /// </summary>
    [SugarColumn(ColumnDescription = "数据类型")]
    public VariableType DataType { get; set; }

    /// <summary>
    /// 上限值，空表示不校验。
    /// </summary>
    [DisplayName("上限值")]
    [SugarColumn(ColumnDescription = "上限值", Length = 12, DecimalDigits = 2)]
    public decimal? Higher { get; set; }

    /// <summary>
    /// 下限值，空表示不校验。
    /// </summary>
    [DisplayName("下限值")]
    [SugarColumn(ColumnDescription = "下限值", Length = 12, DecimalDigits = 2)]
    public decimal? Lower { get; set; }
}
