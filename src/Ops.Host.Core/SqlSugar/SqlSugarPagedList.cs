namespace Ops.Host.Core;

/// <summary>
/// 分页泛型集合
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public sealed class PagedList<TEntity>
    where TEntity : new()
{
    /// <summary>
    /// 页码
    /// </summary>
    public int PageIndex { get; set; }

    /// <summary>
    /// 页容量
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// 总条数
    /// </summary>
    public int TotalCount { get; set; }

    /// <summary>
    /// 总页数
    /// </summary>
    public int TotalPages { get; set; }

    /// <summary>
    /// 当前页集合
    /// </summary>
    [NotNull]
    public List<TEntity>? Items { get; set; }

    /// <summary>
    /// 是否有上一页
    /// </summary>
    public bool HasPrevPage { get; set; }

    /// <summary>
    /// 是否有下一页
    /// </summary>
    public bool HasNextPage { get; set; }
}

/// <summary>
/// 分页拓展类
/// </summary>
public static class SqlSugarPagedExtensions
{
    /// <summary>
    /// 分页拓展
    /// </summary>
    /// <param name="queryable">Sugar Queryable</param>
    /// <param name="pageIndex">pageIndex 是从1开始</param>
    /// <param name="pageSize">页容量</param>
    /// <returns></returns>
    public static PagedList<TEntity> ToPagedList<TEntity>(this ISugarQueryable<TEntity> queryable, int pageIndex, int pageSize)
        where TEntity : new()
    {
        int total = 0, totalPage = 0;
        var items = queryable.ToPageList(pageIndex, pageSize, ref total, ref totalPage);
        return new PagedList<TEntity>
        {
            PageIndex = pageIndex,
            PageSize = pageSize,
            Items = items,
            TotalCount = total,
            TotalPages = totalPage,
            HasNextPage = pageIndex < totalPage,
            HasPrevPage = pageIndex - 1 > 0
        };
    }

    /// <summary>
    /// 分页拓展
    /// </summary>
    /// <param name="queryable">Sugar Queryable</param>
    /// <param name="pageIndex">pageIndex 是从1开始</param>
    /// <param name="pageSize">页容量</param>
    /// <returns></returns>
    public static async Task<PagedList<TEntity>> ToPagedListAsync<TEntity>(this ISugarQueryable<TEntity> queryable, int pageIndex, int pageSize)
        where TEntity : new()
    {
        RefAsync<int> total = 0, totalPage = 0;
        var items = await queryable.ToPageListAsync(pageIndex, pageSize, total, totalPage);
        return new PagedList<TEntity>
        {
            PageIndex = pageIndex,
            PageSize = pageSize,
            Items = items,
            TotalCount = total,
            TotalPages = totalPage,
            HasNextPage = pageIndex < totalPage,
            HasPrevPage = pageIndex - 1 > 0
        };
    }
}
