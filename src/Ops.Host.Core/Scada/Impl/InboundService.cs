namespace Ops.Host.Core.Services;

internal sealed class InboundService : ScadaDomainService, IInboundService
{
    public async Task<ReplyResult> HandleAsync(ForwardData data)
    {
        await Task.Delay(100);

        var sn = data.GetString(PlcSymbolTag.PLC_Inbound_SN); // SN
        var formula = data.GetInt(PlcSymbolTag.PLC_Inbound_Formula); // PLC 配方号
        var pallet = data.GetString(PlcSymbolTag.PLC_Inbound_Pallet); // 托盘码
        if (string.IsNullOrWhiteSpace(sn))
        {
            return Error();
        }
        if (formula == 0)
        {
            return Error();
        }

        try
        { 

            return Ok();
        }
        catch (Exception)
        {
            return Error();
        }
    }
}
