namespace Ops.Host.Core.Management;

/// <summary>
/// 工艺管理
/// </summary>
public sealed class ProcessManager : IManager
{
    private readonly IMemoryCache _memoryCache;

	public ProcessManager(IMemoryCache memoryCache)
	{
		_memoryCache = memoryCache;
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
