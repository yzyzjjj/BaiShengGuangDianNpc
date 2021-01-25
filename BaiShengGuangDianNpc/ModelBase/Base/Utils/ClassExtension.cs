using ModelBase.Base.Logic;
using System;
using System.Linq;

namespace ModelBase.Base.Utils
{
    public class ClassExtension
    {
        public static TChildClass ParentCopyToChild<TParentClass, TChildClass>(TParentClass parent)
            where TParentClass : class, new()
            where TChildClass : TParentClass, new()
        {
            var child = new TChildClass();
            var parentType = typeof(TParentClass);
            var properties = parentType.GetProperties();
            foreach (var property in properties)
            {
                if (property.CanRead && property.CanWrite)
                {
                    property.SetValue(child, property.GetValue(parent, null), null);
                }
            }

            return child;
        }

        public static bool HaveChange<TClass>(TClass oldObj, TClass newObj) where TClass : class
        {
            var thisProperties = oldObj.GetType().GetProperties();
            var properties = newObj.GetType().GetProperties();
            foreach (var propInfo in typeof(TClass).GetProperties())
            {
                var attr = (IgnoreChangeAttribute)propInfo.GetCustomAttributes(typeof(IgnoreChangeAttribute), false).FirstOrDefault();
                if (attr != null)
                {
                    continue;
                }

                var thisValue = thisProperties.First(x => x.Name == propInfo.Name).GetValue(oldObj);
                var value = properties.First(x => x.Name == propInfo.Name).GetValue(newObj);
                if (propInfo.PropertyType == typeof(DateTime))
                {
                    if ((DateTime)thisValue != (DateTime)value)
                    {
                        return true;
                    }
                }
                else if (propInfo.PropertyType == typeof(decimal))
                {
                    if ((decimal)thisValue != (decimal)value)
                    {
                        return true;
                    }
                }
                else
                {
                    var oldValue = thisValue?.ToString() ?? "";
                    var newValue = value?.ToString() ?? "";
                    if (oldValue != newValue)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

    }
}
