using Ops.Exchange.Forwarder;

namespace Ops.Host.Core.Services;

internal sealed class NoticeService : INoticeService
{
    private readonly IFreeSql _freeSql;

    public NoticeService(IFreeSql freeSql) => _freeSql = freeSql;

    public Task HandleAsync(ForwardData data)
    {
        return Task.CompletedTask;
    }
}
