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
        new MenuItemModel("", "卡号管理", typeof(CardCtl), false),
        new MenuItemModel("", "工站信息", typeof(StationCtl), false),
        new MenuItemModel("", "工艺设置", typeof(ProcessCtl), false),
        new MenuItemModel("", "工艺BOM", typeof(ProcessBomCtl), false),
        new MenuItemModel("", "工参设置", typeof(ProcessParamCtl), false),
        new MenuItemModel("", "工艺路线", typeof(ProcessRouteCtl), false),
        new MenuItemModel("", "工单管理", typeof(WorkOrderCtl), false),
        new MenuItemModel("", "生产排程", typeof(WoScheduleCtl), false),
        new MenuItemModel("", "进站记录", typeof(InboundCtl), false),
        new MenuItemModel("", "过站记录", typeof(ArchiveCtl), false),
        new MenuItemModel("", "物料追溯", typeof(MaterialTraceCtl), false),
        new MenuItemModel("", "警报记录", typeof(AlarmRecordCtl), false),
        new MenuItemModel("", "刷卡记录", typeof(CardRecordCtl), false),
    };
}
