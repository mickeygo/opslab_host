namespace Ops.Host.Core.Management;

/// <summary>
/// 工艺管理，包括工艺路线、BOM及工参。
/// </summary>
public sealed class ProcessManager : IManager
{
    const string Key = "ops.host.cache.procprocess";

    private readonly IMemoryCache _memoryCache;

	public ProcessManager(IMemoryCache memoryCache)
	{
		_memoryCache = memoryCache;
	}

	public Task<bool> IsHead(string lineCode, string stationCode, string productCode)
	{

		return Task.FromResult(true);
	}

	/// <summary>
	/// 是否为首站。
	/// </summary>
	/// <returns></returns>
	public bool IsHead(ProcProcess process)
	{
		return true;
	}

	/// <summary>
	/// 是否为尾站。
	/// </summary>
	/// <returns></returns>
	public bool IsTail(ProcProcess process)
	{
		return true;
	}
}
