namespace Ops.Host.Core.Services;

internal sealed class NoticeService : ScadaDomainService, INoticeService
{
    public Task HandleAsync(ForwardData data)
    {
        return Task.CompletedTask;
    }
}
