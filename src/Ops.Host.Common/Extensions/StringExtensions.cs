namespace Ops.Host.Common.Extensions;

/// <summary>
/// 字符串扩展类。
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// 转换为 Int32。
    /// </summary>
    /// <remarks>转换失败会抛出异常。</remarks>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="FormatException"></exception>
    /// <exception cref="OverflowException"></exception>
    public static int ToInt(this string str)
    {
        return int.Parse(str);
    }

    /// <summary>
    /// 转换为 Int32。
    /// </summary>
    /// <param name="defaultValue">转换失败时返回的默认值。</param>
    /// <remarks>转换失败不会抛出异常。</remarks>
    /// <returns></returns>
    public static int? AsInt(this string str, int? defaultValue = default)
    {
        if (int.TryParse(str, out var result))
        {
            return result;
        }

        return defaultValue;
    }

    /// <summary>
    /// 转换为 double。
    /// </summary>
    /// <remarks>转换失败会抛出异常。</remarks>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="FormatException"></exception>
    /// <exception cref="OverflowException"></exception>
    public static double ToDouble(this string str)
    {
        return double.Parse(str);
    }

    /// <summary>
    /// 转换为 double。
    /// </summary>
    /// <param name="defaultValue">转换失败时返回的默认值。</param>
    /// <remarks>转换失败不会抛出异常。</remarks>
    /// <returns></returns>
    public static double? AsDouble(this string str, double? defaultValue = default)
    {
        if (double.TryParse(str, out var result))
        {
            return result;
        }

        return defaultValue;
    }

    /// <summary>
    /// 转换为 DateTime。
    /// </summary>
    /// <remarks>转换失败会抛出异常。</remarks>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="FormatException"></exception>
    public static DateTime ToDateTime(this string str)
    {
        return DateTime.Parse(str);
    }

    /// <summary>
    /// 转换为 DateTime。
    /// </summary>
    /// <param name="defaultValue">转换失败时返回的默认值。</param>
    /// <remarks>转换失败不会抛出异常。</remarks>
    /// <returns></returns>
    public static DateTime? AsDouble(this string str, DateTime? defaultValue = default)
    {
        if (DateTime.TryParse(str, out var result))
        {
            return result;
        }

        return defaultValue;
    }
}
