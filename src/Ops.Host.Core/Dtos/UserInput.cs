namespace Ops.Host.Core.Dtos;

/// <summary>
/// 用户信息筛选对象。
/// </summary>
public sealed class UserFilter
{
    public string? UserName { get; set; }

    public DateTime? CreateTimeStart { get; set; }

    public DateTime? CreateTimeEnd { get; set; }
}
