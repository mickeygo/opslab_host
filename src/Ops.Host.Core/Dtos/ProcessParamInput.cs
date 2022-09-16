namespace Ops.Host.Core.Dtos;

public class ProcessParamFilter
{
    /// <summary>
    /// 产品/物料编码
    /// </summary>
    public string? ProductCode { get; set; }

    /// <summary>
    /// 产品/物料名称
    /// </summary>
    public string? ProductName { get; set; }

    /// <summary>
    /// 工序编码
    /// </summary>
    public string? ProcessCode { get; set; }
}

public class ProcessParamInput
{
}
