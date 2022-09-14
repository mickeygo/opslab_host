namespace Ops.Host.Core.Dtos;

public class ProductBomFilter
{
    /// <summary>
    /// 产品/物料代码
    /// </summary>
    public string? Code { get; set; }

    /// <summary>
    /// 产品/物料名称
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// 产线
    /// </summary>
    public string? LineCode { get; set; }

    /// <summary>
    /// 工站
    /// </summary>
    public string? StationCode { get; set; }
}

public class ProductBomInput
{

}
