namespace Ops.Host.Common;

/// <summary>
/// 资源画刷，用于在界面显示样式。
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
public sealed class ResourceBrushAttribute : Attribute
{
	/// <summary>
	/// 画刷名称
	/// </summary>
	public string BrushName { get; set; }

	public ResourceBrushAttribute(string brushName)
	{
		BrushName = brushName;
	}
}
