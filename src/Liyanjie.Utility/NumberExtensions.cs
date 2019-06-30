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

        static bool _Is2PowN(long input)
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

        static List<long> _Divisors(long number)
        {
            if (number < 0)
                throw new ArgumentOutOfRangeException(nameof(number), $"{nameof(number)} 必须为非负整型！");

            if (number < 2)
                return new List<long> { 1 };

            var divisors = new List<long>(2);

            for (long i = 1; i <= number; i++)
            {
                if (divisors.Contains(i))
                    break;
                if (number % i == 0)
                {
                    divisors.Add(i);
                    divisors.Add(number / i);
                }
            }

            return divisors;
        }
    }
}
