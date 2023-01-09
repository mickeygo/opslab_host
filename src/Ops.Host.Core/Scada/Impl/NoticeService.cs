namespace Ops.Host.Core.Services;

internal sealed class NoticeService : ScadaDomainService, INoticeService
{
    private readonly ILogger _logger;

    public NoticeService(ILogger<NoticeService> logger)
    {
        _logger = logger;
    }

    public Task HandleAsync(ForwardData data)
    {
        try
        {
            if (data.Tag == "PLC_Custom_Heartbeat")
            {
                var ret0 = data.Self().GetBit();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[OpsUnderlyForwarder] Handle Error");
        }

        return Task.CompletedTask;
    }
}
