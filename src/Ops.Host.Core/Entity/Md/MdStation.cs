namespace Ops.Host.Core.Entity;

/// <summary>
/// 工站信息
/// </summary>
[SugarTable("md_station", "工站信息表")]
public sealed class MdStation : EntityBase
{
    /// <summary>
    /// 产线编码
    /// </summary>
    [Required, MaxLength(64)]
    [NotNull]
    public string? LineCode { get; set; }

    /// <summary>
    /// 产线名称
    /// </summary>
    [Required, MaxLength(64)]
    [NotNull]
    public string? LineName { get; set; }

    /// <summary>
    /// 工站编码
    /// </summary>
    [Required, MaxLength(64)]
    [NotNull]
    public string? StationCode { get; set; }

    /// <summary>
    /// 工站名称
    /// </summary>
    [Required, MaxLength(64)]
    [NotNull]
    public string? StationName { get; set; }

    /// <summary>
    /// 工站类型。
    /// </summary>
    public StationTypeEnum Type { get; set; } = StationTypeEnum.Assembly;

    /// <summary>
    /// 工站归属。
    /// </summary>
    /// <remarks>线外站没有工艺路线。</remarks>
    public StationOwnerEnum Owner { get; set; } = StationOwnerEnum.Inline;

    /// <summary>
    /// 额外标志
    /// </summary>
    [MaxLength(255)]
    public string? ExtraFlag { get; set; }

    /// <summary>
    /// 设备信息，会序列化为 Json 存储。
    /// </summary>
    [SugarColumn(IsJson = true)]
    public DeviceInfo? DeviceInfoExt { get; set; }
}
