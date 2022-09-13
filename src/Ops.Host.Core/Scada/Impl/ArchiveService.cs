namespace Ops.Host.Core.Services;

internal sealed class ArchiveService : ScadaDomainService, IArchiveService
{
    private const char ArrayValueSeparator = ','; // 数组值分隔符

    private readonly SqlSugarRepository<PtArchive> _archiveRep;
    private readonly SqlSugarRepository<PtSnTransit> _transitRep;
    private readonly SqlSugarRepository<ProdWo> _woRep;
    private readonly ILogger _logger;

    public ArchiveService(SqlSugarRepository<PtArchive> archiveRep, 
        SqlSugarRepository<PtSnTransit> transitRep,
        SqlSugarRepository<ProdWo> woRep,
        ILogger<ArchiveService> logger)
    {
        _archiveRep = archiveRep;
        _transitRep = transitRep;
        _woRep = woRep;
        _logger = logger;
    }

    public async Task<ReplyResult> HandleAsync(ForwardData data)
    {
        var sn = data.GetString(PlcSymbolTag.PLC_Archive_SN); // SN
        var formula = data.GetInt(PlcSymbolTag.PLC_Archive_Formula); // PLC 配方号
        var pass = data.GetInt(PlcSymbolTag.PLC_Archive_Pass); // 结果
        var ct = data.GetInt(PlcSymbolTag.PLC_Archive_Cycletime); // CT
        var @operator = data.GetString(PlcSymbolTag.PLC_Archive_Operator); // 操作人
        var shift = data.GetInt(PlcSymbolTag.PLC_Archive_Shift); // 班次
        var pallet = data.GetString(PlcSymbolTag.PLC_Archive_Pallet); // 托盘号

        if (string.IsNullOrWhiteSpace(sn))
        {
            return Error(ErrorCodeEnum.E0404);
        }
        if (formula == 0)
        {
            return Error(ErrorCodeEnum.E0403);
        }
        if (!EnumExtensions.TryParse<PassEnum>(pass, out var pass0))
        {
            return Error(ErrorCodeEnum.E1201);
        }
       
        // 记录进站信息
        try
        {
            // 检查在该站是否有进站信息。
            var snTransit = await _transitRep.GetFirstAsync(s => s.SN == sn && s.LineCode == data.Schema.Line && s.StationCode == data.Schema.Station);
            if (snTransit == null)
            {
                return Error(ErrorCodeEnum.E1202);
            }

            // 主数据
            var item0 = new PtArchive
            {
                LineCode = data.Schema.Line,
                StationCode = data.Schema.Station,
                ProductCode = snTransit.ProductCode,
                WO = snTransit.WO,
                SN = sn,
                FormualNo = (int)formula!,
                Pass = pass0!,
                Cycletime = ct,
                Operator = @operator,
                Shift = "", // 需反向查找班次
                Pallet = pallet,
                ArchiveItems = new(),
            };

            // 明细数据
            foreach (var v in data.Values.Where(s => s.IsAdditional))
            {
                var item = new PtArchiveItem
                {
                    Tag = v.Tag,
                    Name = v.Name,
                    Value = v.GetString(), // 都以 String 类型存储
                    IsArray = v.IsArray(),
                    DataType = GetDataType(v.VarType),
                    Lower = 0, // 需从工艺参数中查找
                    Higher = 0, // 需从工艺参数中查找
                    ArchiveItemLines = new(),
                };
                item.IsPass = CheckRange(item.DataType, item.Value!, item.Lower, item.Higher);

                // 数组行细分
                if (v.IsArray())
                {
                    var arr2 = item.Value!.Split(ArrayValueSeparator);
                    for (int i = 0; i < arr2.Length; i++)
                    {
                        var v2 = arr2[i];
                        PtArchiveItemLine itemLine = new()
                        {
                            Value = v2,
                            Seq = i + 1, // 序号从 1 开始
                            Lower = 0, // 需从工艺参数中查找
                            Higher = 0, // 需从工艺参数中查找
                        };
                        itemLine.IsPass = CheckRange(item.DataType, itemLine.Value!, itemLine.Lower, itemLine.Higher);

                        item.ArchiveItemLines.Add(itemLine);
                    }
                }

                item0.ArchiveItems.Add(item);
            }

            // 级联插入
            await _archiveRep.AsSugarClient()
                .InsertNav(item0)
                .Include(s => s.ArchiveItems)
                .ThenInclude(s => s.ArchiveItemLines)
                .ExecuteCommandAsync();

            // 更新 SN 过站状态信息
            snTransit.LineCode = data.Schema.Line;
            snTransit.StationCode = data.Schema.Station;
            snTransit.TransitMode = TransitModeEnum.Outbound;  // 需检验是否完工
            snTransit.OutboundTime = DateTime.Now;
            snTransit.Pass = pass0;

            await _transitRep.UpdateAsync(snTransit);

            // 若是 NG，添加到可返工列表中。
            if (snTransit.Pass is PassEnum.NG or PassEnum.ForceNG)
            {

            }

            // TODD: 检查是否为尾站
            bool isTail = false;
            if (isTail)
            {
                // 更新工单数据（如果存在工单）
                var workOrder = await _woRep.GetFirstAsync(s => s.Code == item0.WO);
                if (workOrder is not null)
                {
                    // OK 过站
                    workOrder.CompletedQty += 1;

                    // 当数据量相等时，表示单据完工。
                    if ((workOrder.CompletedQty + workOrder.ScrappedQty + workOrder.DismantlingQty) == workOrder.Qty)
                    {
                        workOrder.Status = WoStatusEnum.Completed;
                        workOrder.ActualEndDate = DateTime.Now;
                    }

                    await _woRep.UpdateAsync(workOrder);
                }
            }

            // 工站状态统计

            // 推送消息
            // await MessageTaskQueueManager.Default.QueueAsync(new Message(item0.LineCode, item0.StationCode, MessageClassify.Inbound, item0.SN ?? ""));

            return Ok();
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, $"[ArchiveService] {sn} 出站异常");
            return Error();
        }
    }

    private bool CheckRange(DynamicDataTypeEnum dataType, string value, decimal? lower, decimal? higher)
    {
        // 对于整型和浮点类型，上限或下限设置了值才会进行比较；
        //  bool 类型直接判断真假，没有上下限。
        if (dataType == DynamicDataTypeEnum.Float)
        {
            var v = Convert.ToDecimal(value);
            if (lower is not null)
            {
                if (v < lower)
                {
                    return false;
                }
            }

            if (higher is not null)
            {
                if (v > higher)
                {
                    return false;
                }
            }
        } 
        else if (dataType == DynamicDataTypeEnum.Integer)
        {
            var v = Convert.ToInt32(value);
            if (lower is not null)
            {
                if (v < lower)
                {
                    return false;
                }
            }

            if (higher is not null)
            {
                if (v > higher)
                {
                    return false;
                }
            }
        }
        else if (dataType == DynamicDataTypeEnum.Boolean)
        {
            // bool 类型没有上下限，直接判断真假即可。
            return Convert.ToBoolean(value);
        }

        return true;
    }

    private static DynamicDataTypeEnum GetDataType(VariableType v)
    {
        return v switch
        {
            VariableType.Bit => DynamicDataTypeEnum.Boolean,
            VariableType.Byte or
            VariableType.Word or
            VariableType.DWord or
            VariableType.Int or
            VariableType.DInt => DynamicDataTypeEnum.Integer,
            VariableType.Real or 
            VariableType.LReal => DynamicDataTypeEnum.Float,
            VariableType.String or 
            VariableType.S7String or 
            VariableType.S7WString => DynamicDataTypeEnum.String,
            _ => DynamicDataTypeEnum.String,
        };
    }
}
