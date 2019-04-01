using Microsoft.Extensions.Configuration;
using ServiceStack;
using System;
using System.Globalization;

namespace ModelBase.Base.Utils
{
    public static class ConfigurationExtension
    {
        public static T GetAppSettings<T>(this IConfiguration configuration, string name) where T : IComparable
        {
            var val = configuration?.GetSection("AppSettings")?[name];
            if (val.IsNullOrEmpty()) return (T) typeof(T).GetDefaultValue();
            var effectiveType = typeof(T);
            var convertToType = Nullable.GetUnderlyingType(effectiveType) ?? effectiveType;
            return (T) Convert.ChangeType(val, convertToType, CultureInfo.InvariantCulture);
        }

    }
}
