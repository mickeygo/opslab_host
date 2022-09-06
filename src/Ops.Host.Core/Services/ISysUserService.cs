namespace Ops.Host.Core.Services;

/// <summary>
/// 用户信息接口
/// </summary>
public interface ISysUserService : IDomainService
{
    SysUser GetUserById(int id);

    SysUser GetUserByName(string userName);

    List<SysUser> GetAllUsers();

    PagedList<SysUser> GetPagedList(UserFilter filter, int pageIndex, int pageSize);

    Task<PagedList<SysUser>> GetPagedListAsync(UserFilter filter, int pageIndex, int pageSize);

    bool InsertOrUpdateUser(SysUser input);

    bool DeleteUser(SysUser input);
}
