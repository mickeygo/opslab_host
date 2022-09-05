namespace Ops.Host.Core.Services;

internal sealed class NoticeService : INoticeService
{
    public Task HandleAsync(ForwardData data)
    {
        return Task.CompletedTask;
    }
}
