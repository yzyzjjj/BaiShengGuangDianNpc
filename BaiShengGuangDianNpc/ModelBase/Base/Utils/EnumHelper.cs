using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace ModelBase.Base.Utils
{
    public class EnumHelper
    {
        /// <summary>
        /// 将int转成相应的enum，失败返回false，成功返回true, 并out相应的enum
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="enumInt"></param>
        /// <param name="result"></param>
        /// <param name="defaultEnum"></param>
        /// <returns></returns>
        public static bool TryParseInt<TEnum>(int enumInt, out TEnum result, bool defaultEnum = false)
        {
            if (!Enum.IsDefined(typeof(TEnum), enumInt))
            {
                result = default(TEnum);
                return false;
            }
            else
            {
                result = (TEnum)Enum.ToObject(typeof(TEnum), enumInt);
                if (!defaultEnum && result.ToString().Equals(default(TEnum)?.ToString()))//不能用默认值
                {
                    return false;
                }
                return true;
            }
        }

        /// <summary>
        /// 将int字符串转成对应的enum，失败返回false，成功返回true, 并out相应的enum
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="enumIntStr"></param>
        /// <param name="result"></param>
        /// <param name="defaultEnum"></param>
        /// <returns></returns>
        public static bool TryParseIntStr<TEnum>(string enumIntStr, out TEnum result, bool defaultEnum = false)
        {
            int enumInt = Convert.ToInt32(enumIntStr);
            if (!Enum.IsDefined(typeof(TEnum), enumInt))
            {
                result = default(TEnum);
                return false;
            }
            else
            {
                result = (TEnum)Enum.ToObject(typeof(TEnum), enumInt);
                if (!defaultEnum && result.ToString().Equals(default(TEnum)?.ToString()))//不能用默认值
                {
                    return false;
                }
                return true;
            }
        }

        /// <summary>
        /// 将enum的字符串转成相对于的enum值
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="enumStr"></param>
        /// <param name="result"></param>
        /// <param name="defaultEnum"></param>
        /// <returns></returns>
        public static bool TryParseStr<TEnum>(string enumStr, out TEnum result, bool defaultEnum = false) where TEnum : struct
        {
            if (Enum.TryParse<TEnum>(enumStr, true, out result))
            {

                if (!defaultEnum && result.ToString().Equals(default(TEnum).ToString()))//不能用默认值
                {
                    return false;
                }
                return true;
            }

            result = default(TEnum);
            return false;
        }

        /// <summary>
        /// 将enum转换成enum定义的字符串
        /// </summary>
        /// <param name="enumValue"></param>
        /// <returns></returns>
        public static string EnumToStr(Enum enumValue)
        {

            return enumValue.ToString();
        }
        /// <summary>
        /// 枚举转成List
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<EnumEntity> EnumToList<T>(bool defaultEnum = false)
        {
            var list = new List<EnumEntity>();

            foreach (var e in Enum.GetValues(typeof(T)))
            {
                if (!defaultEnum && e.ToString().Equals(default(T)?.ToString()))//不能用默认值
                {
                    continue;
                }

                var m = new EnumEntity();
                //var attrs = e.GetType().GetField(e.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), true);
                //if (attrs.Length>0 && attrs[0] as DescriptionAttribute attr)
                var attr = (DescriptionAttribute)e.GetType().GetField(e.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), true).FirstOrDefault();
                if (attr != null)
                {
                    m.Description = attr.Description;
                }
                m.EnumValue = Convert.ToInt32(e);
                m.EnumName = e.ToString();
                list.Add(m);
            }
            return list;
        }

    }
    public class EnumEntity
    {
        /// <summary>
        /// 枚举的描述
        /// </summary>
        public string Description { set; get; }

        /// <summary>
        /// 枚举名称
        /// </summary>
        public string EnumName { set; get; }

        /// <summary>
        /// 枚举对象的值
        /// </summary>
        public int EnumValue { set; get; }
    }
}