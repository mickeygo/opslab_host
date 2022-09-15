namespace Ops.Host.App.Models;

public sealed class ProcProcessBomModel : ObservableObject
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
    /// 工序 Id
    /// </summary>
    public long ProcessId { get; set; }

    /// <summary>
    /// 工序
    /// </summary>
    public ProcProcess? Process { get; set; }

    private ObservableCollection<ProcProcessBomContentModel>? _contents;

    /// <summary>
    /// BOM 详细
    /// </summary>
    public ObservableCollection<ProcProcessBomContentModel>? Contents
    {
        get => _contents;
        set => SetProperty(ref _contents, value);
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

public sealed class ProcProcessBomContentModel : ObservableObject
{
    /// <summary>
    /// 工艺BOM Id。
    /// </summary>
    public long ProcessBomId { get; set; }

    /// <summary>
    /// 物料信息 Id。
    /// </summary>
    public long MaterialId { get; set; }

    /// <summary>
    /// 物料信息。
    /// </summary>
    public MdItemModel? Material { get; set; }

    private int _qty;

    /// <summary>
    /// 使用数量。
    /// </summary>
    public int Qty
    {
        get => _qty;
        set => SetProperty(ref _qty, value);
    }


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