namespace Ops.Host.App.Forwarders;

internal sealed class OpsLocalUnderlyForwarder : IUnderlyForwarder
{
    private readonly ILogger _logger;

    public OpsLocalUnderlyForwarder(ILogger<OpsLocalUnderlyForwarder> logger)
	{
        _logger= logger;
	}

    public Task<UnderlyResult> ExecuteAsync(ForwardData data, CancellationToken cancellationToken = default)
    {
        var result = new UnderlyResult();

        try
		{
            if (data.Tag == "PLC_Custom_Heartbeat")
            {
                var ret0 = data.Self().GetBit();
                result.AddValue("PLC_Custom_Heartbeat_Callback", ret0);
            }
        }
		catch (Exception ex)
		{
            _logger.LogError(ex, "[OpsUnderlyForwarder] Handle Error");
        }

        return Task.FromResult(result);
    }
}
