namespace Ops.Host.Core.Entity;

/// <summary>
/// 用户信息
/// </summary>
[Table("User")]
public sealed class SysUser : BaseEntity
{
    /// <summary>
    /// 用户名
    /// </summary>
    [NotNull]
    [DisplayName("用户名")]
    public string? UserName { get; set; }

    /// <summary>
    /// 密码
    /// </summary>
    [NotNull]
    [DisplayName("密码")]
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
