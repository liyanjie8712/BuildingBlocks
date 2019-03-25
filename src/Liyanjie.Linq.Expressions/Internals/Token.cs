namespace Liyanjie.Linq.Expressions.Internals
{
    /// <summary>
    /// 
    /// </summary>
    internal class Token
    {
        /// <summary>
        /// 
        /// </summary>
        public TokenId Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Length { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public dynamic Value { get; set; }
    }
}