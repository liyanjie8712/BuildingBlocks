using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using JiebaNet.Segmenter;

namespace Liyanjie.Pinyin
{
    /// <summary>
    /// 
    /// </summary>
    public class PinyinHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="chineseChar"></param>
        /// <returns></returns>
        public static string[] GetChineseCharPinyins(char chineseChar)
        {
            if ((chineseChar > 47 && chineseChar < 58) || (chineseChar > 64 && chineseChar < 91) || (chineseChar > 96 && chineseChar < 123))
                return new[] { chineseChar.ToString() };

            return ChineseCharManager.ChineseChars.ContainsKey(chineseChar)
                ? ChineseCharManager.ChineseChars[chineseChar].Pinyins
                : new[] { "*" };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="chineseWord"></param>
        /// <returns></returns>
        public static string[] GetChineseWordPinyin(string chineseWord)
        {
            if (string.IsNullOrWhiteSpace(chineseWord))
                return default;

            return ChineseWordManager.ChineseWords.ContainsKey(chineseWord)
                ? ChineseWordManager.ChineseWords[chineseWord]
                : chineseWord.Select(_ => GetChineseCharPinyins(_)[0]).ToArray();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="chineseInput"></param>
        /// <returns></returns>
        public static string[] GetPinyin(string chineseInput)
        {
            return new JiebaSegmenter().Cut(chineseInput)
                .SelectMany(_ => GetChineseWordPinyin(_))
                .ToArray();
        }

        /// <summary>
        /// 
        /// </summary>
        public class ChineseCharManager
        {
            /// <summary>
            /// 
            /// </summary>
            public static string ChineseCharsFile { get; set; } = @"Pinyin\pinyin.txt";
            static readonly Lazy<Dictionary<char, (string Code, string[] Pinyins)>> chineseChars_Lazy = new(() =>
            {
                var chineseChars = new Dictionary<char, (string, string[])>();
                var data = File.ReadAllLines(GetAbsoluteFile(ChineseCharsFile), Encoding.UTF8)
                    .Where(_ => !string.IsNullOrWhiteSpace(_))
                    .Where(_ => !_.StartsWith("#"))
                    .Select(_ => _.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));
                foreach (var array in data)
                {
                    var @char = array[3][0];
                    if (chineseChars.ContainsKey(@char))
                        continue;

                    chineseChars.Add(@char, (array[0].TrimEnd(':'), array[1].Split(',')));
                }
                return chineseChars;
            });

            /// <summary>
            /// 
            /// </summary>
            public static IReadOnlyDictionary<char, (string Code, string[] Pinyins)> ChineseChars => chineseChars_Lazy.Value;

            /// <summary>
            /// 
            /// </summary>
            /// <param name="chineseChar"></param>
            /// <param name="code"></param>
            /// <param name="pinyins"></param>
            public static void AddChineseChar(char chineseChar, string code, string[] pinyins)
            {
                if (chineseChars_Lazy.Value.ContainsKey(chineseChar))
                    chineseChars_Lazy.Value[chineseChar] = (chineseChars_Lazy.Value[chineseChar].Code, chineseChars_Lazy.Value[chineseChar].Pinyins.Concat(pinyins).ToArray());
                else
                    chineseChars_Lazy.Value[chineseChar] = (code, pinyins);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public class ChineseWordManager
        {
            /// <summary>
            /// 
            /// </summary>
            public static string ChineseWordsFile { get; set; } = @"Pinyin\large_pinyin.txt";
            static readonly Lazy<Dictionary<string, string[]>> chineseWords_Lazy = new(() =>
            {
                var chineseWords = new Dictionary<string, string[]>();
                var data = File.ReadAllLines(GetAbsoluteFile(ChineseWordsFile), Encoding.UTF8)
                    .Where(_ => !string.IsNullOrWhiteSpace(_))
                    .Where(_ => !_.StartsWith("#"))
                    .Select(_ => _.Split(new[] { ':' }, StringSplitOptions.RemoveEmptyEntries));
                foreach (var array in data)
                {
                    var word = array[0];
                    if (chineseWords.ContainsKey(word))
                        continue;
                    chineseWords.Add(word, array[1].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));
                }
                return chineseWords;
            });

            /// <summary>
            /// 
            /// </summary>
            public static IReadOnlyDictionary<string, string[]> ChineseWords => chineseWords_Lazy.Value;

            /// <summary>
            /// 
            /// </summary>
            /// <param name="chineseWord"></param>
            /// <param name="pinyins"></param>
            public static void AddChineseWord(string chineseWord, params string[] pinyins)
            {
                if (chineseWords_Lazy.Value.ContainsKey(chineseWord))
                    chineseWords_Lazy.Value[chineseWord] = chineseWords_Lazy.Value[chineseWord].Concat(pinyins).ToArray();
                else
                    chineseWords_Lazy.Value[chineseWord] = pinyins;
            }
        }

        static string GetAbsoluteFile(string file)
        {
            return Path.IsPathRooted(file) ? file : Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, file));
        }
    }
}
