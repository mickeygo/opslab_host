namespace Ops.Host.Core.Services;

internal sealed class MaterialService : ScadaDomainService, IMaterialService
{
    private const char RuleSeparator = ','; // 规则分隔符

    private readonly SqlSugarRepository<PtSnMaterial> _materialRep;
    private readonly SqlSugarRepository<PtTrackMaterial> _trackRep;
    private readonly SqlSugarRepository<PtSnTransit> _transitRep;
    private readonly IMdProductBomService _productBomService;

    private readonly ILogger _logger;

    public MaterialService(SqlSugarRepository<PtSnMaterial> materialRep,
        SqlSugarRepository<PtTrackMaterial> trackRep,
        SqlSugarRepository<PtSnTransit> transitRep,
        IMdProductBomService productBomService,
        ILogger<MaterialService> logger)
    {
        _materialRep = materialRep;
        _trackRep = trackRep;
        _transitRep = transitRep;
        _productBomService = productBomService;
        _logger = logger;
    }

    public async Task<ReplyResult> HandleCriticalMaterialAsync(ForwardData data)
    {
        var barcode = data.GetString(PlcSymbolTag.PLC_Critical_Material_Barcode); // 物料条码
        var index = data.GetInt(PlcSymbolTag.PLC_Critical_Material_Index); // 扫描索引，不为零值表示使用索引
        if (string.IsNullOrWhiteSpace(barcode))
        {
            return Error(ErrorCodeEnum.E1301);
        }

        try
        {
            // 是否有进站信息。
            var snTransit = await _transitRep.GetFirstAsync(s => s.LineCode == data.Schema.Line && s.StationCode == data.Schema.Station);
            if (snTransit == null || snTransit.TransitMode != TransitModeEnum.Inbound)
            {
                return Error(ErrorCodeEnum.E1202);
            }

            // 校验物料是否是重复使用
            if (await _trackRep.IsAnyAsync(s => s.SN == snTransit.SN))
            {
                return Error(ErrorCodeEnum.E1303);
            }

            // 校验 BOM
            var bom = await _productBomService.GetBomByProductCodeAsync(snTransit.ProductCode!);

            if (bom == null)
            {
                return Error(ErrorCodeEnum.E1304);
            }

            bool equalLength = true; // 是否匹配长度
            MdItem? scanMaterial = null; // 扫入的物料

            // 带有索引的扫描
            if (index > 0)
            {
                var item = bom.Items!.FirstOrDefault(s => s.Seq == index);
                if (item == null)
                {
                    return Error(ErrorCodeEnum.E1307);
                }

                if (item.Material!.Attr != MaterialAttrEnum.Critical)
                {
                    return Error(ErrorCodeEnum.E1306);
                }

                var rules = item.Material!.BarcodeRule.Split(RuleSeparator, StringSplitOptions.RemoveEmptyEntries);
                if (MatchRuleAny(rules, barcode, equalLength))
                {
                    scanMaterial = item.Material;
                }
            }
            else
            {
                // 无索引的关键物料，匹配任一即可。
                foreach (var item in bom.Items!.Where(s => s.Material!.Attr == MaterialAttrEnum.Critical))
                {
                    var rules = item.Material!.BarcodeRule.Split(RuleSeparator, StringSplitOptions.RemoveEmptyEntries);
                    if (MatchRuleAny(rules, barcode, equalLength))
                    {
                        scanMaterial = item.Material;
                        break;
                    }
                }
            }

            // 校验
            if (scanMaterial is null)
            {
                return Error(ErrorCodeEnum.E1305);
            }

            // 写入数据
            PtSnMaterial snMaterial = new()
            {
                SN = snTransit.SN,
                Barcode = barcode,
                ItemCode = scanMaterial.Code,
                LineCode = data.Schema.Line,
                StationCode = data.Schema.Station,
                Attr = MaterialAttrEnum.Critical,
            };
            await _materialRep.InsertAsync(snMaterial);

            PtTrackMaterial trackMaterial = new()
            {
                 SN = snTransit.SN,
                 Barcode = barcode,
            };
            await _trackRep.InsertAsync(trackMaterial);

            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"[MaterialService] '{barcode}' 扫码异常");
            return Error();
        }
    }

    public Task<ReplyResult> HandleBactchMaterialAsync(ForwardData data)
    {
        return Task.FromResult(Ok());
    }

    private bool MatchRuleAny(string[] rules, string barcode, bool equalLength)
    {
        foreach (var rule in rules)
        {
            if (MatchRule(rule, barcode, equalLength))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// 校验条码规则
    /// </summary>
    /// <param name="rule">规则</param>
    /// <param name="barcode">条码</param>
    /// <param name="equalLength">是否需要两者长度相等，若不需要，必须满足条码长度不小于规则长度。</param>
    /// <returns></returns>
    private bool MatchRule(string rule, string barcode, bool equalLength)
    {
        if (equalLength)
        {
            if (barcode.Length != rule.Length)
            {
                return false;
            }
        }
        else
        {
            if (barcode.Length < rule.Length)
            {
                return false;
            }
        }

        for (int i = 0; i < rule.Length; i++)
        {
            if (rule[i] != '#' && barcode[i] != rule[i])
            {
                return false;
            }
        }

        return true;
    }
}
