namespace Ops.Host.Core.Dtos;

public class PtInboundFilter
{
    public string? StationCode { get; set; }

    public string? SN { get; set; }

    public DateTime? CreateTimeStart { get; set; } = DateTime.Now.AddDays(-1);

    public DateTime? CreateTimeEnd { get; set; } = DateTime.Now;
}

public class PtInboundInput
{
}
