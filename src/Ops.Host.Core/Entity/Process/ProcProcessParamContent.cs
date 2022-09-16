namespace Ops.Host.Core.Entity;

/// <summary>
/// 工艺参数内容
/// </summary>
[SugarTable("proc_process_param_content", "工艺参数内容表")]
public class ProcProcessParamContent : EntityBaseId
{
    /// <summary>
    /// 工艺参数详主表Id
    /// </summary>
    public long ProcessParamId { get; set; }

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

    /// <summary>
    /// 是否进行校验，默认为 true。
    /// </summary>
    /// <remarks>当全局校验开启时，此参数才有效。</remarks>
    public bool IsCheck { get; set; } = true;
}
