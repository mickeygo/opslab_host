namespace Ops.Host.Core.Dtos;

public class AlarmRecordFilter
{
    /// <summary>
    /// 工站编码
    /// </summary>
    public string? StationCode { get; set; }

    /// <summary>
    /// 描述
    /// </summary>
    public string? Descirption { get; set; }

    public DateTime? CreateTimeStart { get; set; }

    public DateTime? CreateTimeEnd { get; set; }
}

public class AlarmRecordInput
{
}
