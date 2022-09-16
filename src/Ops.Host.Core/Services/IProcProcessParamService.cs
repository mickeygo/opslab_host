namespace Ops.Host.Core.Services;

/// <summary>
/// 工艺参数服务。
/// </summary>
public interface IProcProcessParamService : IDomainService
{
    Task<PagedList<ProcProcessParam>> GetPagedListAsync(ProcessParamFilter filter, int pageIndex, int pageSize);

    Task<(bool ok, string err)> InsertOrUpdateAsync(ProcProcessParam input);

    /// <summary>
    /// 生成工参模板。
    /// </summary>
    /// <param name="productId">产品 Id</param>
    /// <param name="processId">工序 Id</param>
    /// <returns></returns>
    Task<(bool ok, ProcProcessParam? content, string? err)> GenerateTemplateAsync(long productId, long processId);
}
