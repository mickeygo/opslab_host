namespace Ops.Host.Common.Utils;

/// <summary>
/// 下拉框帮助类。
/// </summary>
public static class DropdownListHelper
{
    /// <summary>
    /// 生成下拉框选项。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <returns></returns>
    public static List<T> Make<T>(IEnumerable<T> list)
        where T : class, new()
    {
        if (list is List<T> list1)
        {
            list1.Insert(0, new T());
        }

        var list2 = list.ToList();
        list2.Insert(0, new T());

        return list2;
    }

    /// <summary>
    /// 生成下拉框选项。
    /// </summary>
    /// <param name="list"></param>
    /// <returns></returns>
    public static List<NameValue> MakeDropdownList(this List<NameValue> list)
    {
        list.Insert(0, new NameValue("", ""));
        return list;
    }
}
