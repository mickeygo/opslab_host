namespace Ops.Host.App.Models;

public class ProcProcessParamModel : ObservableObject
{
    public long Id { get; set; }

    /// <summary>
    /// 产品 Id
    /// </summary>
    public long ProductId { get; set; }

    /// <summary>
    /// 产品信息
    /// </summary>
    public MdItem? Product { get; set; }

    /// <summary>
    /// 工序 Id
    /// </summary>
    public long ProcessId { get; set; }

    /// <summary>
    /// 工序
    /// </summary>
    public ProcProcess? Process { get; set; }

    private ObservableCollection<ProcProcessParamContent>? _contents;

    /// <summary>
    /// 工艺参数详细项集合
    /// </summary>
    public ObservableCollection<ProcProcessParamContent>? Contents
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
