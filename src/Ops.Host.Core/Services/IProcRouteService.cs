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

    Task<(bool ok, string err)> InsertOrUpdateAsync(ProcRoute input);

    Task<(bool ok, string err)> DeleteAsync(long id);
}
