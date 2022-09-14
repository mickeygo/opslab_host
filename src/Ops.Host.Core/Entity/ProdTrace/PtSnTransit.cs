namespace Ops.Host.Core.Entity;

/// <summary>
/// SN 状态信息。
/// </summary>
/// <remarks>该表汇总 SN相关信息，此表中 SN 数据应该唯一。</remarks>
[SugarTable("pt_sn_transit", "SN过站状态表")]
public sealed class PtSnTransit : EntityBaseId
{
    /// <summary>
    /// SN
    /// </summary>
    [SugarColumn(ColumnDescription = "SN", Length = 64)]
    [Required, MaxLength(64)]
    [NotNull]
    public string? SN { get; set; }

    /// <summary>
    /// 产品
    /// </summary>
    [SugarColumn(ColumnDescription = "产品", Length = 64)]
    [Description("产品")]
    [MaxLength(64)]
    public string? ProductCode { get; set; }

    /// <summary>
    /// 工单号
    /// <para>工单可以不存在。</para>
    /// </summary>
    [SugarColumn(ColumnDescription = "工单号", Length = 64)]
    [MaxLength(64)]
    public string? WO { get; set; }

    /// <summary>
    /// 产线代码
    /// </summary>
    [SugarColumn(ColumnDescription = "产线代码", Length = 64)]
    [Required, MaxLength(64)]
    [NotNull]
    public string? LineCode { get; set; }

    /// <summary>
    /// 当前工站
    /// </summary>
    [SugarColumn(ColumnDescription = "当前工站", Length = 64)]
    [Required, MaxLength(64)]
    [NotNull]
    public string? StationCode { get; set; }

    /// <summary>
    /// 过站状态
    /// </summary>
    [DisplayName("过站状态")]
    public TransitModeEnum TransitMode { get; set; }

    /// <summary>
    /// 过站状态
    /// </summary>
    [SugarColumn(ColumnDescription = "过站状态")]
    public PassEnum? Pass { get; set; }

    /// <summary>
    /// 产品状态
    /// </summary>
    public ProductStatusEnum ProductStatus { get; set; } = ProductStatusEnum.OK;

    /// <summary>
    /// 当前站进站时间
    /// </summary>
    [SugarColumn(ColumnDescription = "进站时间")]
    public DateTime? InboundTime { get; set; }

    /// <summary>
    /// 当前站出站时间
    /// </summary>
    [SugarColumn(ColumnDescription = "出站时间")]
    public DateTime? OutboundTime { get; set; }

    /// <summary>
    /// NG 原因
    /// </summary>
    [SugarColumn(ColumnDescription = "NG 原因", Length = 255)]
    [MaxLength(255)]
    public string? NGReason { get; set; }

    /// <summary>
    /// 返修次数
    /// </summary>
    public int ReworkCount { get; set; }

    /// <summary>
    /// 上线时间
    /// </summary>
    public DateTime? OnlineTime { get; set; }

    /// <summary>
    /// 完工/下线时间
    /// </summary>
    public DateTime? CompletedTime { get; set; }

    /// <summary>
    /// 是否是 OK
    /// </summary>
    /// <returns></returns>
    public bool IsOK()
    {
        return Pass is PassEnum.OK or PassEnum.ForceOK;
    }

    /// <summary>
    /// 是否是 NG
    /// </summary>
    /// <returns></returns>
    public bool IsNG()
    {
        return Pass is PassEnum.NG or PassEnum.ForceNG;
    }

    /// <summary>
    /// 设置产品状态
    /// </summary>
    public void SetProductStatus()
    {
        ProductStatus = IsOK() ? ProductStatusEnum.OK : ProductStatusEnum.NG;
    }
}
