namespace Ops.Host.Core.Dtos;

public class ProdWoFilter
{
    /// <summary>
    /// 工单编码
    /// </summary>
    public string? WoCode { get; set; }

    /// <summary>
    /// 工单名称
    /// </summary>
    public string? WoName { get; set; }

    /// <summary>
    /// 产品信息Id
    /// </summary>
    public long? ProductId { get; set; }
}

public class ProdWoInput
{
}
