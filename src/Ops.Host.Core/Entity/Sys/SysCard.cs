namespace Ops.Host.Core.Entity;

/// <summary>
/// 卡号信息
/// </summary>
[SugarTable("sys_card")]
public sealed class SysCard : EntityBase
{
    /// <summary>
    /// 卡号
    /// </summary>
    [DisplayName("卡号")]
    [Required, MaxLength(64)]
    [NotNull]
    public string? CardNo { get; set; }

    /// <summary>
    /// 卡片等级
    /// </summary>
    [DisplayName("卡片等级")]
    public CardLevelEnum CardLevel { get; set; } = CardLevelEnum.L1;

    /// <summary>
    /// 持卡人
    /// </summary>
    [DisplayName("持卡人")]
    [MaxLength(64)]
    public string? Owner { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    [DisplayName("备注")]
    [MaxLength(255)]
    public string? Remark { get; set; }

    /// <summary>
    /// 启动状态。
    /// </summary>
    public StatusEnum Status { get; set; } = StatusEnum.Enable;
}
