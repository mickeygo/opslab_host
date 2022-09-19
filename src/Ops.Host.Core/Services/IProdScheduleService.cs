namespace Ops.Host.Core.Services;

/// <summary>
/// 生产排程服务
/// </summary>
public interface IProdScheduleService : IDomainService
{
    /// <summary>
    /// 获取所有处于下发状态的工单。
    /// </summary>
    /// <returns></returns>
    List<ProdWo> GetAllIssue();

    /// <summary>
    /// 获取所有排程的工单。工单状态为 [已下发, 完工)。
    /// </summary>
    /// <returns></returns>
    List<ProdSchedule> GetAllSchedule();

    /// <summary>
    /// 获取所有排程的工单。工单状态为 [已下发, 完工)。
    /// </summary>
    /// <returns></returns>
    Task<List<ProdSchedule>> GetAllScheduleAsync();
}
