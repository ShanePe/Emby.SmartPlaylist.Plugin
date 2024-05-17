using System;

namespace SmartPlaylist.Extensions
{
    public static class EnumExtensions
    {
        public static T GetAttributeOfType<T>(this Enum enumVal) where T : System.Attribute
        {
            var type = enumVal.GetType();
            var memInfo = type.GetMember(enumVal.ToString());
            var attributes = memInfo[0].GetCustomAttributes(typeof(T), false);
            return (attributes.Length > 0) ? (T)attributes[0] : null;
        }

        public static T ConvertFromString<T>(this Enum enumVal, string convert, T defaultReturn) where T : struct, IComparable, IConvertible, IFormattable
        {
            if (Enum.TryParse(convert, true, out T e)) return e;
            return defaultReturn;
        }
    }
}