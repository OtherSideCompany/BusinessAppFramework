using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessAppFramework.Contracts
{
    public static class EnumKeys
    {
        public static string For(Type enumType, object value)
            => $"{KebabStringFormatter.ToKebab(enumType.Name)}-{KebabStringFormatter.ToKebab(value.ToString()!)}";
    }

    public static class EnumKeys<TEnum> where TEnum : struct, Enum
    {
        public static string For(TEnum value) => EnumKeys.For(typeof(TEnum), value);

        public static IEnumerable<(TEnum Value, string Key)> All()
            => Enum.GetValues<TEnum>().Select(v => (v, For(v)));
    }
}
