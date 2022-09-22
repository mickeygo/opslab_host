namespace Ops.Host.Core.Services;

public interface INoticeService : IDomainService
{
    Task HandleAsync(ForwardData data);
}
