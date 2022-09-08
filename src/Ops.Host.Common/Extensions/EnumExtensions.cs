namespace Ops.Host.Common.Extensions;

/// <summary>
/// 枚举类型扩展。
/// </summary>
public static class EnumExtensions
{
    /// <summary>
    /// 获取指定类型的枚举字段列表。
    /// </summary>
    /// <remarks>
    /// Name 显示 <see cref="DisplayAttribute"/> 描述，若没有，这显示 <see cref="DescriptionAttribute"/>，再没有则显示字段名称；Value 为字段值（int 类型）。
    /// </remarks>
    /// <returns></returns>
    public static List<NameValue<string, int>> ToNameValueList<T>()
         where T : Enum
    {
        var fields = typeof(T).GetFields().Where(s => s.FieldType.IsEnum).OrderBy(s => s.GetRawConstantValue()).ToList();
        List<NameValue<string, int>> list = new(fields.Count);

        foreach (var field in fields)
        {
            var attr0 = field.GetCustomAttribute<DisplayAttribute>(false);
            if (attr0 != null)
            {
                list.Add(new NameValue<string, int>(attr0!.GetName() ?? "", (int)field.GetRawConstantValue()!));
                continue;
            }

            var attr1 = field.GetCustomAttribute<DescriptionAttribute>(false);
            list.Add(new NameValue<string, int>(attr1?.Description ?? field.Name, (int)field.GetRawConstantValue()!));
        }
        
        return list;
    }

    /// <summary>
    /// 获取指定类型的枚举字段列表。
    /// </summary>
    /// <remarks>
    /// Name 显示 <see cref="DisplayAttribute"/> 描述，若没有，这显示 <see cref="DescriptionAttribute"/>，再没有则显示字段名称；Value 为字段名称。
    /// </remarks>
    /// <returns></returns>
    public static List<NameValue> ToNameValueList2<T>()
         where T : Enum
    {
        return ToNameValueList2(typeof(T));
    }

    /// <summary>
    /// 获取指定类型的枚举字段列表
    /// </summary>
    /// <param name="enumType">枚举类型</param>
    /// <remarks>
    /// Name 显示 <see cref="DisplayAttribute"/> 描述，若没有，这显示 <see cref="DescriptionAttribute"/>，再没有则显示字段名称；Value 为字段名称。
    /// </remarks>
    /// <returns></returns>
    public static List<NameValue> ToNameValueList2(Type enumType)
    {
        var fields = enumType.GetFields().Where(s => s.FieldType.IsEnum).OrderBy(s => s.GetRawConstantValue()).ToList();
        List<NameValue> list = new(fields.Count);

        foreach (var field in fields)
        {
            var attr0 = field.GetCustomAttribute<DisplayAttribute>(false);
            if (attr0 != null)
            {
                list.Add(new NameValue(attr0!.GetName() ?? "", field.Name));
                continue;
            }

            var attr1 = field.GetCustomAttribute<DescriptionAttribute>(false);
            list.Add(new NameValue(attr1?.Description ?? field.Name, field.Name));
        }

        return list;
    }

    /// <summary>
    /// 将枚举类型转换为下拉框的 KV 对象。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="hasEmpty">是否有空行</param>
    /// <returns></returns>
    public static List<NameValue> ToDropdownList<T>(bool hasEmpty = true)
        where T : Enum
    {
        return ToDropdownList(typeof(T), hasEmpty);
    }

    /// <summary>
    /// 将枚举类型转换为下拉框的 KV 对象。
    /// </summary>
    /// <param name="enumType">枚举类型</param>
    /// <param name="hasEmpty">是否有空行</param>
    /// <returns></returns>
    public static List<NameValue> ToDropdownList(Type enumType, bool hasEmpty = true)
    {
        var items = ToNameValueList2(enumType);
        if (hasEmpty)
        {
            items.Insert(0, new("", ""));
        }
        return items;
    }

    /// <summary>
    /// 获取枚举类型的 <see cref="DescriptionAttribute"/> 描述，没有则为 null。
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public static string? Desc(this Enum source)
    {
        var fi = source.GetType().GetField(source.ToString());
        var attr = fi!.GetCustomAttribute<DescriptionAttribute>(false);
        return attr?.Description;
    }

    /// <summary>
    /// 获取枚举类型的 <see cref="DisplayAttribute"/> 描述，没有则为 null。
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public static string? DisplayName(this Enum source)
    {
        var fi = source.GetType().GetField(source.ToString());
        var attr = fi!.GetCustomAttribute<DisplayAttribute>(false);
        return attr?.GetName();
    }

    /// <summary>
    /// 获取枚举类型的 <see cref="DescriptionAttribute"/> 描述，没有则为 null。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="name">枚举名称</param>
    /// <returns></returns>
    public static string? GetDesc<T>(string name)
    {
        var fi = typeof(T).GetField(name);
        var attr = fi!.GetCustomAttribute<DescriptionAttribute>(false);
        return attr?.Description;
    }

    /// <summary>
    /// 获取枚举类型的 <see cref="DisplayAttribute"/> 描述，没有则为 null。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="name">枚举名称</param>
    /// <returns></returns>
    public static string? GetDisplayName<T>(string name)
    {
        var fi = typeof(T).GetField(name);
        var attr = fi!.GetCustomAttribute<DisplayAttribute>(false);
        return attr?.GetName();
    }

    /// <summary>
    /// 校验类型是否为枚举类型
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static bool IsEnum(Type type)
    {
        return type.IsEnum || (type.IsConstructedGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>) && type.GetGenericArguments()[0].IsEnum);
    }

    /// <summary>
    /// 通过值（如 int）转换为枚举类型。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="InvalidCastException"></exception>
    public static T Parse<T>(object value)
        where T : Enum
    {
        return (T)Parse(typeof(T), value);
    }

    /// <summary>
    /// 通过值（如 int）转换为枚举类型。
    /// </summary>
    /// <param name="enumType">类型</param>
    /// <param name="value">值</param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="InvalidCastException"></exception>
    public static object Parse(Type enumType, object value)
    {
        if (!IsEnum(enumType))
        {
            throw new InvalidOperationException($"'{enumType.FullName}' 不是枚举类型");
        }

        // 注：枚举类型字段必须筛选出字面量（有隐藏字段），且 object 类型之间不能直接进行比较。
        var name = enumType.GetFields().FirstOrDefault(s => s.IsLiteral && s.GetRawConstantValue()?.ToString() == value.ToString())?.Name;
        if (name == null)
        {
            throw new InvalidCastException($"枚举类型 '{enumType.FullName}' 中没有指定的值 '{value}'");
        }

        return Enum.Parse(enumType, name);
    }
}
