using System.Linq;
using System.Text;

namespace System
{
    public enum CnNumberType
    {
        /// <summary>
        /// 12345.678 &gt;&gt; 一万二千三百四十五点六七八
        /// </summary>
        Normal,

        /// <summary>
        /// 12345.678 &gt;&gt; 壹万贰仟叁佰肆拾伍点陆柒捌
        /// </summary>
        NormalUpper,

        /// <summary>
        /// 12345.678 &gt;&gt; 一二三四五点六七八
        /// </summary>
        Direct,

        /// <summary>
        /// 12345.678 &gt;&gt; 壹贰叁肆伍点陆柒捌
        /// </summary>
        DirectUpper,

        /// <summary>
        /// 12345.678 &gt;&gt; 壹万贰仟叁佰肆拾伍圆陆角柒分捌厘
        /// </summary>
        Currency,
    }

    /// <summary>
    /// 数值类型的扩展方法
    /// </summary>
    public static class NumberExtensions
    {
        /// <summary>
        /// 将数字转换成中文表示形式
        /// </summary>
        /// <param name="number"></param>
        /// <param name="numberType">Normal:中文数字,Currency:中文货币,Direct:仅转中文</param>
        /// <returns></returns>
        public static string ToCn(this short number, CnNumberType numberType = default) => ToCn((double)number, numberType);

        /// <summary>
        /// 将数字转换成中文表示形式
        /// </summary>
        /// <param name="number"></param>
        /// <param name="numberType">Normal:中文数字,Currency:中文货币,Direct:仅转中文</param>
        /// <returns></returns>
        public static string ToCn(this int number, CnNumberType numberType = default) => ToCn((double)number, numberType);

        /// <summary>
        /// 将数字转换成中文表示形式
        /// </summary>
        /// <param name="number"></param>
        /// <param name="numberType">Normal:中文数字,Currency:中文货币,Direct:仅转中文</param>
        /// <returns></returns>
        public static string ToCn(this long number, CnNumberType numberType = default) => ToCn((double)number, numberType);

        /// <summary>
        /// 将数字转换成中文表示形式
        /// </summary>
        /// <param name="number"></param>
        /// <param name="numberType">Normal:中文数字,Currency:中文货币,Direct:仅转中文</param>
        /// <returns></returns>
        public static string ToCn(this float number, CnNumberType numberType = default) => ToCn((double)number, numberType);

        /// <summary>
        /// 将数字转换成中文表示形式
        /// </summary>
        /// <param name="number"></param>
        /// <param name="numberType">Normal:中文数字,Currency:中文货币,Direct:仅转中文</param>
        /// <returns></returns>
        public static string ToCn(this double number, CnNumberType numberType = default)
        {
            var @long = (long)Math.Truncate(number);
            var @decimal = @long - number == 0d ? null : number.ToString();
            return ToCn(@long, @decimal, numberType);
        }

        /// <summary>
        /// 将数字转换成中文表示形式
        /// </summary>
        /// <param name="number"></param>
        /// <param name="numberType">Normal:中文数字,Currency:中文货币,Direct:仅转中文</param>
        /// <returns></returns>
        public static string ToCn(this decimal number, CnNumberType numberType = default)
        {
            var @long = (long)Math.Truncate(number);
            var @decimal = @long - number == 0m ? null : number.ToString();
            return ToCn(@long, @decimal, numberType);
        }

        /// <summary>
        /// 将 0-9，10，100，1000，10000，100000000等数字转换成中文
        /// </summary>
        /// <param name="number"></param>
        /// <param name="uppercase">true：大写</param>
        /// <returns></returns>
        public static string ToCnNumber(this long number, bool uppercase = false)
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
            return ToCnNumber_(number, uppercase);
        }

        static string ToCn(long number, string @decimal, CnNumberType numberType)
        {
            var sb = new StringBuilder();

            if (number < 0)
                sb.Append("负");
            number = Math.Abs(number);

            sb.Append(numberType switch
            {
                CnNumberType.Normal => ProcessLong(number),
                CnNumberType.NormalUpper => ProcessLong(number, uppercase: true),
                CnNumberType.Direct => string.Join(string.Empty, number.ToString().Select(_ => ToCnNumber_(long.Parse(_.ToString()), false))),
                CnNumberType.DirectUpper => string.Join(string.Empty, number.ToString().Select(_ => ToCnNumber_(long.Parse(_.ToString()), true))),
                CnNumberType.Currency => ProcessLong(number, uppercase: true),
                _ => string.Empty,
            });

            if (!string.IsNullOrEmpty(@decimal))
            {
                sb.Append(numberType switch
                {
                    CnNumberType.Normal => "点",
                    CnNumberType.NormalUpper => "点",
                    CnNumberType.Direct => "点",
                    CnNumberType.DirectUpper => "点",
                    CnNumberType.Currency => "圆",
                    _ => string.Empty,
                });

                var index = @decimal.IndexOf('.');
                if (index > 0)
                    @decimal = @decimal.Substring(index + 1);

                sb.Append(numberType switch
                {
                    CnNumberType.Normal => string.Join(string.Empty, @decimal.Select(_ => ToCnNumber_(long.Parse(_.ToString()), false))),
                    CnNumberType.NormalUpper => string.Join(string.Empty, @decimal.Select(_ => ToCnNumber_(long.Parse(_.ToString()), true))),
                    CnNumberType.Direct => string.Join(string.Empty, @decimal.Select(_ => ToCnNumber_(long.Parse(_.ToString()), false))),
                    CnNumberType.DirectUpper => string.Join(string.Empty, @decimal.Select(_ => ToCnNumber_(long.Parse(_.ToString()), true))),
                    CnNumberType.Currency => ProcessDecimal(@decimal),
                    _ => string.Empty,
                });
            }
            else if (numberType == CnNumberType.Currency)
                sb.Append("圆整");

            return sb.ToString();

            static string ProcessLong(long number, bool uppercase = false)
            {
                var sb = new StringBuilder();
                var zeroFlag = false;//输出0
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
                                        sb.Append(ToCnNumber(tmp, uppercase));
                                        if (j > 1)
                                            sb.Append(ToCnNumber(j, uppercase));
                                        zeroFlag = true;
                                    }
                                    else if (zeroFlag)
                                    {
                                        sb.Append(ToCnNumber(0L, uppercase));
                                        zeroFlag = false;
                                    }

                                    if (j == 1)
                                        break;

                                    number %= j;
                                    j /= 10;
                                }
                            }
                            else if (number >= 10)
                            {
                                sb.Append(ToCnNumber(10L, uppercase));
                                if (number % 10 > 0)
                                {
                                    sb.Append(ToCnNumber(number % 10, uppercase));
                                    zeroFlag = true;
                                }
                            }
                            else
                                sb.Append(ToCnNumber(number, uppercase));

                            if (level > 1)
                                sb.Append(ToCnNumber(level, uppercase));
                        }
                        number = l;
                    }
                }
                else
                    sb.Append(ToCnNumber(number, uppercase));
                return sb.ToString();
            }
            static string ProcessDecimal(string @decimal)
            {
                var sb = new StringBuilder();
                var zeroFlag = false;//输出0
                for (int i = 0; i < @decimal.Length; i++)
                {
                    var c = @decimal[i].ToString();
                    var d = long.Parse(c.ToString());

                    if (d > 0)
                    {
                        sb.Append(ToCnNumber_(d, true));
                        sb.Append(Unit(i));
                        zeroFlag = true;
                    }
                    else if (@decimal.Length > i + 1 && zeroFlag)
                    {
                        sb.Append(ToCnNumber_(0L, true));
                        zeroFlag = false;
                    }
                }
                return sb.ToString();

                static string Unit(int i)
                {
                    return i switch
                    {
                        0 => "角",
                        1 => "分",
                        2 => "厘",
                        _ => string.Empty,
                    };
                }
            }
        }
        static string ToCnNumber_(long number, bool uppercase)
        {
            if (number > 100000000)
                number /= 100000000;

            return number switch
            {
                0 => uppercase ? "零" : "〇",
                1 => uppercase ? "壹" : "一",
                2 => uppercase ? "贰" : "二",
                3 => uppercase ? "叁" : "三",
                4 => uppercase ? "肆" : "四",
                5 => uppercase ? "伍" : "五",
                6 => uppercase ? "陆" : "六",
                7 => uppercase ? "柒" : "七",
                8 => uppercase ? "捌" : "八",
                9 => uppercase ? "玖" : "九",
                10 => uppercase ? "拾" : "十",
                100 => uppercase ? "佰" : "百",
                1000 => uppercase ? "仟" : "千",
                10000 => uppercase ? "万" : "万",
                100000000 => uppercase ? "亿" : "亿",
                _ => string.Empty,
            };
        }
    }
}