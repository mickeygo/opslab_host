namespace Ops.Host.Core.Services;

internal sealed class AndonService : ScadaDomainService, IAndonService
{
    public Task HandleAsync(ForwardData data)
    {
        return Task.CompletedTask;
    }
}
