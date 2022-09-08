namespace Ops.Host.App.ViewModels;

public sealed class UserDocumentViewModel
{
    public string? Title { get; set; }

    public List<SysUser>? DataSourceList { get; set; }
}

public sealed class UserDocumentRender : IDocumentRenderer
{
    public void Render(FlowDocument doc, object? data)
    {
        if (data is not UserDocumentViewModel vm || vm.DataSourceList is null)
        {
            return;
        }

        if (doc.FindName("rows") is not TableRowGroup group)
        {
            return;
        }

        Style? styleCell = doc.Resources["BorderedCell"] as Style;
        foreach (var item in vm.DataSourceList!)
        {
            TableRow row = new();

            TableCell cell = new(new Paragraph(new Run(item.UserName)));
            cell.Style = styleCell;
            row.Cells.Add(cell);

            cell = new(new Paragraph(new Run(item.DisplayName)));
            cell.Style = styleCell;
            row.Cells.Add(cell);

            cell = new(new Paragraph(new Run(item.CreateTime.ToString("yyyy-MM-dd HH:mm:ss"))));
            cell.Style = styleCell;
            row.Cells.Add(cell);

            group.Rows.Add(row);
        }
    }
}