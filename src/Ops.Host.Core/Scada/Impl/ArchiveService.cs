namespace Ops.Host.Core.Services;

internal sealed class ArchiveService : ScadaDomainService, IArchiveService
{
    public async Task<ReplyResult> HandleAsync(ForwardData data)
    {
        await Task.Delay(100); // test

        var sn = data.GetString(PlcSymbolTag.PLC_Archive_SN); // SN
        var pass = data.GetInt(PlcSymbolTag.PLC_Archive_Pass); // 结果
        var ct = data.GetInt(PlcSymbolTag.PLC_Archive_Cycletime); // CT
        var @operator = data.GetString(PlcSymbolTag.PLC_Archive_Operator); // 操作人
        var shift = data.GetInt(PlcSymbolTag.PLC_Archive_Shift); // 班次
        var pallet = data.GetString(PlcSymbolTag.PLC_Archive_Pallet); // 托盘号

        if (string.IsNullOrWhiteSpace(sn))
        {
            return Error();
        }

        // 记录进站信息
        try
        {
            // 按位解析

            // 主数据

            // 明细数据
            foreach (var item in data.Values.Where(s => s.IsAdditional))
            {
               
            }

            // 工站状态统计

            return Ok();
        }
        catch(Exception)
        {
            return Error();
        }
    }
}
