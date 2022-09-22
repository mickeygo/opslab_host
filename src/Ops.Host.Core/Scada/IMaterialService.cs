namespace Ops.Host.Core.Services;

/// <summary>
/// SCADA 扫入物料服务。
/// </summary>
public interface IMaterialService : IDomainService
{
    /// <summary>
    /// 扫入关键物料。
    /// </summary>
    /// <returns></returns>
    Task<ReplyResult> HandleCriticalMaterialAsync(ForwardData data);

    /// <summary>
    /// 扫入批次料。
    /// </summary>
    /// <returns></returns>
    Task<ReplyResult> HandleBactchMaterialAsync(ForwardData data);
}
