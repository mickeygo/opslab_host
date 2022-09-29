namespace Ops.Host.Core.Services;

internal sealed class CustomService : ScadaDomainService, ICustomService
{
    private readonly SqlSugarRepository<ProdWo> _woRepo;
    private readonly SqlSugarRepository<ProdSchedule> _scheduleRepo;

    public Task<ReplyResult> HandleAsync(ForwardData data)
    {
        // 工单号处理
        if (data.Tag == "PLC_Custom_WorkOrder")
        {
            // 根据工单排程获取工单号
        }

        return Task.FromResult(Ok());
    }

    // 获取工单
    private Task<ReplyResult> HandleOrderAsync(ForwardData data)
    {
        return Task.FromResult(Ok());
    }
}
