namespace Ops.Host.Core.Entity;

/// <summary>
/// 刷卡记录。
/// </summary>
[SugarTable("sys_card_record")]
public sealed class SysCardRecord : EntityBaseId
{
    /// <summary>
    /// 产线
    /// </summary>
    [DisplayName("产线")]
    [Required, MaxLength(64)]
    [NotNull]
    public string? LineCode { get; set; }

    /// <summary>
    /// 工站
    /// </summary>
    [DisplayName("工站")]
    [Required, MaxLength(64)]
    [NotNull]
    public string? StationCode { get; set; }

    /// <summary>
    /// 卡号
    /// </summary>
    [DisplayName("卡号")]
    [Required, MaxLength(64)]
    [NotNull]
    public string? CardNo { get; set; }

    /// <summary>
    /// 持卡人
    /// </summary>
    [DisplayName("持卡人")]
    [MaxLength(64)]
    public string? Owner { get; set; }

    /// <summary>
    /// 刷卡机名称
    /// </summary>
    [DisplayName("卡机")]
    [Required, MaxLength(64)]
    [NotNull]
    public string? CardDeviceName { get; set; }

    /// <summary>
    /// 刷卡机设备编号
    /// </summary>
    [DisplayName("卡机编号")]
    [MaxLength(64)]
    public string? CardDeviceNo { get; set; }

    /// <summary>
    /// 刷卡时间
    /// </summary>
    [DisplayName("刷卡时间")]
    public DateTime CreateTime { get; set; }
}
