namespace Ops.Host.Core.Dtos;

public class SysCardRecordFilter
{
    /// <summary>
    /// 卡号
    /// </summary>
    public string? CardNo { get; set; }

    /// <summary>
    /// 持卡人
    /// </summary>
    public string? Owner { get; set; }

    public string? CardDeviceName { get; set; }

    public DateTime? CreateTimeStart { get; set; }

    public DateTime? CreateTimeEnd { get; set; }
}

public class SysCardRecordInput
{
}
