namespace Ops.Host.Core.Services;

/// <summary>
/// 工序BOM 服务
/// </summary>
public interface IProcProcessBomService : IDomainService
{
    Task<ProcProcessBom> GetBomByIdAsync(long id);

    Task<ProcProcessBom> GetBomAsync(long productId, long processId);

    Task<ProcProcessBom> GetBomAsync(string productCode, string lineCode, string stationCode);

    Task<PagedList<ProcProcessBom>> GetPagedListAsync(ProcessBomFilter filter, int pageIndex, int pageSize);

    Task<(bool ok, string err)> InsertOrUpdateAsync(ProcProcessBom input);

    Task<(bool ok, string err)> DeleteAsync(long id);

    /// <summary>
    /// 将源产品工艺BOM复制到目标产品。
    /// </summary>
    /// <param name="sourceProductId">源产品工艺BOM Id</param>
    /// <param name="targetId">目标产品工艺BOM Id</param>
    /// <returns></returns>
    Task<(bool ok, string err)> CopyAsync(long sourceProductId, long targetProductId);
}
