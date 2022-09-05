namespace Ops.Host.Core.Services;

internal sealed class CustomService : ScadaDomainService, ICustomService
{
    public Task<ReplyResult> HandleAsync(ForwardData data)
    {
        return Task.FromResult(Ok());
    }
}
