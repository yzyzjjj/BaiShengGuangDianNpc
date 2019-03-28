using System.Collections.Generic;
using System.Linq;

namespace ModelBase.Base.Utils
{
    public static class CollectionExtension
    {
        /// <summary>
        /// 将集合内容按照字典升序排列，
        /// 并按照key=value通过“&”拼接起来
        /// </summary>
        /// <param name="dictionary"></param>
        /// <returns></returns>
        public static string GetSignContentOrderByKey(this Dictionary<string, string> dictionary)
        {
            var dict = dictionary.OrderBy(d => d.Key).Select(d => string.Format("{0}={1}", d.Key, d.Value));
            string s = string.Join("&", dict);
            return s;
        }

        public static string GetSignContentOrderByKeyWithoutKey(this Dictionary<string, string> dictionary)
        {
            var dict = dictionary.OrderBy(d => d.Key).Select(d => string.Format("{0}", d.Value));
            string s = string.Join("", dict);
            return s;
        }
        /// <summary>
        /// 获取与指定的键相关联的值，如果没有则返回输入的默认值
        /// </summary>
        public static TValue GetValue<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue defaultValue = default(TValue))
        {
            return dict.ContainsKey(key) ? dict[key] : defaultValue;
        }

        /// <summary>
        /// 向字典中批量添加键值对
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dict"></param>
        /// <param name="values"></param>
        /// <param name="replaceExisted">如果已存在，是否替换</param>
        /// <returns></returns>
        public static Dictionary<TKey, TValue> AddRange<TKey, TValue>(this Dictionary<TKey, TValue> dict, IEnumerable<KeyValuePair<TKey, TValue>> values, bool replaceExisted = true)
        {
            foreach (var item in values)
            {
                if (dict.ContainsKey(item.Key) == false || replaceExisted)
                    dict[item.Key] = item.Value;
            }
            return dict;
        }

        /// <summary>
        /// 尝试将键和值添加到字典中：如果不存在，才添加；存在，不添加也不抛导常
        /// <returns>true:添加成功,false:未添加(已存在)</returns>
        /// </summary>
        public static bool TryAdd<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue value)
        {
            if (dict.ContainsKey(key))
                return false;

            dict.Add(key, value);
            return true;
        }

        /// <summary>
        /// 将键和值添加或替换到字典中：如果不存在，则添加；存在，则替换
        /// </summary>
        public static Dictionary<TKey, TValue> AddOrReplace<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue value)
        {
            dict[key] = value;
            return dict;
        }
    }
}