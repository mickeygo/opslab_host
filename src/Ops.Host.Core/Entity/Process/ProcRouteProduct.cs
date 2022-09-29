namespace Ops.Host.Core.Entity;

/// <summary>
/// 产品制程（产品对应的工艺路线）
/// </summary>
[SugarTable("proc_route_product", "产品制程表")]
public sealed class ProcRouteProduct : EntityBase
{
    /// <summary>
    /// 产品 Id
    /// </summary>
    [SugarColumn(ColumnDescription = "产品Id")]
    public long ProductId { get; set; }

    /// <summary>
    /// 产品信息
    /// </summary>
    [Navigate(NavigateType.OneToOne, nameof(ProductId))]
    public MdItem? Product { get; set; }

    /// <summary>
    /// 工艺路线 Id
    /// </summary>
    [SugarColumn(ColumnDescription = "工艺路线Id")]
    public long RouteId { get; set; }

    /// <summary>
    /// 工艺路线
    /// </summary>
    [Navigate(NavigateType.OneToOne, nameof(RouteId))]
    public ProcRoute? Route { get; set; }

    /// <summary>
    /// 程序配方号（PLC 程序配方）。
    /// </summary>
    /// <remarks>产品:配方 => N:1</remarks>
    [SugarColumn(ColumnDescription = "程序配方号")]
    public int FormulaNo { get; set; }
}
