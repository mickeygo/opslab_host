namespace Ops.Host.Core.Services;

internal sealed class InboundService : ScadaDomainService, IInboundService
{
    private readonly SqlSugarRepository<PtInbound> _inboundRep;
    private readonly SqlSugarRepository<PtSnTransit> _transitRep;
    private readonly SqlSugarRepository<ProdWo> _woRep;
    private readonly IProcRouteService _routeService;
    private readonly StationCacheManager _stationCacheManager;
    private readonly BusinessOptions _bizOptions;
    private readonly ILogger _logger;

    public InboundService(SqlSugarRepository<PtInbound> inboundRep,
        SqlSugarRepository<PtSnTransit> transitRep,
        SqlSugarRepository<ProdWo> woRep,
        IProcRouteService routeService,
        StationCacheManager stationCacheManager,
        IOptions<BusinessOptions> bizOptions,
        ILogger<InboundService> logger)
    {
        _inboundRep = inboundRep;
        _transitRep = transitRep;
        _woRep = woRep;
        _routeService = routeService;
        _stationCacheManager = stationCacheManager;
        _bizOptions = bizOptions.Value;
        _logger = logger;
    }

    public async Task<ReplyResult> HandleAsync(ForwardData data)
    {
        var sn = data.GetString(PlcSymbolTag.PLC_Inbound_SN); // SN
        var formula = data.GetInt(PlcSymbolTag.PLC_Inbound_Formula) ?? 0; // PLC 配方号
        var pallet = data.GetString(PlcSymbolTag.PLC_Inbound_Pallet); // 托盘码

        if (string.IsNullOrWhiteSpace(sn))
        {
            return Error(ErrorCodeEnum.E0404);
        }

        try
        {
            var station = _stationCacheManager.GetStation(data.Schema.Line, data.Schema.Station);
            if (station == null)
            {
                return Error(ErrorCodeEnum.E405);
            }

            string? productCode = string.Empty, wo = string.Empty;

            // 查找SN状态信息（若 SN 是返工或重复进站，会保留相关的产品信息）
            var snTransit = await _transitRep.GetFirstAsync(s => s.SN == sn);
            bool hadOnline = snTransit != null; // 表示SN已经上线过
            if (!hadOnline)
            {
                // SN 首次上线，通过程序配方号查找
                // TODO: SN 反查产品

                // 首站且首次上线 "_bizOptions.UseWo && isHead"
                if (_bizOptions.UseWo)
                {
                    // TODO：查找生产（排产）中的工单

                    wo = "";
                }
            }
            else
            {
                // 已存在 SN
                productCode = snTransit!.ProductCode;
                wo = snTransit.WO;
            }

            // 校验工艺路线。
            if (_bizOptions.IsMatchRoute)
            {
                // TODO: 校验工艺路线
                // 前一道工序OK过站后才能进行当前工序
            }

            // 主数据
            PtInbound item0 = new()
            {
                LineCode = data.Schema.Line,
                StationCode = data.Schema.Station,
                ProductCode = productCode,
                WO = wo,
                SN = sn,
                FormualNo = (int)formula!,
                InboundItems = new(),
            };

            // 附加数据
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
            snTransit ??= new()
            {
                SN = sn,
                ProductCode = item0.ProductCode,
                WO = item0.WO,
            };

            snTransit.LineCode = data.Schema.Line;
            snTransit.StationCode = data.Schema.Station;
            snTransit.TransitStage = TransitStageEnum.Inbound;
            snTransit.InboundTime = DateTime.Now;
            snTransit.OutboundTime = null;
            snTransit.Pass = null;

            // 检查是否为首站（线上站+工艺路线）
            bool isHead = station!.Owner == StationOwnerEnum.Inline
                            && await _routeService.IsHeadAsync(snTransit.ProductCode, station.LineCode, station.StationCode);
            if (isHead)
            {
                snTransit.OnlineTime = DateTime.Now; 
            }

            await _transitRep.InsertOrUpdateAsync(snTransit);
           
            // 首站（线上站），更新工单数据（如果存在工单）
            if (_bizOptions.UseWo && !hadOnline && isHead)
            {
                var workOrder = await _woRep.GetFirstAsync(s => s.Code == item0.WO);
                if (workOrder is not null)
                {
                    // 首次上线（工单处于排产状态，非返工）
                    if (workOrder.Status == WoStatusEnum.Scheduled)
                    {
                        workOrder.OnlineQty += 1;
                        workOrder.Status = WoStatusEnum.Producing; // 生产中
                        workOrder.ActualStartDate = DateTime.Now;

                        await _woRep.UpdateAsync(workOrder);
                    }
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
