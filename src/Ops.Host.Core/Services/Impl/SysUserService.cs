namespace Ops.Host.Core.Services.Impl;

internal sealed class SysUserService : ISysUserService
{
    private readonly SqlSugarRepository<SysUser> _userRep;

    public SysUserService(SqlSugarRepository<SysUser> userRep)
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

    public PagedList<SysUser> GetPagedList(UserFilter filter, int pageIndex, int pageSize)
    {
        return _userRep.AsQueryable()
            .WhereIF(!string.IsNullOrEmpty(filter.UserName), s => s.UserName == filter.UserName)
            .WhereIF(filter.CreateTimeStart != null, s => s.CreateTime >= filter.CreateTimeStart.ToDayMin())
            .WhereIF(filter.CreateTimeEnd != null, s => s.CreateTime <= filter.CreateTimeEnd.ToDayMax())
            .ToPagedList(pageIndex, pageSize);
    }

    public async Task<PagedList<SysUser>> GetPagedListAsync(UserFilter filter, int pageIndex, int pageSize)
    {
        return await _userRep.AsQueryable()
           .WhereIF(!string.IsNullOrEmpty(filter.UserName), s => s.UserName == filter.UserName)
           .WhereIF(filter.CreateTimeStart != null, s => s.CreateTime >= filter.CreateTimeStart.ToDayMin())
           .WhereIF(filter.CreateTimeEnd != null, s => s.CreateTime <= filter.CreateTimeEnd.ToDayMax())
           .ToPagedListAsync(pageIndex, pageSize);
    }

    public bool InsertOrUpdateUser(SysUser input)
    {
        return _userRep.InsertOrUpdate(input);
    }

    public bool DeleteUser(SysUser input)
    {
        return _userRep.DeleteById(input.Id);
    }
}
