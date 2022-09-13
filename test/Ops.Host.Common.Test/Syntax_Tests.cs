namespace Ops.Host.Common.Test;

/// <summary>
/// »ù´¡Óï·¨²âÊÔ
/// </summary>
public class Syntax_Tests
{
    [Fact]
    public void Enum_Test()
    {
        short? v1 = 1;
        TestEnum enum1 = (TestEnum)v1;
        Assert.True(enum1 == TestEnum.Test1); // true
    }

    public enum TestEnum
    {
        [Description("²âÊÔ1")]
        Test1 = 1,

        [Description("²âÊÔ2")]
        Test2 = 2,
    }
}
