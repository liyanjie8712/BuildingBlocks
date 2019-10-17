using System.Linq;
using System.Text.RegularExpressions;

using Liyanjie.Utilities.Cn;

namespace System
{
    /// <summary>
    /// 
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// 是否为手机号码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsPhoneNumber_Cn(this string input)
        {
            if (input == null || input == string.Empty || input.Length != 11)
                return false;

            return Regex.IsMatch(input, $"^{Consts.RegexPattern_PhoneNumber}$", RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 使用字符遮挡中间部分，只留前6位与后4位
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="char"></param>
        /// <returns></returns>
        public static string HidePhoneNumber_Cn(this string origin, char @char = '*')
        {
            return $"{origin.Substring(0, 3)}{@char}{@char}{@char}{@char}{origin.Substring(origin.Length - 4, 4)}";
        }

        /// <summary>
        /// 是否为身份证号
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsIdNumber_Cn(this string input)
        {
            if (string.IsNullOrWhiteSpace(input) || input.Length != 18) // 非18位为假 
                return false;

            if (!Regex.IsMatch(input, $"^{Consts.RegexPattern_IdNumber}$", RegexOptions.IgnoreCase))
                return false;

            var int1_17 = input.Substring(0, 17).ToCharArray().Select(m => int.Parse(m.ToString())).ToList();
            var code18 = input.Substring(17, 1);

            var verify = 0;
            var xxx = new int[] { 7, 9, 10, 5, 8, 4, 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2 };
            for (int i = 0; i < int1_17.Count; i++)
            {
                verify += int1_17[i] * xxx[i];
            }
            verify = verify % 11;

            var verifycode = string.Empty;
            switch (verify)
            {
                case 0:
                    verifycode = "1";
                    break;
                case 1:
                    verifycode = "0";
                    break;
                case 2:
                    verifycode = "X";
                    break;
                case 3:
                case 4:
                case 5:
                case 6:
                case 7:
                case 8:
                case 9:
                case 10:
                    verifycode = (12 - verify).ToString();
                    break;
                default:
                    break;
            }

            if (string.IsNullOrWhiteSpace(verifycode))
                return false;

            if (!verifycode.Equals(code18, StringComparison.CurrentCultureIgnoreCase))  // 将身份证的第18位与算出来的校码进行匹配，不相等就为假 
                return false;

            return true;
        }

        /// <summary>
        /// 使用字符遮挡中间部分，只留前6位与后4位
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="char"></param>
        /// <returns></returns>
        public static string HideIdNumber_Cn(this string origin, char @char = '*')
        {
            return $"{origin.Substring(0, 6)}{@char}{@char}{@char}{@char}{origin.Substring(origin.Length - 4, 4)}";
        }

        /// <summary>
        /// 获取拼音
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static (string Pinyin, bool IsPinyin)[] GetPinyin(this string input)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            if (input.Length == 0)
                return new(string Pinyin, bool IsPinyin)[0];

            return input.ToCharArray().Select(_ => _.GetPinyin()).ToArray();
        }

        /// <summary>
        /// 获取拼音
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        static (string Pinyin, bool IsPinyin) GetPinyin(this char input)
        {
            if ((input > 47 && input < 58) || (input > 64 && input < 91) || (input > 96 && input < 123))
                return (input.ToString(), false);

            var pinyin = PinyinHelper.Dictionary.FirstOrDefault(_ => _.Value.IndexOf(input) > -1).Key;
            if (pinyin != null)
                return (pinyin, true);

            return ("*", false);
        }
    }
}
