namespace Ops.Host.Core.Entity;

/// <summary>
/// 字典数据
/// </summary>
[SugarTable("sys_dict_data", "业务字典数据表")]
public class SysDictData : EntityBase
{
    /// <summary>
    /// 分类编码
    /// </summary>
    [DisplayName("分类编码")]
    [Required, MaxLength(64)]
    [NotNull]
    public string? Code { get; set; }

    /// <summary>
    /// 类型描述
    /// </summary>
    [DisplayName("类型描述")]
    [Required, MaxLength(64)]
    public string? CodeDesc { get; set; }

    /// <summary>
    /// 字典名称
    /// </summary>
    [DisplayName("字典名称")]
    [Required, MaxLength(64)]
    [NotNull]
    public string? Name { get; set; }

    /// <summary>
    /// 字典使用值
    /// </summary>
    [DisplayName("字典值")]
    [Required, MaxLength(128)]
    [NotNull]
    public string? Value { get; set; }

    /// <summary>
    /// 是否固定，0->不固定，1->固定，固定的字典不提供编辑功能。
    /// </summary>
    public int Fixed { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    [DisplayName("备注")]
    [MaxLength(128)]
    public string? Remark { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    [SugarColumn(ColumnDescription = "状态")]
    public StatusEnum Status { get; set; } = StatusEnum.Enable;
}
