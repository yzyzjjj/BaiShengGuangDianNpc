using System.ComponentModel;
using ModelBase.Base.ServerConfig.Enum;
using ModelBase.Base.Utils;

namespace ModelBase.Models.Result
{
    public class Result
    {
        private Error _errno = Error.Success;
        public Error errno
        {
            get { return _errno; }
            set
            {
                _errno = value;
                var att = _errno.GetAttribute<DescriptionAttribute>();
                if (att != null)
                {
                    errmsg = att.Description;
                }
            }
        }

        public string errmsg = "成功";

        /// <summary>
        /// 通用的错误结果产生方法，result 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="err"></param>
        /// <returns></returns>
        public static T GenError<T>(Error err) where T : Result, new()
        {
            T res = new T
            {
                errno = err
            };
            return res;
        }

    }
}