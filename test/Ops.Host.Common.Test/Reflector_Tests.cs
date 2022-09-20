using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Ops.Host.Common.Test;

public class Reflector_Tests
{
    [Fact]
    public void Should_Check_NotNull_Test()
    {
        ReflectorModel model = new();

        var prop = typeof(ReflectorModel).GetProperty(nameof(ReflectorModel.Date));
        var v1 = prop!.PropertyType.IsConstructedGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>);
        Assert.True(v1);

        var v2 = prop.GetValue(model);
        var v3 = v2 is null && prop.GetCustomAttribute<RequiredAttribute>() is not null;
        Assert.True(v3);
    }
}

internal class ReflectorModel
{
    public int? Qty { get; set; }

    [Required]
    public DateTime? Date { get; set; }

}
