﻿namespace Ops.Host.Core.Entity;

/// <summary>
/// 产品/物料信息
/// </summary>
[SugarTable("md_item", "物料产品表")]
public sealed class MdItem : EntityBase
{
    /// <summary>
    /// 产品/物料编码
    /// </summary>
    [DisplayName("物料编码")]
    [Required, MaxLength(64)]
    [NotNull]
    public string? Code { get; set; }

    /// <summary>
    /// 产品/物料名称
    /// </summary>
    [DisplayName("物料名称")]
    [Required, MaxLength(64)]
    [NotNull]
    public string? Name { get; set; }

    /// <summary>
    /// 规格型号
    /// </summary>
    [DisplayName("规格型号")]
    [MaxLength(128)]
    public string? Spec { get; set; }

    /// <summary>
    /// 物料属性。
    /// </summary>
    public MaterialAttrEnum Attr { get; set; } = MaterialAttrEnum.Critical;

    /// <summary>
    /// 条码规则，多个以逗号分隔。
    /// </summary>
    [DisplayName("条码规则")]
    [Required, MaxLength(512)]
    [NotNull]
    public string? BarcodeRule { get; set; }

    /// <summary>
    /// 保质期（天）
    /// </summary>
    [DisplayName("保质期")]
    public int? Expiration { get; set; }
}
