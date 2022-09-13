namespace Ops.Host.Core.Services;

internal sealed class InboundService : ScadaDomainService, IInboundService
{
    private readonly SqlSugarRepository<PtInbound> _inboundRep;
    private readonly SqlSugarRepository<PtSnTransit> _transitRep;
    private readonly SqlSugarRepository<ProdWo> _woRep;
    private readonly ILogger _logger;

    public InboundService(SqlSugarRepository<PtInbound> inboundRep,
        SqlSugarRepository<PtSnTransit> transitRep,
        SqlSugarRepository<ProdWo> woRep,
        ILogger<InboundService> logger)
    {
        _inboundRep = inboundRep;
        _transitRep = transitRep;
        _woRep = woRep;
        _logger = logger;
    }

    public async Task<ReplyResult> HandleAsync(ForwardData data)
    {
        var sn = data.GetString(PlcSymbolTag.PLC_Inbound_SN); // SN
        var formula = data.GetInt(PlcSymbolTag.PLC_Inbound_Formula); // PLC 配方号
        var pallet = data.GetString(PlcSymbolTag.PLC_Inbound_Pallet); // 托盘码

        if (string.IsNullOrWhiteSpace(sn))
        {
            return Error(ErrorCodeEnum.E0404);
        }
        if (formula == 0)
        {
            return Error(ErrorCodeEnum.E0403);
        }

        try
        {
            // 主数据
            PtInbound item0 = new()
            {
                LineCode = data.Schema.Line,
                StationCode = data.Schema.Station,
                ProductCode = "", // SN 反查产品
                WO = "", // SN 反查工单
                SN = sn,
                FormualNo = (int)formula!,
            };

            item0.InboundItems = new();

            foreach (var v in data.Values.Where(s => s.IsAdditional))
            {
                item0.InboundItems.Add(new PtInboundItem
                {
                    Tag = v.Tag,
                    Name = v.Name,
                    Value = v.GetString(), // 都以 String 类型存储
                });
            }

            // 级联插入
            await _inboundRep.AsSugarClient()
                .InsertNav(item0)
                .Include(s => s.InboundItems)
                .ExecuteCommandAsync();

            // 更新 SN 过站状态信息
            var snTransit = await _transitRep.GetFirstAsync(s => s.SN == sn);
            snTransit ??= new()
            {
                SN = sn,
                ProductCode = item0.ProductCode,
                WO = item0.WO,
            };

            snTransit.LineCode = data.Schema.Line;
            snTransit.StationCode = data.Schema.Station;
            snTransit.TransitMode = TransitModeEnum.Inbound;
            snTransit.InboundTime = DateTime.Now;
            snTransit.OutboundTime = null;
            snTransit.Pass = null;

            await _transitRep.InsertOrUpdateAsync(snTransit);

            // 更新工单数据（如果存在工单）
            var workOrder = await _woRep.GetFirstAsync(s => s.Code == item0.WO);
            if (workOrder is not null)
            {
                // 首次上线（非返工）
                if (workOrder.Status == WoStatusEnum.Scheduled)
                {
                    workOrder.OnlineQty += 1;
                    workOrder.Status = WoStatusEnum.Producing;
                    workOrder.ActualStartDate = DateTime.Now;

                    await _woRep.UpdateAsync(workOrder);
                }
            }

            // 推送消息
            // await MessageTaskQueueManager.Default.QueueAsync(new Message(item0.LineCode, item0.StationCode, MessageClassify.Inbound, item0.SN ?? ""));

            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"[InboundService] {sn} 进站异常");
            return Error();
        }
    }
}
