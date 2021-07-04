using Liyanjie.Pinyin;

namespace System
{
    /// <summary>
    /// 
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// 获取拼音
        /// </summary>
        /// <param name="chineseChar"></param>
        /// <returns></returns>
        public static string[] GetChineseCharPinyins(this char chineseChar)
        {
            return PinyinHelper.GetChineseCharPinyins(chineseChar);
        }

        /// <summary>
        /// 获取拼音
        /// </summary>
        /// <param name="chineseWord"></param>
        /// <returns></returns>
        public static string[] GetChineseWordPinyin(this string chineseWord)
        {
            return PinyinHelper.GetChineseWordPinyin(chineseWord);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="chineseInput"></param>
        /// <returns></returns>
        public static string[] GetPinyin(this string chineseInput)
        {
            return PinyinHelper.GetPinyin(chineseInput);
        }
    }
}
