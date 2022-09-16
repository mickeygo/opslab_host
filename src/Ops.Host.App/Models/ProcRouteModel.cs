namespace Ops.Host.App.Models;

public sealed class ProcRouteModel : ObservableObject
{
    public long Id { get; set; }

    /// <summary>
    /// 工艺路线编码
    /// </summary>
    [DisplayName("工艺路线编码")]
    [Required, MaxLength(64)]
    [NotNull]
    public string? Code { get; set; }

    /// <summary>
    /// 工艺路线名称
    /// </summary>
    [DisplayName("工艺路线名称")]
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

    private ObservableCollection<ProcRouteProcessModel>? _contents;

    /// <summary>
    /// 工序详细信息。
    /// </summary>
    public ObservableCollection<ProcRouteProcessModel>? Contents
    {
        get => _contents;
        set => SetProperty(ref _contents, value);
    }

    private ObservableCollection<ProcRouteProduct>? _linkProducts;
    public ObservableCollection<ProcRouteProduct>? LinkProducts
    {
        get => _linkProducts;
        set => SetProperty(ref _linkProducts, value);
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

public class ProcRouteProcessModel : ObservableObject
{
    public long Id { get; set; }

    /// <summary>
    /// 工艺路线 Id
    /// </summary>
    public long RouteId { get; set; }

    /// <summary>
    /// 工序 Id
    /// </summary>
    public long ProcessId { get; set; }

    /// <summary>
    /// 当前工序
    /// </summary>
    public ProcProcess? Process { get; set; }

    /// <summary>
    /// 下一道工序 Id。
    /// </summary>
    /// <remarks>null 表示没有下一道工序。</remarks>
    public long? NextProcessId { get; set; }

    /// <summary>
    /// 下一道工序
    /// </summary>
    public ProcProcess? NextProcess { get; set; }

    private int _seq;

    /// <summary>
    /// 顺序。
    /// </summary>
    public int Seq
    {
        get => _seq;
        set => SetProperty(ref _seq, value);
    }

    /// <summary>
    /// 与下一工序的关系。
    /// </summary>
    public FlowRelationshipEnum Relationship { get; set; }
}
