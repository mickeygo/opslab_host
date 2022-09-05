namespace Ops.Host.Core.Services.Impl;

internal sealed class UserService : IUserService
{
    private readonly SqlSugarRepository<SysUser> _userRep;

    public UserService(SqlSugarRepository<SysUser> userRep)
    {
        _userRep = userRep;
    }

    public SysUser GetUserById(int id)
    {
        return _userRep.GetById(id);
    }

    public SysUser GetUserByName(string userName)
    {
        return _userRep.GetFirst(s => s.UserName == userName);
    }

    public List<SysUser> GetAllUsers()
    {
        return _userRep.GetList();
    }

    public PagedList<SysUser> GetPaged(UserFilter filter, int pageIndex, int pageItems)
    {
        return _userRep.AsQueryable()
            .WhereIF(!string.IsNullOrEmpty(filter.UserName), s => s.UserName == filter.UserName)
            .WhereIF(filter.CreateTimeStart != null, s => s.CreateTime >= filter.CreateTimeStart.ToDayMin())
            .WhereIF(filter.CreateTimeEnd != null, s => s.CreateTime <= filter.CreateTimeEnd.ToDayMax())
            .ToPagedList(pageIndex, pageItems);
    }

    public async Task<PagedList<SysUser>> GetPagedAsync(UserFilter filter, int pageIndex, int pageItems)
    {
        return await _userRep.AsQueryable()
           .WhereIF(!string.IsNullOrEmpty(filter.UserName), s => s.UserName == filter.UserName)
           .WhereIF(filter.CreateTimeStart != null, s => s.CreateTime >= filter.CreateTimeStart.ToDayMin())
           .WhereIF(filter.CreateTimeEnd != null, s => s.CreateTime <= filter.CreateTimeEnd.ToDayMax())
           .ToPagedListAsync(pageIndex, pageItems);
    }
}
