namespace Ops.Host.Core.Services;

/// <summary>
/// 用户信息接口
/// </summary>
public interface IUserService : IDomainService
{
    SysUser GetUserById(int id);

    SysUser GetUserByName(string userName);

    List<SysUser> GetAllUsers();

    PagedList<SysUser> GetPaged(UserFilter filter, int pageIndex, int pageItems);

    Task<PagedList<SysUser>> GetPagedAsync(UserFilter filter, int pageIndex, int pageItems);
}
