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
    /// <remarks>来源于工站编码。</remarks>
    [DisplayName("工序编码")]
    [Required, MaxLength(64)]
    [NotNull]
    public string? Code { get; set; }

    /// <summary>
    /// 工序名称
    /// </summary>
    /// <remarks>来源于工站名称。</remarks>
    [DisplayName("工序名称")]
    [Required, MaxLength(64)]
    [NotNull]
    public string? Name { get; set; }

    /// <summary>
    /// 工站Id。
    /// </summary>
    /// <remarks>目前工序与工站是一一对应关系。</remarks>
    public long StationId { get; set; }

    /// <summary>
    /// 工站。
    /// </summary>
    [Navigate(NavigateType.OneToOne, nameof(StationId))]
    public MdStation? Station { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    [DisplayName("备注")]
    [MaxLength(255)]
    public string? Remark { get; set; }

    /// <summary>
    /// 是否禁用
    /// </summary>
    public bool IsDisable { get; set; }
}
