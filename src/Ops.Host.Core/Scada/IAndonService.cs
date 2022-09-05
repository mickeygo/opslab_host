namespace Ops.Host.Core.Services;

public interface IAndonService
{
    Task HandleAsync(ForwardData data);
}
