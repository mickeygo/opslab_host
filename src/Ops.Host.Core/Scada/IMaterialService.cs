namespace Ops.Host.Core.Services;

public interface IMaterialService
{
    Task<ReplyResult> HandleCriticalMaterialAsync(ForwardData data);

    Task<ReplyResult> HandleBactchMaterialAsync(ForwardData data);
}
