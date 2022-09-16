namespace Ops.Host.Core.Dtos;

public class MdItemInput
{
}

public class MdItemFilter
{
    /// <summary>
    /// 产品/物料编码
    /// </summary>
    public string? Code { get; set; }

    /// <summary>
    /// 产品/物料名称
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// 物料属性。
    /// </summary>
    public MaterialAttrEnum? Attr { get; set; }
}
