namespace Ops.Host.Core.Entity;

/// <summary>
/// 产品对应的工艺路线信息
/// </summary>
[SugarTable("proc_product_process_route", "产品对应的工艺路线表")]
public sealed class ProcProductProcessRoute : EntityBase
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
    [SugarColumn(ColumnDescription = "工艺路线 Id")]
    public long ProcessRouteId { get; set; }

    /// <summary>
    /// 工艺路线
    /// </summary>
    [Navigate(NavigateType.OneToOne, nameof(ProcessRouteId))]
    public ProcProcessRoute? ProcessRoute { get; set; }

    /// <summary>
    /// 程序配方号（PLC 程序配方）。
    /// </summary>
    [SugarColumn(ColumnDescription = "程序配方号")]
    public int FormulaNo { get; set; }
}
