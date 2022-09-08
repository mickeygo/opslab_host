namespace Ops.Host.Core.Entity;

/// <summary>
/// 产品SOP
/// </summary>
[SugarTable("proc_process_sop", "产品SOP表")]
public sealed class ProcProcessSop : EntityBase
{
    /// <summary>
    /// 产品 Id
    /// </summary>
    [SugarColumn(ColumnDescription = "产品Id")]
    public long ProductId { get; set; }

    /// <summary>
    /// 产品信息
    /// </summary>
    [Navigate(NavigateType.OneToOne, nameof(ProductId))]
    public MdItem? Product { get; set; }

    /// <summary>
    /// 工序 Id
    /// </summary>
    [SugarColumn(ColumnDescription = "工序Id")]
    public long ProcessId { get; set; }

    /// <summary>
    /// 工序
    /// </summary>
    [Navigate(NavigateType.OneToOne, nameof(ProcessId))]
    public ProcProcess? Process { get; set; }

    /// <summary>
    /// SOP 标题
    /// </summary>
    [DisplayName("标题")]
    [Required, MaxLength(128)]
    [NotNull]
    public string? Title { get; set; }

    /// <summary>
    /// SOP 描述
    /// </summary>
    [DisplayName("描述")]
    [Required, MaxLength(255)]
    [NotNull]
    public string? Description { get; set; }

    /// <summary>
    /// SOP 资源地址
    /// </summary>
    [DisplayName("资源地址")]
    [Required, MaxLength(255)]
    [NotNull]
    public string? Url { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    [SugarColumn(ColumnDescription = "排序")]
    public int Order { get; set; }
}
