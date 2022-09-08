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
    [DisplayName("工序编码")]
    [Required, MaxLength(64)]
    [NotNull]
    public string? Code { get; set; }

    /// <summary>
    /// 工序名称
    /// </summary>
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
    /// 工序状态
    /// </summary>
    public StatusEnum Status { get; set; } = StatusEnum.Enable;

    /// <summary>
    /// 工序参数集合
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(ProcProcessParameter.ProcessId))]
    public List<ProcProcessParameter>? ProcessParameters { get; set; }
}
