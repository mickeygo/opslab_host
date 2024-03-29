﻿namespace Ops.Host.Core.Entity;

/// <summary>
/// 工艺路线信息。
/// </summary>
[SugarTable("proc_route", "工艺路线表")]
public sealed class ProcRoute : EntityBase
{
    /// <summary>
    /// 工艺路线编码
    /// </summary>
    [DisplayName("工艺路线编码")]
    [SugarColumn(ColumnDescription = "工艺路线编码", Length = 64)]
    [Required, MaxLength(64)]
    [NotNull]
    public string? Code { get; set; }

    /// <summary>
    /// 工艺路线名称
    /// </summary>
    [DisplayName("工艺路线名称")]
    [SugarColumn(ColumnDescription = "工艺路线名称", Length = 64)]
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
    /// 启用状态
    /// </summary>
    public StatusEnum Status { get; set; } = StatusEnum.Enable;

    /// <summary>
    /// 工序详细信息。
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(ProcRouteProcess.RouteId))]
    public List<ProcRouteProcess>? Contents { get; set; }

    /// <summary>
    /// 路由关联的产品。
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(ProcRouteProduct.RouteId))]
    public List<ProcRouteProduct>? LinkProducts { get; set; }
}
