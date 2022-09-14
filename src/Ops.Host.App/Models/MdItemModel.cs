namespace Ops.Host.App.Models;

public class MdItemModel : ObservableObject
{
    public long Id { get; set; }

    private string? _code;

    /// <summary>
    /// 产品/物料代码
    /// </summary>
    [DisplayName("物料代码")]
    [Required]
    public string? Code 
    { 
        get => _code; 
        set => SetProperty(ref _code, value); 
    }

    private string? _name;

    /// <summary>
    /// 产品/物料名称
    /// </summary>
    [DisplayName("物料名称")]
    [Required]
    public string? Name
    {
        get => _name;
        set => SetProperty(ref _name, value);
    }

    private string? _spec;

    /// <summary>
    /// 规格型号
    /// </summary>
    [DisplayName("规格型号")]
    public string? Spec
    {
        get => _spec;
        set => SetProperty(ref _spec, value);
    }

    /// <summary>
    /// 物料属性。
    /// </summary>
    public MaterialAttrEnum Attr { get; set; } = MaterialAttrEnum.Critical;

    private string? _barcodeRule;

    /// <summary>
    /// 条码规则，多个以逗号分隔。
    /// </summary>
    [DisplayName("条码规则")]
    [Required]
    public string? BarcodeRule
    {
        get => _barcodeRule;
        set => SetProperty(ref _barcodeRule, value);
    }

    /// <summary>
    /// 保质期（天）
    /// </summary>
    [DisplayName("保质期")]
    public int? Expiration { get; set; }
}
