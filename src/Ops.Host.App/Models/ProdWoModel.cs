namespace Ops.Host.App.Models;

public class ProdWoModel : ObservableObject
{
    public long Id { get; set; }

    /// <summary>
    /// 工单编码
    /// </summary>
    [DisplayName("工单编码")]
    [Required, MaxLength(64)]
    [NotNull]
    public string? Code { get; set; }

    /// <summary>
    /// 工单名称
    /// </summary>
    [DisplayName("工单名称")]
    [Required, MaxLength(64)]
    [NotNull]
    public string? Name { get; set; }

    /// <summary>
    /// 工单来源，如 ERP、LOCAL
    /// </summary>
    [DisplayName("工单来源")]
    [Required, MaxLength(16)]
    [NotNull]
    public string? Source { get; set; }

    /// <summary>
    /// 来源单据。
    /// </summary>
    [DisplayName("来源单据")]
    [MaxLength(32)]
    public string? SourceOrder { get; set; }

    /// <summary>
    /// 工单类型
    /// </summary>
    public WoTypeEnum WoType { get; set; } = WoTypeEnum.Official;

    /// <summary>
    /// 产品信息Id
    /// </summary>
    public long ProductId { get; set; }

    /// <summary>
    /// 产品信息
    /// </summary>
    public MdItem? Product { get; set; }

    /// <summary>
    /// 投入数量（Qty = CompletedQty + ScrappedQty + DismantlingQty）
    /// </summary>
    public int Qty { get; set; }

    /// <summary>
    /// 已上线数量
    /// </summary>
    public int OnlineQty { get; set; }

    /// <summary>
    /// 已完工（下线）数量
    /// </summary>
    public int CompletedQty { get; set; }

    /// <summary>
    /// 报废数量
    /// </summary>
    public int ScrappedQty { get; set; }

    /// <summary>
    /// 拆解数量（脱离工单数量，用于尾数处理）
    /// </summary>
    public int DismantlingQty { get; set; }

    /// <summary>
    /// 计划开始时间
    /// </summary>
    [DisplayName("计划开始时间")]
    [Required]
    public DateTime? PlanStartDate { get; set; }

    /// <summary>
    /// 计划结束时间
    /// </summary>
    [DisplayName("计划结束时间")]
    [Required]
    public DateTime? PlanEndDate { get; set; }

    /// <summary>
    /// 生产实际开始时间
    /// </summary>
    public DateTime? ActualStartDate { get; set; }

    /// <summary>
    /// 实际结束时间
    /// </summary>
    public DateTime? ActualEndDate { get; set; }

    /// <summary>
    /// 上一次工单状态，当工单暂停后再恢复时使用。
    /// </summary>
    public WoStatusEnum LastStatus { get; set; }

    private WoStatusEnum _status = WoStatusEnum.Created;

    /// <summary>
    /// 单据当前状态
    /// </summary>
    public WoStatusEnum Status
    {
        get => _status;
        set => SetProperty(ref _status, value);
    }

    /// <summary>
    /// 备注
    /// </summary>
    [MaxLength(255)]
    public string? Remark { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    [DisplayName("创建时间")]
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 更新时间
    /// </summary>
    [DisplayName("更新时间")]
    public DateTime? UpdateTime { get; set; }
}
