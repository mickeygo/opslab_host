namespace Ops.Host.Core.Services;

internal sealed class MaterialService : ScadaDomainService, IMaterialService
{
    public async Task<ReplyResult> HandleCriticalMaterialAsync(ForwardData data)
    {
        await Task.Delay(100);

        var barcode = data.GetString(PlcSymbolTag.PLC_Critical_Material_Barcode); // 物料条码
        var index = data.GetInt(PlcSymbolTag.PLC_Critical_Material_Index); // 扫描索引
        if (string.IsNullOrWhiteSpace(barcode))
        {
            return Error();
        }

        try
        {
            // 1. 校验物料是否是重复使用
            // 2. 校验 BOM

            return Ok();
        }
        catch (Exception)
        {
            return Error();
        }
    }

    public Task<ReplyResult> HandleBactchMaterialAsync(ForwardData data)
    {
        return Task.FromResult(Ok());
    }
}
