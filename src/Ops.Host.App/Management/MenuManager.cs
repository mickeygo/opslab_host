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
        new MenuItemModel("", "设备看板", typeof(KibanaCtl), true),
        new MenuItemModel("", "用户信息", typeof(UserCtl), false),
        new MenuItemModel("", "物料信息", typeof(ItemCtl), false),
        new MenuItemModel("", "字典数据", typeof(DictDataCtl), false),
        new MenuItemModel("", "产品BOM", typeof(ProductBomCtl), false),
        new MenuItemModel("", "工站信息", typeof(StationCtl), false),
        new MenuItemModel("", "工艺管理", typeof(ProcessCtl), false),
        new MenuItemModel("", "进站记录", typeof(InboundCtl), false),
        new MenuItemModel("", "过站记录", typeof(ArchiveCtl), false),
        new MenuItemModel("", "物料追溯", typeof(MaterialTraceCtl), false),
        new MenuItemModel("", "卡号管理", typeof(CardCtl), false),
        new MenuItemModel("", "刷卡记录", typeof(CardRecordCtl), false),
    };
}
