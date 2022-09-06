namespace Ops.Host.Core.Entity;

/// <summary>
/// 用户信息
/// </summary>
[SugarTable("sys_user")]
public sealed class SysUser : EntityBase
{
    /// <summary>
    /// 用户名
    /// </summary>
    [DisplayName("用户名")]
    [NotNull]
    public string? UserName { get; set; }

    /// <summary>
    /// 密码
    /// </summary>
    [DisplayName("密码")]
    [NotNull]
    public string? Password { get; set; }

    /// <summary>
    /// 显示名
    /// </summary>
    [DisplayName("显示名")]
    public string? DisplayName { get; set; }

    /// <summary>
    /// 角色
    /// </summary>
    [DisplayName("角色")]
    public int Role { get; set; }
}
