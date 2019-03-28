using System;

namespace ModelBase.Base.Utils
{
    public static class MathHelper
    {
        /// <summary>
        /// 舍弃
        /// </summary>
        /// <param name="num"></param>
        /// <param name="cnt">保留几位小数</param>
        /// <returns></returns>
        public static decimal ToFloor(this decimal num, int cnt = 2)
        {
            var point = Math.Pow(0.1, cnt);
            return num - num % (decimal)point;
        }
        /// <summary>
        /// 四舍五入
        /// </summary>
        /// <param name="num"></param>
        /// <param name="cnt">保留几位小数</param>
        /// <returns></returns>
        public static decimal ToRound(this decimal num, int cnt = 2)
        {
            return Math.Round(num, cnt, MidpointRounding.AwayFromZero);
        }

        /// <summary>
        /// 舍弃
        /// </summary>
        /// <param name="num"></param>
        /// <param name="cnt">保留几位小数</param>
        /// <returns></returns>
        public static double ToFloor(this double num, int cnt = 2)
        {
            var point = Math.Pow(0.1, cnt);
            return num - num % (double)point;
        }
        /// <summary>
        /// 四舍五入
        /// </summary>
        /// <param name="num"></param>
        /// <param name="cnt">保留几位小数</param>
        /// <returns></returns>
        public static double ToRound(this double num, int cnt = 2)
        {
            return Math.Round(num, cnt, MidpointRounding.AwayFromZero);
        }
    }
}