namespace Ops.Host.Core.Services;

/// <summary>
/// 产品 BOM 服务
/// </summary>
public interface IMdProductBomService : IDomainService
{
    Task<MdProductBom> GetBomByIdAsync(long id);

    Task<MdProductBom> GetBomByProductIdAsync(long productId);

    Task<MdProductBom> GetBomByProductCodeAsync(string productCode);

    Task<PagedList<MdProductBom>> GetPagedListAsync(ProductBomFilter filter, int pageIndex, int pageSize);

    Task<(bool ok, string err)> InsertOrUpdateAsync(MdProductBom input);

    Task<(bool ok, string err)> DeleteAsync(long id);
}
