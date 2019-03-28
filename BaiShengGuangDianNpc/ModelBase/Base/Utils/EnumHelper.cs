using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace ModelBase.Base.Utils
{
    public class EnumHelper
    {
        /// <summary>
        /// 将int转成相应的enum，失败返回false，成功返回true, 并out相应的enum
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="enumint"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool TryParseInt<TEnum>(int enumint, out TEnum result)
        {
            if (!Enum.IsDefined(typeof(TEnum), enumint))
            {
                result = default(TEnum);
                return false;
            }
            else
            {
                result = (TEnum)Enum.ToObject(typeof(TEnum), enumint);
                if (result.ToString().Equals(default(TEnum).ToString()))//不能用默认值
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
        /// <param name="enumintstr"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool TryParseIntStr<TEnum>(string enumintstr, out TEnum result)
        {
            int enumint = Convert.ToInt32(enumintstr);
            if (!Enum.IsDefined(typeof(TEnum), enumint))
            {
                result = default(TEnum);
                return false;
            }
            else
            {
                result = (TEnum)Enum.ToObject(typeof(TEnum), enumint);
                if (result.ToString().Equals(default(TEnum).ToString()))//不能用默认值
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
        /// <param name="enumstr"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool TryParseStr<TEnum>(string enumstr, out TEnum result) where TEnum : struct
        {
            if (Enum.TryParse<TEnum>(enumstr, true, out result))
            {

                if (result.ToString().Equals(default(TEnum).ToString()))//不能用默认值
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
        /// <param name="enumvalue"></param>
        /// <returns></returns>
        public static string EnumToStr(Enum enumvalue)
        {

            return enumvalue.ToString();
        }
        /// <summary>
        /// 枚举转成List
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<EnumberEntity> EnumToList<T>()
        {
            List<EnumberEntity> list = new List<EnumberEntity>();

            foreach (var e in Enum.GetValues(typeof(T)))
            {
                if (e.ToString().Equals(default(T).ToString()))//不能用默认值
                    continue;
                EnumberEntity m = new EnumberEntity();
                object[] objArr = e.GetType().GetField(e.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), true);
                if (objArr != null && objArr.Length > 0)
                {
                    DescriptionAttribute da = objArr[0] as DescriptionAttribute;
                    m.Desction = da.Description;
                }
                m.EnumValue = Convert.ToInt32(e);
                m.EnumName = e.ToString();
                list.Add(m);
            }
            return list;
        }

    }
    public class EnumberEntity
    {
        /// <summary>
        /// 枚举的描述
        /// </summary>
        public string Desction { set; get; }

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