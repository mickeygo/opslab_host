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
        new MenuItemModel("ColorWheel_16x.png", "控制台", typeof(KibanaCtl), true),
        new MenuItemModel("User_16x.png", "用户信息", typeof(UserCtl), false),
        new MenuItemModel("WindowsForm_16x.png", "物料信息", typeof(ItemCtl), false),
        new MenuItemModel("Procedure_16x.png", "字典数据", typeof(DictDataCtl), false),
        new MenuItemModel("TextBox_16x.png", "卡号管理", typeof(CardCtl), false),
        new MenuItemModel("ProgressButton_16x.png", "工站信息", typeof(StationCtl), false),

        new MenuItemModel("LinearCarousel_16x.png", "工序设置", typeof(ProcessCtl), false),
        new MenuItemModel("CheckboxList_16x.png", "工艺BOM", typeof(ProcessBomCtl), false),
        new MenuItemModel("brackets_Square_16xLG.png", "工参设置", typeof(ProcessParamCtl), false),
        new MenuItemModel("Flow_16x.png", "工艺路线", typeof(ProcessRouteCtl), false),

        new MenuItemModel("ListView_16x.png", "工单管理", typeof(WorkOrderCtl), false),
        new MenuItemModel("SortAscending_gray_16x.png", "生产排程", typeof(WoScheduleCtl), false),

        new MenuItemModel("Tag_16x.png", "进站信息", typeof(InboundCtl), false),
        new MenuItemModel("DataGrid_16x.png", "过站信息", typeof(ArchiveCtl), false),
        new MenuItemModel("TreeView_16x.png", "物料追溯", typeof(MaterialTraceCtl), false),
        new MenuItemModel("Search_16x.png", "警报记录", typeof(AlarmRecordCtl), false),
        new MenuItemModel("Search_16x.png", "刷卡记录", typeof(CardRecordCtl), false),
    };
}
