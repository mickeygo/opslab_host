namespace Ops.Host.Core.Entity;

/// <summary>
/// 产品BOM关系
/// </summary>
[SugarTable("md_product_bom", "产品BOM关系表")]
public sealed class MdProductBom : EntityBase
{
    /// <summary>
    /// 产品信息 Id
    /// </summary>
    public long ProductId { get; set; }

    /// <summary>
    /// 产品信息
    /// </summary>
    [Navigate(NavigateType.OneToOne, nameof(ProductId))]
    public MdItem? Product { get; set; }

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
    /// BOM 详细
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(MdProductBomItem.ProductBomId))]
    public List<MdProductBomItem>? Items { get; set; }
}
