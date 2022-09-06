﻿namespace Ops.Host.Core.Entity;

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
    public string? Code { get; set; }

    /// <summary>
    /// 工站名称
    /// </summary>
    [Required, MaxLength(64)]
    [NotNull]
    public string? Name { get; set; }
}