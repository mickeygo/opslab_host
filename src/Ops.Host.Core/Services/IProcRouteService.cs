namespace Ops.Host.Core.Services;

/// <summary>
/// 工艺路线服务。
/// </summary>
public interface IProcRouteService : IDomainService
{
    Task<List<ProcRoute>> GetAsync(long id);

    List<ProcRoute> GetAll();

    Task<List<ProcRoute>> GetAllAsync();

    Task<PagedList<ProcRoute>> GetPagedListAsync(ProcRouteFilter filter, int pageIndex, int pageSize);

    /// <summary>
    /// 新增或更新工艺路由，其中不包含关联的产品。
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<(bool ok, string err)> InsertOrUpdateAsync(ProcRoute input);

    /// <summary>
    /// 删除数据，包括关联的明细项和关联的产品。
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<(bool ok, string err)> DeleteAsync(long id);

    /// <summary>
    /// 关联产品。
    /// </summary>
    /// <param name="input">要关联的产品</param>
    /// <returns></returns>
    Task<(bool ok, string err)> LinkProductAsync(ProcRouteProduct input);

    /// <summary>
    /// 删除关联的产品。
    /// </summary>
    /// <param name="routeId">工艺路线 Id</param>
    /// <param name="productId">产品 Id</param>
    /// <returns></returns>
    Task<(bool ok, string err)> DelLinkProductAsync(long routeId, long productId);
}
