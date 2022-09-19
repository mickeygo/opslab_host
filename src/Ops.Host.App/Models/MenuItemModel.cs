namespace Ops.Host.App.Models;

/// <summary>
/// 菜单选项。
/// </summary>
public sealed class MenuItemModel
{
    /// <summary>
    /// 菜单图标
    /// </summary>
    public string? Icon { get; set; }

    /// <summary>
    /// 菜单名称
    /// </summary>
    [NotNull]
    public string? Name { get; set; }

    /// <summary>
    /// 呈现的内容对象类型。
    /// </summary>
    [NotNull]
    public Type? ContentType { get; set; }

    /// <summary>
    /// 呈现的内容对象。
    /// </summary>
    public object? Content { get; set; }

    /// <summary>
    /// 是否为首页。
    /// </summary>
    public bool IsHome { get; set; }

    /// <summary>
    /// 是否显示。
    /// </summary>
    public bool IsShow { get; set; }

    /// <summary>
    /// 页面是否是单例模式。
    /// </summary>
    /// <remarks>单例模式在页面每次切换时都不会重新创建页面。</remarks>
    public bool IsSingleton { get; set; }

    public MenuItemModel()
    {

    }

    public MenuItemModel(string icon, string name, Type contentType, bool isHome = false, bool isShow = true, bool isSingleton = true)
    {
        Icon = icon;
        Name = name;
        ContentType = contentType;
        IsHome = isHome;
        IsShow = isShow;
        IsSingleton = isSingleton;
    }
}
