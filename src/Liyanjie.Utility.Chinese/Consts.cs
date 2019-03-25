namespace Liyanjie.Utility.Chinese
{
    /// <summary>
    /// 
    /// </summary>
    public static class Consts
    {
        /// <summary>
        /// 正则表达式：Cellnumber
        /// </summary>
        public const string RegexPattern_PhoneNumber = @"1[345678]\d{9}";

        /// <summary>
        /// 正则表达式：IdNumber
        /// </summary>
        public const string RegexPattern_IdNumber = @"[1-9]\d{5}[1-9]\d{3}(((0[13578]|1[02])(0[1-9]|[1-2]\d|3[0-1]))|(0[246]|11)(0[1-9]|[1-2]\d|30))\d{3}[0-9X]";
    }
}
