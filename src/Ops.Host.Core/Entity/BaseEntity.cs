namespace Ops.Host.Core.Entity;

/// <summary>
/// 实体基类。
/// </summary>
public abstract class BaseEntity
{
    /// <summary>
    /// 主键。
    /// </summary>
    /// <remarks>为了方便迁移，主键不设置为自增增长。</remarks>
    [SugarColumn(IsPrimaryKey = true, IsIdentity = false)]
    public long Id { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    [DisplayName("创建时间")]
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 更新时间
    /// </summary>
    [DisplayName("更新时间")]
    public DateTime? UpdateTime { get; set; }

    public bool IsTransient()
    {
        return Id <= 0;
    }
}
