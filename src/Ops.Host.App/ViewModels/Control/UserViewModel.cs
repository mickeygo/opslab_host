namespace Ops.Host.App.ViewModels;

public sealed class UserViewModel : SinglePagedViewModelBase<SysUser, UserFilter>, IViewModel
{
    private readonly ISysUserService _userService;

    public UserViewModel(ISysUserService userService)
    {
        _userService = userService;
    }

    protected override (bool ok, string? err) OnSave(SysUser data)
    {
        return _userService.InsertOrUpdateUser(data);
    }

    protected override (bool ok, string? err) OnDelete(SysUser data)
    {
        var ok = _userService.DeleteUser(data);
        return (ok, "");
    }

    protected override void OnExcelCreating(ExcelModelBuilder builder)
    {
        builder.ExcelName = "用户信息";
        builder.SheetName = "用户";
        builder.Settings.Excludes.Add(nameof(SysUser.Password));

        //builder.Header = new()
        //{
        //    new Common.IO.RowCustom
        //    {
        //         PaddingTop = 1,
        //         ColunmSpan = 9,
        //         Text = "用户测试示例 Header1",
        //         HorizontalAlignment = Common.IO.HorizontalAlignment.Center,
        //    },
        //    new Common.IO.RowCustom
        //    {
        //         ColunmSpan = 9,
        //         Text = "用户测试示例 Header2",
        //         HorizontalAlignment = Common.IO.HorizontalAlignment.Center,
        //    },
        //};

        //builder.Footer = new()
        //{
        //    new Common.IO.RowCustom
        //    {
        //         PaddingTop = 2,
        //         ColunmSpan = 9,
        //         Text = "用户测试示例 Footer1",
        //         HorizontalAlignment = Common.IO.HorizontalAlignment.Right,
        //    },
        //    new Common.IO.RowCustom
        //    {
        //         ColunmSpan = 9,
        //         Text = "用户测试示例 Footer2",
        //         HorizontalAlignment = Common.IO.HorizontalAlignment.Right,
        //    },
        //};
    }

    protected override void OnPrintCreating(PrintModelBuilder builder)
    {
        builder.TemplateUrl = "./UserControls/Document/UserDocument.xaml";
        builder.DataContext = new UserDocumentViewModel
        {
            Title = "用户测试示例",
            DataSourceList = SearchedAllData,
        };
        builder.Render = new UserDocumentRender();
    }

    protected override PagedList<SysUser> OnSearch(int pageIndex, int pageSize)
    {
        return _userService.GetPagedList(QueryFilter, pageIndex, pageSize);
    }
}
