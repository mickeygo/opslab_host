namespace Ops.Host.Core.Entity;

/// <summary>
/// 工艺路线信息。
/// </summary>
[SugarTable("proc_process_route", "工艺路线表")]
public sealed class ProcProcessRoute : EntityBase
{
    /// <summary>
    /// 产品信息Id
    /// </summary>
    [SugarColumn(ColumnDescription = "产品信息Id")]
    public long ProductId { get; set; }

    /// <summary>
    /// 产品信息
    /// </summary>
    [SugarColumn(IsIgnore = true)]
    public MdItem? Product { get; set; }

    /// <summary>
    /// 工艺路线代码
    /// </summary>
    [SugarColumn(ColumnDescription = "工艺路线代码", Length = 64)]
    [Required, MaxLength(64)]
    [NotNull]
    public string? Code { get; set; }

    /// <summary>
    /// 工艺路线名称
    /// </summary>
    [SugarColumn(ColumnDescription = "工艺路线名称", Length = 64)]
    [Required, MaxLength(64)]
    [NotNull]
    public string? Name { get; set; }

    /// <summary>
    /// 当前工序 Id
    /// </summary>
    [SugarColumn(ColumnDescription = "当前工序 Id")]
    public long CurrentId { get; set; }

    /// <summary>
    /// 当前工序
    /// </summary>
    [Navigate(NavigateType.OneToOne, nameof(CurrentId))]
    public ProcProcess? Current { get; set; }

    /// <summary>
    /// 下一工序 Id，Null 表示当前为最终工序。
    /// </summary>
    [SugarColumn(ColumnDescription = "下一工序 Id")]
    public long? NextId { get; set; }

    /// <summary>
    /// 下一工序
    /// </summary>
    [Navigate(NavigateType.OneToOne, nameof(NextId))]
    public ProcProcess? Next { get; set; }
}
