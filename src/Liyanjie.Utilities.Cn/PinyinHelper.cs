using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace Liyanjie.Utilities.Cn
{
    /// <summary>
    /// 
    /// </summary>
    public class PinyinHelper
    {
        static readonly Dictionary<char, (string, int, string[])> chineseChars;
        static PinyinHelper()
        {
            chineseChars = new();
            using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Liyanjie.Utilities.Cn.ChineseChars.txt");
            using var reader = new StreamReader(stream, Encoding.UTF8);
            while (true)
            {
                var @string = reader.ReadLine();
                if (string.IsNullOrEmpty(@string))
                    break;
                if (@string.StartsWith("#"))
                    continue;

                var array = @string.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                chineseChars.Add(array[1][0], (array[0], int.Parse(array[2]), array[3].Split(',')));
            }
        }

        public static IReadOnlyDictionary<char, (string Code, int StrokeCount, string[] Pinyins)> ChineseChars => chineseChars;

        public static bool TryGetPinyin(char chineseChar, out string[] pinyins)
        {
            if (ChineseChars.ContainsKey(chineseChar))
            {
                pinyins = ChineseChars[chineseChar].Pinyins;
                return true;
            }
            else
            {
                pinyins = default;
                return false;
            }
        }
    }
}
