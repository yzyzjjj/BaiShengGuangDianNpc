using System;
using System.Collections.Generic;
using System.Text;

namespace ModelBase.Base.Utils
{
    public class ClassExtension
    {
        public static TCClass ParentCopyToChild<TPClass, TCClass>(TPClass parent) where TCClass : class, TPClass, new()
        {
            var child = new TCClass();
            var parentType = typeof(TPClass);
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
    }
}
