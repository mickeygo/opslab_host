﻿namespace Ops.Host.Core.Entity;

/// <summary>
/// 生产工单。
/// <para></para>
/// </summary>
[SugarTable("prod_workorder", "生产工单表")]
[SugarIndex("index_prod_workorder_code", nameof(Code), OrderByType.Asc)]
public class ProdWo : EntityBase
{
    /// <summary>
    /// 工单编码
    /// </summary>
    [DisplayName("工单编码")]
    [SugarColumn(ColumnDescription = "工单编码", Length = 64)]
    [Required, MaxLength(64)]
    [NotNull]
    public string? Code { get; set; }

    /// <summary>
    /// 工单名称
    /// </summary>
    [DisplayName("工单名称")]
    [SugarColumn(ColumnDescription = "工单名称", Length = 64)]
    [Required, MaxLength(64)]
    [NotNull]
    public string? Name { get; set; }

    /// <summary>
    /// 工单来源，如 ERP、LOCAL
    /// </summary>
    [DisplayName("工单来源")]
    [SugarColumn(ColumnDescription = "工单来源", Length = 16)]
    [Required, MaxLength(16)]
    [NotNull]
    public string? Source { get; set; }

    /// <summary>
    /// 来源单据。
    /// </summary>
    [DisplayName("来源单据")]
    [SugarColumn(ColumnDescription = "来源单据", Length = 32)]
    [MaxLength(32)]
    public string? SourceOrder { get; set; }

    /// <summary>
    /// 工单类型
    /// </summary>
    public WoTypeEnum WoType { get; set; } = WoTypeEnum.Official;

    /// <summary>
    /// 产品信息Id
    /// </summary>
    [SugarColumn(ColumnDescription = "产品信息Id")]
    public long ProductId { get; set; }

    /// <summary>
    /// 产品信息
    /// </summary>
    [Navigate(NavigateType.OneToOne, nameof(ProductId))]
    public MdItem? Product { get; set; }

    /// <summary>
    /// 投入数量（Qty = CompletedQty + ScrappedQty + DismantlingQty）
    /// </summary>
    [SugarColumn(ColumnDescription = "投入数量")]
    public int Qty { get; set; }

    /// <summary>
    /// 已上线数量
    /// </summary>
    [SugarColumn(ColumnDescription = "已上线数量")]
    public int OnlineQty { get; set; }

    /// <summary>
    /// 已完工（下线）数量
    /// </summary>
    [SugarColumn(ColumnDescription = "已完工数量")]
    public int CompletedQty { get; set; }

    /// <summary>
    /// 报废数量
    /// </summary>
    [SugarColumn(ColumnDescription = "报废数量")]
    public int ScrappedQty { get; set; }

    /// <summary>
    /// 拆解数量（脱离工单数量，用于尾数处理）
    /// </summary>
    [SugarColumn(ColumnDescription = "拆解数量")]
    public int DismantlingQty { get; set; }

    /// <summary>
    /// 计划开始时间
    /// </summary>
    [DisplayName("计划开始时间")]
    [SugarColumn(ColumnDescription = "计划开始时间")]
    [Required]
    public DateTime? PlanStartDate { get; set; }

    /// <summary>
    /// 计划结束时间
    /// </summary>
    [DisplayName("计划结束时间")]
    [SugarColumn(ColumnDescription = "计划结束时间")]
    [Required]
    public DateTime? PlanEndDate { get; set; }

    /// <summary>
    /// 生产实际开始时间
    /// </summary>
    [SugarColumn(ColumnDescription = "实际开始时间")]
    public DateTime? ActualStartDate { get; set; }

    /// <summary>
    /// 实际结束时间
    /// </summary>
    [SugarColumn(ColumnDescription = "实际结束时间")]
    public DateTime? ActualEndDate { get; set; }

    /// <summary>
    /// 上一次工单状态，当工单暂停后再恢复时使用。
    /// </summary>
    [SugarColumn(ColumnDescription = "上一次工单状态")]
    public WoStatusEnum LastStatus { get; set; }

    /// <summary>
    /// 单据当前状态
    /// </summary>
    [SugarColumn(ColumnDescription = "单据当前状态")]
    public WoStatusEnum Status { get; set; } = WoStatusEnum.Created;

    /// <summary>
    /// 备注
    /// </summary>
    [SugarColumn(ColumnDescription = "备注", Length = 255)]
    [MaxLength(255)]
    public string? Remark { get; set; }
}
