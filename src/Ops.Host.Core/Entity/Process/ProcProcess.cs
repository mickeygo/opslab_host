namespace Ops.Host.Core.Entity;

/// <summary>
/// 生产工序
/// </summary>
[SugarTable("proc_process", "生产工序表")]
public sealed class ProcProcess : EntityBase
{
    /// <summary>
    /// 工序编码
    /// </summary>
    /// <remarks>来源于工站。</remarks>
    [DisplayName("工序编码")]
    [Required, MaxLength(64)]
    [NotNull]
    public string? Code { get; set; }

    /// <summary>
    /// 工序名称
    /// </summary>
    /// <remarks>来源于工站。</remarks>
    [DisplayName("工序名称")]
    [Required, MaxLength(64)]
    [NotNull]
    public string? Name { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    [DisplayName("备注")]
    [MaxLength(255)]
    public string? Remark { get; set; }

    /// <summary>
    /// 工序类型。
    /// </summary>
    /// <remarks>来源于工站。</remarks>
    public StationTypeEnum Type { get; set; }

    /// <summary>
    /// 工序归属。
    /// </summary>
    /// <remarks>来源于工站，线外站没有工艺路线。</remarks>
    public StationOwnerEnum Owner { get; set; }

    /// <summary>
    /// 工序参数集合
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(ProcProcessParameter.ProcessId))]
    public List<ProcProcessParameter>? Parameters { get; set; }
}
