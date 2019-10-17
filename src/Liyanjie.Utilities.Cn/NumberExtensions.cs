using System.Text;

namespace System
{
    /// <summary>
    /// 数值类型的扩展方法
    /// </summary>
    public static class NumberExtensions
    {
        /// <summary>
        /// 将数字转换成中文表示形式
        /// </summary>
        /// <param name="number"></param>
        /// <param name="isCurrency">是否输出中文大写货币</param>
        /// <returns></returns>
        public static string ToCN(this short number, bool isCurrency = false)
            => ToCN((double)number, isCurrency);

        /// <summary>
        /// 将数字转换成中文表示形式
        /// </summary>
        /// <param name="number"></param>
        /// <param name="isCurrency">是否输出中文大写货币</param>
        /// <returns></returns>
        public static string ToCN(this int number, bool isCurrency = false)
            => ToCN((double)number, isCurrency);

        /// <summary>
        /// 将数字转换成中文表示形式
        /// </summary>
        /// <param name="number"></param>
        /// <param name="isCurrency">是否输出中文大写货币</param>
        /// <returns></returns>
        public static string ToCN(this long number, bool isCurrency = false)
            => ToCN((double)number, isCurrency);

        /// <summary>
        /// 将数字转换成中文表示形式
        /// </summary>
        /// <param name="number"></param>
        /// <param name="isCurrency">是否输出中文大写货币</param>
        /// <returns></returns>
        public static string ToCN(this float number, bool isCurrency = false)
            => ToCN(number, isCurrency);

        /// <summary>
        /// 将数字转换成中文表示形式
        /// </summary>
        /// <param name="number"></param>
        /// <param name="isCurrency">是否输出中文大写货币</param>
        /// <returns></returns>
        public static string ToCN(this double number, bool isCurrency = false)
        {
            return _ToCN(number, isCurrency);
        }

        /// <summary>
        /// 将数字转换成中文表示形式
        /// </summary>
        /// <param name="number"></param>
        /// <param name="isCurrency">是否输出中文大写货币</param>
        /// <returns></returns>
        public static string ToCN(this decimal number, bool isCurrency = false)
        {
            return _ToCN(number, isCurrency);
        }

        /// <summary>
        /// 将 0-9，10，100，1000，10000，100000000等数字转换成中文
        /// </summary>
        /// <param name="number"></param>
        /// <param name="isCurrency">true：中文货币大写</param>
        /// <returns></returns>
        public static string ToCNNumber(this long number, bool isCurrency = false)
        {
            if (number >= 100000000)
                number = 100000000;
            else if (number >= 10000)
                number = 10000;
            else if (number >= 1000)
                number = 1000;
            else if (number >= 100)
                number = 100;
            else if (number >= 10)
                number = 10;
            else if (number < 0)
                number = 0;
            return _ToCNNumber(number, isCurrency);
        }

        static string _ToCN(double number, bool isCurrency)
        {
            return _ToCN(number.ToString(), (long)Math.Truncate(number), isCurrency);
        }

        static string _ToCN(decimal number, bool isCurrency)
        {
            return _ToCN(number.ToString(), (long)Math.Truncate(number), isCurrency);
        }

        static string _ToCN(string numberString, long number, bool isCurrency)
        {
            var dec = string.Empty;

            var index = numberString.IndexOf('.');
            if (index >= 0)
                dec = numberString.Substring(index + 1);

            var sb = new StringBuilder();

            if (number < 0)
                sb.Append("负");

            var outputZero = false;//输出0

            if (number > 1)
            {
                var l = number;
                for (int ii = 4; ii >= 0; ii--)
                {
                    var level = (long)Math.Pow(10000, ii);
                    if (number >= level)
                    {
                        l = number % level;
                        number /= level;
                        if (number > 19)
                        {
                            var j = 1000;
                            while (number % (j * 10) >= 1)
                            {
                                var tmp = number / j;

                                if (tmp != 0)
                                {
                                    sb.Append(_ToCNNumber(tmp, isCurrency));
                                    if (j > 1)
                                        sb.Append(_ToCNNumber(j, isCurrency));
                                    outputZero = true;
                                }
                                else if (outputZero)
                                {
                                    sb.Append(_ToCNNumber(0L, isCurrency));
                                    outputZero = false;
                                }

                                if (j == 1)
                                    break;

                                number %= j;
                                j /= 10;
                            }
                        }
                        else if (number >= 10)
                        {
                            sb.Append(_ToCNNumber(10L, isCurrency));
                            if (number % 10 > 0)
                            {
                                sb.Append(_ToCNNumber(number % 10, isCurrency));
                                outputZero = true;
                            }
                        }
                        else
                            sb.Append(_ToCNNumber(number, isCurrency));

                        if (level > 1)
                            sb.Append(_ToCNNumber(level, isCurrency));
                    }
                    number = l;
                }
            }
            else if (!isCurrency)
                sb.Append(_ToCNNumber(number, isCurrency));

            if (!string.IsNullOrEmpty(dec))
            {
                #region 处理小数部分

                if (isCurrency)
                {
                    if (sb.Length > 0)
                        sb.Append("圆");

                    //if (outputZero)
                    //    sb.Append(toCNNumber(0L, isCurrency));

                    for (int i = 0; i < dec.Length; i++)
                    {
                        var c = dec[i].ToString();
                        var d = long.Parse(c.ToString());

                        if (d > 0)
                        {
                            sb.Append(_ToCNNumber(d, isCurrency));
                            sb.Append(_Unit(i));
                            outputZero = true;
                        }
                        else if (dec.Length > i + 1 && outputZero)
                        {
                            sb.Append(_ToCNNumber(0L, isCurrency));
                            outputZero = false;
                        }
                    }
                }
                else
                {
                    sb.Append("点");
                    foreach (char c in dec)
                    {
                        sb.Append(_ToCNNumber(long.Parse(c.ToString()), isCurrency));
                    }
                }

                #endregion
            }
            else if (isCurrency)
                sb.Append("圆整");

            return sb.ToString();
        }

        static string _Unit(int i)
        {
            var output = string.Empty;
            switch (i)
            {
                case 0:
                    output = "角";
                    break;
                case 1:
                    output = "分";
                    break;
                case 2:
                    output = "厘";
                    break;
                default:
                    break;
            }
            return output;
        }

        static string _ToCNNumber(long number, bool isCurrency)
        {
            if (number > 100000000)
                number /= 100000000;

            switch (number)
            {
                case 0:
                    return isCurrency ? "零" : "〇";
                case 1:
                    return isCurrency ? "壹" : "一";
                case 2:
                    return isCurrency ? "贰" : "二";
                case 3:
                    return isCurrency ? "叁" : "三";
                case 4:
                    return isCurrency ? "肆" : "四";
                case 5:
                    return isCurrency ? "伍" : "五";
                case 6:
                    return isCurrency ? "陆" : "六";
                case 7:
                    return isCurrency ? "柒" : "七";
                case 8:
                    return isCurrency ? "捌" : "八";
                case 9:
                    return isCurrency ? "玖" : "九";
                case 10:
                    return isCurrency ? "拾" : "十";
                case 100:
                    return isCurrency ? "佰" : "百";
                case 1000:
                    return isCurrency ? "仟" : "千";
                case 10000:
                    return isCurrency ? "万" : "万";
                case 100000000:
                    return isCurrency ? "亿" : "亿";
                default:
                    return string.Empty;
            }
        }
    }
}