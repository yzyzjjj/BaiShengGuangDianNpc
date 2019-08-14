using System.Text.RegularExpressions;

namespace ModelBase.Base.Utils
{
    public static class RegexString
    {
        private static readonly string _En = @"[^a-zA-Z]";
        private static readonly string _Num = @"[^0-9]";
        private static readonly string _Cha = @"\W";

        /// <summary>
        /// 字符串是否是英文和数字的组合
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsNumAndEn(this string input)
        {
            var ret = Regex.IsMatch(input, _En) && Regex.IsMatch(input, _Num) && !Regex.IsMatch(input, _Cha);
            return ret;
        }

        public static bool IsNum(this string input)
        {
            var ret = !Regex.IsMatch(input, @"[^0-9]");
            return ret;
        }

        private static string sRxStr = @"[\;|\<|\>|\<\=|\>\=|\<\>|\'|\""|\?|\#|\|\*|\\|\,]";
        public static bool HasSQLInject(this string contents)
        {
            return contents.Length > 0 && Regex.IsMatch(contents, sRxStr);
        }



        //static string phoneRegexStr = "^((13[0-9])|(14[5|7])|(15([0-3]|[5-9]))|(18[0,5-9]))\\d{8}$";
        private static string phoneNum = "^\\d{11}$";
        /// <summary>
        ///  移动号码段:139、138、137、136、135、134、150、151、152、157、158、159、182、183、187、188、147
        ///  联通号码段:130、131、132、136、185、186、145
        ///  电信号码段:133、153、180、189
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public static bool IsPhone(string phone)
        {
            return phone.Length > 0 && Regex.IsMatch(phone, phoneNum);
        }

        /// <summary>
        /// 检查是不是电话号码格式
        /// </summary>
        /// <param name="telephone"></param>
        /// <returns></returns>
        public static bool IsTelePhone(string telephone)
        {
            return telephone.Length > 0 && Regex.IsMatch(telephone, @"^(\(\d{3,4}\)|\d{3,4}-)?\d{6,14}$");
        }

        /// <summary>
        /// 检查字段是否为手机或者固定电话
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public static bool IsPhoneOrTel(string phone)
        {
            return IsPhone(phone) || IsTelePhone(phone);
        }

        /// <summary>
        /// 判断是否是身份证
        /// </summary>
        /// <param name="cardid"></param>
        /// <returns></returns>
        public static bool IsCardId(string cardid)
        {
            return Regex.IsMatch(cardid, @"^(^\d{15}$|^\d{18}$|^\d{17}(\d|X|x))$", RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 姓名的正则表达式,中文和英文
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool IsName(string name)
        {
            return Regex.IsMatch(name, @"^[\u4E00-\u9FA5·]+$", RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 公司名称 的正则表达式,中文和英文
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool IsCompanyName(string name)
        {
            return Regex.IsMatch(name, @"^[\u4E00-\u9FA5·（）()]+$", RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 判断统一社会信用代码 格式
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool IsCompanyUnionId(string uid)
        {
            return uid.Length == 18;
        }

        /// <summary>
        /// 判断字符串是否为中文
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool IsChinese(string name)
        {
            return Regex.IsMatch(name, @"^[\u4E00-\u9FA5]+$", RegexOptions.IgnoreCase);
        }

        //非负浮点数
        public static bool IsNumber(string num)
        {
            Regex reg = new Regex(@"^\d+(\.\d+)?$");
            return reg.IsMatch(num);
        }
        //邮箱
        public static bool IsEmail(string mail)
        {
            var reg = new Regex(@"^[A-Za-z0-9\u4e00-\u9fa5]+@[a-zA-Z0-9_-]+(\.[a-zA-Z0-9_-]+)+$");
            return reg.IsMatch(mail);
        }
    }
}

