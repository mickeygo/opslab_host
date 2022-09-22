namespace Ops.Host.Shared.Options;

public sealed class OpsHostOptions
{
    /// <summary>
    /// 软件标题名称
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// 是否程序启动后自动开始运行。
    /// </summary>
    public bool AutoRunning { get; set; }

    /// <summary>
    /// 是否启用前端代理。
    /// </summary>
    /// <remarks>启用前端代理后，部分数据会通过 MQ Pub/Sub 模式通知代理端。代理端仅能收到消息并显示，不能反向操作。</remarks>
    public bool EnableFrontAgent { get; set; }
}
