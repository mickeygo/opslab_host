namespace Ops.Host.App.Models;

public class MdProductBomModel : ObservableObject
{
    public long Id { get; set; }

    /// <summary>
    /// 产品信息 Id
    /// </summary>
    public long ProductId { get; set; }

    /// <summary>
    /// 产品信息
    /// </summary>
    public MdItemModel? Product { get; set; }

    /// <summary>
    /// 产线
    /// </summary>
    [DisplayName("产线")]
    [Required]
    public string? LineCode { get; set; }

    /// <summary>
    /// 工站
    /// </summary>
    [DisplayName("工站")]
    [Required]
    public string? StationCode { get; set; }

    private ObservableCollection<MdProductBomItemModel>? _items;

    /// <summary>
    /// BOM 详细
    /// </summary>
    public ObservableCollection<MdProductBomItemModel>? Items
    {
        get => _items;
        set => SetProperty(ref _items, value);
    }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 更新时间
    /// </summary>
    public DateTime? UpdateTime { get; set; }
}

public class MdProductBomItemModel : ObservableObject
{
    /// <summary>
    /// 产品 BOM Id。
    /// </summary>
    public long ProductBomId { get; set; }

    /// <summary>
    /// 产品 BOM。
    /// </summary>
    public MdProductBomModel? ProductBom { get; set; }

    /// <summary>
    /// 物料信息 Id。
    /// </summary>
    public long MaterialId { get; set; }

    /// <summary>
    /// 物料信息。
    /// </summary>
    public MdItemModel? Material { get; set; }

    /// <summary>
    /// 使用数量。
    /// </summary>
    public int Qty { get; set; }

    private int _seq;

    /// <summary>
    /// 上料顺序号。
    /// </summary>
    public int Seq
    {
        get => _seq;
        set => SetProperty(ref _seq, value);
    }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 更新时间
    /// </summary>
    public DateTime? UpdateTime { get; set; }
}
