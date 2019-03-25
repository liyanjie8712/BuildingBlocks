namespace System
{
    /// <summary>
    /// 
    /// </summary>
    public static class DateTimeOffsetExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static DateTimeOffset Date(this DateTimeOffset input) => input.Date;

        /// <summary>
        /// 获取当前时间是当前年中的第几周
        /// </summary>
        /// <param name="input">当前时间</param>
        /// <returns></returns>
        public static int WeekOfYear(this DateTimeOffset input)
        {
            var firstDayOfYear = new DateTimeOffset(input.Year, 1, 1, 0, 0, 0, input.Offset);
            var skipWeek = firstDayOfYear.DayOfWeek > 0 ? 1 : 0;
            var days = input.DayOfYear - (firstDayOfYear.DayOfWeek > 0 ? 7 - (int)firstDayOfYear.DayOfWeek : 0);
            var weekOfYear = days / 7 + 1;
            if (days % 7 > 0)
                weekOfYear += 1;
            return weekOfYear;
        }
    }
}
