using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Liyanjie.Utility;
#if NET45 || NETSTANDARD2_0
using System.Drawing;
using System.IO;
#endif

namespace System
{
    /// <summary>
    /// 
    /// </summary>
    public static class StringExtensions
    {
#if NET45 || NETSTANDARD2_0
        /// <summary>
        /// 从base64字符串中获取图片
        /// </summary>
        /// <param name="imageBase64String"></param>
        /// <returns></returns>
        public static Bitmap Decoded(this string imageBase64String)
        {
            return new Bitmap(new MemoryStream(Convert.FromBase64String(imageBase64String)));
        }
#endif

        /// <summary>
        /// Encode
        /// </summary>
        /// <param name="input"></param>
        /// <param name="encodeMode"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string Encoded(this string input, EncodeMode encodeMode, Encoding encoding = null)
            => BitConverter.ToString((encoding ?? Encoding.UTF8).GetBytes(input).Encoded(encodeMode));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string Encoded_Base64(this string input, Encoding encoding = null)
            => Convert.ToBase64String((encoding ?? Encoding.UTF8).GetBytes(input));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string Decoded_Base64(this string input, Encoding encoding = null)
            => (encoding ?? Encoding.UTF8).GetString(Convert.FromBase64String(input));

        /// <summary>
        /// Aes加密
        /// </summary>
        /// <param name="input"></param>
        /// <param name="key">长度32字符</param>
        /// <param name="iv">长度16字符，为空时使用ECB模式，非空时使用CBC模式</param>
        /// <returns></returns>
        public static string Encrypted_Aes(this string input, string key, string iv = null)
            => Encoding.UTF8.GetString(Encoding.UTF8.GetBytes(input).Encrypted_Aes(key, iv));

        /// <summary>
        /// Aes解密
        /// </summary>
        /// <param name="input"></param>
        /// <param name="key">长度32字符</param>
        /// <param name="iv">长度16字符，为空时使用ECB模式，非空时使用CBC模式</param>
        /// <returns></returns>
        public static string Decrypted_Aes(this string input, string key, string iv = null)
            => Encoding.UTF8.GetString(Encoding.UTF8.GetBytes(input).Decrypted_Aes(key, iv));

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="input"></param>
        /// <param name="iv">长度8字符</param>
        /// <param name="key">长度24字符</param>
        /// <returns></returns>
        public static string Encrypted_TripleDES(this string input, string key, string iv = null)
            => Encoding.UTF8.GetString(Encoding.UTF8.GetBytes(input).Encrypted_TripleDES(key, iv));

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="input"></param>
        /// <param name="iv">长度8字符</param>
        /// <param name="key">长度24字符</param>
        /// <returns></returns>
        public static string Decrypted_TripleDES(this string input, string key, string iv = null)
            => Encoding.UTF8.GetString(Encoding.UTF8.GetBytes(input).Decrypted_TripleDES(key, iv));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="publicKeyString">公钥</param>
        /// <returns></returns>
        public static string Encrypted_RSA(this string input, string publicKeyString)
            => Encoding.UTF8.GetString(Encoding.UTF8.GetBytes(input).Encrypted_RSA(publicKeyString));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="privateKeyString">私钥</param>
        /// <returns></returns>
        public static string Decrypted_RSA(this string input, string privateKeyString)
            => Encoding.UTF8.GetString(Encoding.UTF8.GetBytes(input).Decrypted_RSA(privateKeyString));

#if NETSTANDARD1_3
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="publicKeyString">公钥</param>
        /// <param name="encryptionPadding">OaepSHA1|OaepSHA256|OaepSHA384|OaepSHA512|Pkcs1</param>
        /// <returns></returns>
        public static string Encrypted_RSA(this string input, string publicKeyString, RSAEncryptionPadding encryptionPadding)
            => Encoding.UTF8.GetString(Encoding.UTF8.GetBytes(input).Encrypted_RSA(publicKeyString, encryptionPadding));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="privateKeyString">私钥</param>
        /// <param name="encryptionPadding"></param>
        /// <returns></returns>
        public static string Decrypted_RSA(this string input, string privateKeyString, RSAEncryptionPadding encryptionPadding)
            => Encoding.UTF8.GetString(Encoding.UTF8.GetBytes(input).Decrypted_RSA(privateKeyString, encryptionPadding));
#endif

        /// <summary>
        /// 为 null 或空字符
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this string input)
            => string.IsNullOrEmpty(input);

        /// <summary>
        /// 为 null 或空字符，或空白字符
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsNullOrWhiteSpace(this string input)
            => string.IsNullOrWhiteSpace(input);

        /// <summary>
        /// 转换为骆驼命名法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToCamelCase(this string input)
        {
            if (input == null)
                return null;

            if (input.Length < 2)
                return input.ToLower();

            var i = 0;
            var find = false;

            for (; i < input.Length; i++)
            {
                var value = (int)input[i];
                if (value < 65 || value > 90)
                {
                    find = true;
                    break;
                }
            }

            if (!find)
                i++;

            if (i < 1)
                return input;
            else if (i == 1)
                return $"{input.Substring(0, 1).ToLower()}{input.Substring(1)}";
            else
                return $"{input.Substring(0, i - 1).ToLower()}{input.Substring(i - 1)}";
        }

        /// <summary>
        /// 获取HTML文档(部分文档)中的文本内容
        /// </summary>
        /// <param name="input">HTML文档(部分文档)</param>
        /// <returns></returns>
        public static string ClearHTML(this string input)
        {
            var output = input ?? string.Empty;
            if (string.IsNullOrEmpty(output))
                return string.Empty;
            output = Regex.Replace(output, @"(<script[^>]*>[.\n]*?</script>)+?", string.Empty, RegexOptions.IgnoreCase);
            output = Regex.Replace(output, @"(<[^>]*>)+?", string.Empty, RegexOptions.IgnoreCase);
            output = Regex.Replace(output, @"([\r\n])[\s]+", string.Empty, RegexOptions.IgnoreCase);
            output = Regex.Replace(output, @"-->", string.Empty, RegexOptions.IgnoreCase);
            output = Regex.Replace(output, @"<!--.*", string.Empty, RegexOptions.IgnoreCase);
            output = Regex.Replace(output, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
            output = Regex.Replace(output, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
            output = Regex.Replace(output, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
            output = Regex.Replace(output, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
            output = Regex.Replace(output, @"&(nbsp|#160);", " ", RegexOptions.IgnoreCase);
            output = Regex.Replace(output, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
            output = Regex.Replace(output, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
            output = Regex.Replace(output, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
            output = Regex.Replace(output, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);
            output = Regex.Replace(output, @"&#(\d+);", string.Empty, RegexOptions.IgnoreCase);
            output.Replace("<", string.Empty);
            output.Replace(">", string.Empty);
            output.Replace(Environment.NewLine, string.Empty);
            return output;
        }

        /// <summary>
        /// 截取当前字符串的指定长度，并附加一个指定的结尾
        /// 如果指定长度大于当前字符串长度，则返回当前字符串本身
        /// 如果该字符串为null，则返回0长度字符串
        /// </summary>
        /// <param name="input"></param>
        /// <param name="startIndex">截取开始位置</param>
        /// <param name="length">截取后的字符串长度</param>
        /// <param name="endWith">为结果字符串附加一个结尾</param>
        /// <returns></returns>
        public static string Substring(this string input, int startIndex, int length, string endWith)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            if (input.Length <= length)
                return input;

            return input.Substring(startIndex, length) + endWith ?? string.Empty;
        }

        /// <summary>
        /// 获取指定长度的随机字符串
        /// </summary>
        /// <param name="input"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string Random(this string input, int? length = null)
        {
            var random = new Random();
            var chars = input
                .ToCharArray()
                .OrderBy(_ => random.Next())
                .Take(length ?? random.Next(input.Length + 1));
            return string.Join(string.Empty, chars);
        }

        /// <summary>
        /// 指示所指定的正则表达式在指定的输入字符串中是否找到了匹配项。
        /// </summary>
        /// <param name="input"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public static bool IsMatch(this string input, string pattern)
            => Regex.IsMatch(input, pattern);

        /// <summary>
        /// 指示所指定的正则表达式在指定的输入字符串中是否找到了匹配项。
        /// </summary>
        /// <param name="input"></param>
        /// <param name="pattern"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static bool IsMatch(this string input, string pattern, RegexOptions options)
            => Regex.IsMatch(input, pattern, options);

        /// <summary>
        /// 指示所指定的正则表达式在指定的输入字符串中是否找到了匹配项。
        /// </summary>
        /// <param name="input"></param>
        /// <param name="pattern"></param>
        /// <param name="options"></param>
        /// <param name="matchTimeout"></param>
        /// <returns></returns>
        public static bool IsMatch(this string input, string pattern, RegexOptions options, TimeSpan matchTimeout)
            => Regex.IsMatch(input, pattern, options, matchTimeout);

        /// <summary>
        /// 在指定的输入字符串中搜索指定的正则表达式的第一个匹配项。
        /// </summary>
        /// <param name="input"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public static Match Match(this string input, string pattern)
            => Regex.Match(input, pattern);

        /// <summary>
        /// 使用指定的匹配选项在输入字符串中搜索指定的正则表达式的第一个匹配项。
        /// </summary>
        /// <param name="input"></param>
        /// <param name="pattern"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static Match Match(this string input, string pattern, RegexOptions options)
            => Regex.Match(input, pattern, options);

        /// <summary>
        /// 使用指定的匹配选项和超时间隔在输入字符串中搜索指定的正则表达式的第一个匹配项。
        /// </summary>
        /// <param name="input"></param>
        /// <param name="pattern"></param>
        /// <param name="options"></param>
        /// <param name="matchTimeout"></param>
        /// <returns></returns>
        public static Match Match(this string input, string pattern, RegexOptions options, TimeSpan matchTimeout)
            => Regex.Match(input, pattern, options, matchTimeout);

        /// <summary>
        /// 在指定的输入字符串中搜索指定的正则表达式的所有匹配项。
        /// </summary>
        /// <param name="input"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public static MatchCollection Matches(this string input, string pattern)
            => Regex.Matches(input, pattern);

        /// <summary>
        /// 使用指定的匹配选项在指定的输入字符串中搜索指定的正则表达式的所有匹配项。
        /// </summary>
        /// <param name="input"></param>
        /// <param name="pattern"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static MatchCollection Matches(this string input, string pattern, RegexOptions options)
            => Regex.Matches(input, pattern, options);

        /// <summary>
        /// 使用指定的匹配选项和超时间隔在指定的输入字符串中搜索指定的正则表达式的所有匹配项。
        /// </summary>
        /// <param name="input"></param>
        /// <param name="pattern"></param>
        /// <param name="options"></param>
        /// <param name="matchTimeout"></param>
        /// <returns></returns>
        public static MatchCollection Matches(this string input, string pattern, RegexOptions options, TimeSpan matchTimeout)
            => Regex.Matches(input, pattern, options, matchTimeout);

        /// <summary>
        /// 在指定的输入字符串内，使用指定的替换字符串替换与指定正则表达式匹配的所有字符串。
        /// </summary>
        /// <param name="input"></param>
        /// <param name="pattern"></param>
        /// <param name="replacement"></param>
        /// <returns></returns>
        public static string Replace(this string input, string pattern, string replacement)
            => Regex.Replace(input, pattern, replacement);

        /// <summary>
        /// 在指定的输入字符串内，使用指定的替换字符串替换与指定正则表达式匹配的所有字符串。指定的选项将修改匹配操作。
        /// </summary>
        /// <param name="input"></param>
        /// <param name="pattern"></param>
        /// <param name="replacement"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static string Replace(this string input, string pattern, string replacement, RegexOptions options)
            => Regex.Replace(input, pattern, replacement, options);

        /// <summary>
        /// 在指定的输入字符串内，使用指定的替换字符串替换与指定正则表达式匹配的所有字符串。如果未找到匹配项，则其他参数指定修改匹配操作的选项和超时间隔。
        /// </summary>
        /// <param name="input"></param>
        /// <param name="pattern"></param>
        /// <param name="replacement"></param>
        /// <param name="options"></param>
        /// <param name="matchTimeout"></param>
        /// <returns></returns>
        public static string Replace(this string input, string pattern, string replacement, RegexOptions options, TimeSpan matchTimeout)
            => Regex.Replace(input, pattern, replacement, options, matchTimeout);

        /// <summary>
        /// 在指定的输入字符串内，使用 System.Text.RegularExpressions.MatchEvaluator 委托返回的字符串替换与指定正则表达式匹配的所有字符串。
        /// </summary>
        /// <param name="input"></param>
        /// <param name="pattern"></param>
        /// <param name="evaluator"></param>
        /// <returns></returns>
        public static string Replace(this string input, string pattern, MatchEvaluator evaluator)
            => Regex.Replace(input, pattern, evaluator);

        /// <summary>
        /// 在指定的输入字符串内，使用 System.Text.RegularExpressions.MatchEvaluator 委托返回的字符串替换与指定正则表达式匹配的所有字符串。指定的选项将修改匹配操作。
        /// </summary>
        /// <param name="input"></param>
        /// <param name="pattern"></param>
        /// <param name="evaluator"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static string Replace(this string input, string pattern, MatchEvaluator evaluator, RegexOptions options)
            => Regex.Replace(input, pattern, evaluator, options);

        /// <summary>
        /// 在指定的输入字符串内，使用 System.Text.RegularExpressions.MatchEvaluator 委托返回的字符串替换与指定正则表达式匹配的所有字符串。如果未找到匹配项，则其他参数指定修改匹配操作的选项和超时间隔。
        /// </summary>
        /// <param name="input"></param>
        /// <param name="pattern"></param>
        /// <param name="evaluator"></param>
        /// <param name="options"></param>
        /// <param name="matchTimeout"></param>
        /// <returns></returns>
        public static string Replace(this string input, string pattern, MatchEvaluator evaluator, RegexOptions options, TimeSpan matchTimeout)
            => Regex.Replace(input, pattern, evaluator, options, matchTimeout);

        /// <summary>
        /// 在由正则表达式模式定义的位置将输入字符串拆分为一个子字符串数组。
        /// </summary>
        /// <param name="input"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public static string[] Split(this string input, string pattern)
            => Regex.Split(input, pattern);

        /// <summary>
        /// 在由指定正则表达式模式定义的位置将输入字符串拆分为一个子字符串数组。指定的选项将修改匹配操作。
        /// </summary>
        /// <param name="input"></param>
        /// <param name="pattern"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static string[] Split(this string input, string pattern, RegexOptions options)
            => Regex.Split(input, pattern, options);

        /// <summary>
        /// 在由指定正则表达式模式定义的位置将输入字符串拆分为一个子字符串数组。如果未找到匹配项，则其他参数指定修改匹配操作的选项和超时间隔。
        /// </summary>
        /// <param name="input"></param>
        /// <param name="pattern"></param>
        /// <param name="options"></param>
        /// <param name="matchTimeout"></param>
        /// <returns></returns>
        public static string[] Split(this string input, string pattern, RegexOptions options, TimeSpan matchTimeout)
            => Regex.Split(input, pattern, options, matchTimeout);

        /// <summary>
        /// 通过替换为转义码来转义最小的字符集（\、*、+、?、|、{、[、(、)、^、$、.、# 和空白）。这将指示正则表达式引擎按原义解释这些字符而不是解释为元字符。
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string Escaped(this string input)
            => Regex.Escape(input);

        /// <summary>
        /// 转换输入字符串中的任何转义字符。
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string Unescaped(this string input)
            => Regex.Unescape(input);
    }
}
