using Ops.Host.App.Models;
using Ops.Host.App.UserControls;

namespace Ops.Host.App.Management;

/// <summary>
/// 左侧菜单管理
/// </summary>
public sealed class MenuManager
{
    /// <summary>
    /// 获取菜单
    /// </summary>
    public static MenuItemModel[] Menus => new[]
    {
        new MenuItemModel("", "设备看板", typeof(Kibana), true),
        new MenuItemModel("", "用户信息", typeof(User), false),
        new MenuItemModel("", "物料信息", typeof(ItemCtl), false),
        new MenuItemModel("", "字典数据", typeof(DictData), false),
        new MenuItemModel("", "进站记录", typeof(Inbound), false),
        new MenuItemModel("", "过站记录", typeof(Archive), false),
        new MenuItemModel("", "物料追溯", typeof(MaterialTrace), false),
    };
}
