namespace Ops.Host.Shared.Component;

/// <summary>
/// 文档呈现器。
/// </summary>
public interface IDocumentRenderer
{
    void Render(FlowDocument doc, object? data);
}
