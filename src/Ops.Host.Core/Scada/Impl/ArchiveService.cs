namespace Ops.Host.Core.Services;

internal sealed class ArchiveService : ScadaDomainService, IArchiveService
{
    private const char ArrayValueSeparator = ','; // 数组值分隔符

    private readonly SqlSugarRepository<PtArchive> _archiveRep;
    private readonly SqlSugarRepository<PtSnTransit> _transitRep;
    private readonly SqlSugarRepository<ProdWo> _woRep;
    private readonly SqlSugarRepository<ProcProcessParam> _paramRep;
    private readonly ISysDictDataService _sysDictDataService;
    private readonly BusinessOptions _bizOptions;
    private readonly ILogger _logger;

    public ArchiveService(SqlSugarRepository<PtArchive> archiveRep, 
        SqlSugarRepository<PtSnTransit> transitRep,
        SqlSugarRepository<ProdWo> woRep,
        SqlSugarRepository<ProcProcessParam> paramRep,
        ISysDictDataService sysDictDataService,
        IOptions<BusinessOptions> bizOptions,
        ILogger<ArchiveService> logger)
    {
        _archiveRep = archiveRep;
        _transitRep = transitRep;
        _woRep = woRep;
        _paramRep = paramRep;
        _sysDictDataService = sysDictDataService;
        _bizOptions = bizOptions.Value;
        _logger = logger;
    }

    public async Task<ReplyResult> HandleAsync(ForwardData data)
    {
        var sn = data.GetString(PlcSymbolTag.PLC_Archive_SN); // SN
        var formula = data.GetInt(PlcSymbolTag.PLC_Archive_Formula); // PLC 配方号
        var pass = data.GetInt(PlcSymbolTag.PLC_Archive_Pass); // 结果
        var ct = data.GetInt(PlcSymbolTag.PLC_Archive_Cycletime); // CT
        var @operator = data.GetString(PlcSymbolTag.PLC_Archive_Operator); // 操作人
        var shift = data.GetInt(PlcSymbolTag.PLC_Archive_Shift) ?? 0; // 班次
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

            // 从字典中查找对应的警报信息，若字典中没有设置，不会存储对应警报信息。
            var dicts = await _sysDictDataService.GetDicsByCodeAsync(DictCodeEnum.Shift.ToString());
            var shift0 = dicts.FirstOrDefault(s => s.Value == shift.ToString())?.Name;

            // 获取工艺参数
            var processParam = await GetProcessParamAsync(snTransit.ProductCode);

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
                Shift = shift0,
                Pallet = pallet,
                ArchiveItems = new(),
            };

            // 明细数据
            foreach (var v in data.Values.Where(s => s.IsAdditional))
            {
                var paramContent0 = processParam?.Contents?.FirstOrDefault(s => s.Tag == v.Tag);
                bool isArray = v.IsArray();

                var item = new PtArchiveItem
                {
                    Tag = v.Tag,
                    Name = v.Name,
                    Value = v.GetString(), // 都以 String 类型存储
                    IsArray = isArray,
                    DataType = GetDataType(v.VarType),
                    Lower = isArray ? default : paramContent0?.Lower, // 若是数组，在详细项中设置
                    Higher = isArray ? default : paramContent0?.Higher,
                    IsPass = true, // 初始设为 true
                    ArchiveItemLines = new(),
                };
               
                // 数组行细分
                if (isArray)
                {
                    bool isAllPass = true;
                    var arr2 = item.Value!.Split(ArrayValueSeparator);
                    for (int i = 0; i < arr2.Length; i++)
                    {
                        var paramContent1 = processParam?.Contents?.FirstOrDefault(s => s.Tag == v.Tag && s.Seq == i + 1);

                        var v2 = arr2[i];
                        PtArchiveItemLine itemLine = new()
                        {
                            Value = v2,
                            Seq = i + 1, // 序号从 1 开始
                            Lower = paramContent1?.Lower,
                            Higher = paramContent1?.Higher,
                            IsPass = true, // 初始设为 true
                        };

                        // 需校验参数
                        if (paramContent1?.IsCheck == true)
                        {
                            itemLine.IsPass = CheckRange(item.DataType, itemLine.Value!, itemLine.Lower, itemLine.Higher);
                        }

                        isAllPass = isAllPass && itemLine.IsPass;

                        item.ArchiveItemLines.Add(itemLine);
                    }

                    // 设置主项目
                    item.IsPass = isAllPass;
                }
                else
                {
                    // 需校验参数
                    if (paramContent0?.IsCheck == true)
                    {
                        item.IsPass = CheckRange(item.DataType, item.Value!, item.Lower, item.Higher);
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
            snTransit.TransitStage = TransitStageEnum.Outbound; // 需检验是否完工
            snTransit.OutboundTime = DateTime.Now;
            snTransit.Pass = pass0;

            if (_bizOptions.IsMatchProcessParam)
            {
                if (item0.ArchiveItems.Any(s => !s.IsPass))
                {
                    snTransit.Pass = PassEnum.NG;
                }
            }

            snTransit.SetProductStatus();

            // 线上站才处理工单

            // TODD: 检查是否为尾站
            bool isTail = false;
            if (isTail && snTransit.IsOK())
            {
                snTransit.CompletedTime = DateTime.Now;
            }

            await _transitRep.UpdateAsync(snTransit);

            // 若是 NG，添加到可返工列表中。
            if (snTransit.IsNG())
            {
                return Ok();
            }

            // OK 过站
            if (isTail && snTransit.IsOK())
            {
                // 更新工单数据（如果存在工单）
                var workOrder = await _woRep.GetFirstAsync(s => s.Code == item0.WO);
                if (workOrder is not null)
                {
                    workOrder.CompletedQty += 1; // 完工数 +1

                    // 当工单两边数量相等时，表示单据完工。
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

    /// <summary>
    /// 获取工艺参数
    /// </summary>
    private async Task<ProcProcessParam?> GetProcessParamAsync(string? productCode)
    {
        if (productCode is null)
        {
            return default;
        }

        return await _paramRep.AsQueryable()
            .Includes(s => s.Product!.Code == productCode)
            .Includes(s => s.Contents)
            .FirstAsync();
    }

    /// <summary>
    /// 对于整型和浮点类型，上限或下限设置了值才会进行比较；bool 类型会转换为 0或1 去比较。
    /// </summary>
    private bool CheckRange(DynamicDataTypeEnum dataType, string value, decimal? lower, decimal? higher)
    {
        if (dataType == DynamicDataTypeEnum.Float)
        {
            var v = Convert.ToDecimal(value);
            if (lower is not null && v < lower)
            {
                return false;
            }

            if (higher is not null && v > higher)
            {
                return false;
            }
        } 
        else if (dataType == DynamicDataTypeEnum.Integer)
        {
            var v = Convert.ToInt32(value);
            if (lower is not null && v < lower)
            {
                return false;
            }

            if (higher is not null && v > higher)
            {
                return false;
            }
        }
        else if (dataType == DynamicDataTypeEnum.Boolean)
        {
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
