using System.Collections.Generic;
using System.Linq;

using Liyanjie.Utilities;

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
            => _Is2PowN(input);

        /// <summary>
        /// 是否为2的N次方
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool Is2PowN(this int input)
            => _Is2PowN(input);

        /// <summary>
        /// 是否为2的N次方
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool Is2PowN(this long input)
            => _Is2PowN(input);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="radix"></param>
        /// <param name="radixCodes"></param>
        /// <returns></returns>
        public static string ToString(this short input, int radix, string radixCodes = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz")
            => RadixHelper.ToString(input, radix, radixCodes);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="radix"></param>
        /// <param name="radixCodes"></param>
        /// <returns></returns>
        public static string ToString(this int input, int radix, string radixCodes = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz")
            => RadixHelper.ToString(input, radix, radixCodes);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="radix"></param>
        /// <param name="radixCodes"></param>
        /// <returns></returns>
        public static string ToString(this long input, int radix, string radixCodes = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz")
            => RadixHelper.ToString(input, radix, radixCodes);

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
            => _Divisors(number).Select(_ => (short)_).ToList();

        /// <summary>
        /// 获取一个非负整型的所有正约数
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static List<int> Divisors(this int number)
            => _Divisors(number).Select(_ => (int)_).ToList();

        /// <summary>
        /// 获取一个非负整型的所有正约数
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static List<long> Divisors(this long number)
            => _Divisors(number);

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
