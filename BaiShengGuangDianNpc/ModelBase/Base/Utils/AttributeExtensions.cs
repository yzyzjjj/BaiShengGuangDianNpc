using System;
using System.Linq;

namespace ModelBase.Base.Utils
{
    public static class AttributeExtensions
    {
        public static T GetAttribute<T>(this Enum value)
        {
            return (T)value.GetType().GetField(value.ToString()).GetCustomAttributes(typeof(T), false).FirstOrDefault();
        }
        public static T GetAttribute<T>(this object value)
        {
            return (T)value.GetType().GetCustomAttributes(typeof(T), false).FirstOrDefault();
        }
        public static T GetAttribute<T>(this Type type)
        {
            return (T)type.GetCustomAttributes(typeof(T), false).FirstOrDefault();
        }
    }
}