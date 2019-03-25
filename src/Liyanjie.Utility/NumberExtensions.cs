using System.Collections.Generic;
using System.Linq;

namespace System
{
    /// <summary>
    /// 
    /// </summary>
    public static class NumberExtensions
    {
        /// <summary>
        /// 是否为2的N次方
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool Is2PowN(this short input)
        {
            return _Is2PowN(input);
        }

        /// <summary>
        /// 是否为2的N次方
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool Is2PowN(this int input)
        {
            return _Is2PowN(input);
        }

        /// <summary>
        /// 是否为2的N次方
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool Is2PowN(this long input)
        {
            return _Is2PowN(input);
        }

        static bool _Is2PowN(this long input)
        {
            return input < 1 ? false : ((input & (input - 1)) == 0) ? true : false;
        }

        /// <summary>
        /// 获取一个非负整型的所有正约数
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static List<short> Divisors(this short number)
        {
            return _Divisors(number).Cast<short>().ToList();
        }

        /// <summary>
        /// 获取一个非负整型的所有正约数
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static List<int> Divisors(this int number)
        {
            return _Divisors(number).Cast<int>().ToList();
        }

        /// <summary>
        /// 获取一个非负整型的所有正约数
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static List<long> Divisors(this long number)
        {
            return _Divisors(number);
        }

        static List<long> _Divisors(this long number)
        {
            if (number < 0)
                throw new ArgumentOutOfRangeException(nameof(number), $"{nameof(number)} 必须为非负整型！");

            if (number == 0)
                return new List<long> { 1 };

            var divisors = new List<long>(2);
            divisors.Add(1);

            for (long j = 2; j < number; j++)
            {
                if (number % j == 0)
                    divisors.Add(j);
            }

            if (number > 1)
                divisors.Add(number);
            return divisors;
        }
    }
}
