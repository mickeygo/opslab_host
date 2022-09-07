namespace Ops.Host.App.Models;

public sealed class KibanaModel : ObservableObject
{
    /// <summary>
    /// 工站
    /// </summary>
    [NotNull]
    public string? Station { get; set; }

    /// <summary>
    /// 产线
    /// </summary>
    [NotNull]
    public string? Line { get; set; }

    private bool _connectedState;
    /// <summary>
    /// 设备连接状态。
    /// </summary>
    /// <remarks>用此方法设置，UI 才能随着数据通过后台更改而同步更新。</remarks>
    public bool ConnectedState
    {
        get => _connectedState;
        set { SetProperty(ref _connectedState, value); }
    }
}
