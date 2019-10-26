using System.Collections.Generic;
using System.Linq.Expressions;

using Liyanjie.Linq.Expressions;
using Liyanjie.Linq.Internals;

namespace System.Linq
{
    /// <summary>
    /// 
    /// </summary>
    public static class DynamicQueryable
    {
        #region All

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="predicate"></param>
        /// <param name="variables"></param>
        /// <returns></returns>
        public static bool All(this IQueryable source, string predicate, IDictionary<string, dynamic> variables = null)
        {
            Check.NotNull(source, nameof(source));
            Check.NotEmpty(predicate, nameof(predicate));

            var lambda = ExpressionParser.ParseLambda(source.ElementType, predicate, variables);

            return source.Execute<bool>(Methods.AllWithPredicate, lambda);
        }

        #endregion

        #region Any

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool Any(this IQueryable source)
        {
            Check.NotNull(source, nameof(source));

            return source.Execute<bool>(Methods.Any);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="predicate"></param>
        /// <param name="variables"></param>
        /// <returns></returns>
        public static bool Any(this IQueryable source, string predicate, IDictionary<string, dynamic> variables = null)
        {
            Check.NotNull(source, nameof(source));
            Check.NotEmpty(predicate, nameof(predicate));

            var lambda = ExpressionParser.ParseLambda(source.ElementType, predicate, variables);

            return source.Execute<bool>(Methods.AnyWithPredicate, lambda);
        }

        #endregion

        #region AsEnumerable

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static IEnumerable<object> AsEnumerable(this IQueryable source)
        {
            foreach (var item in source)
            {
                yield return item;
            }
        }

        #endregion

        #region Average

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static object Average(this IQueryable source)
        {
            Check.NotNull(source, nameof(source));

            return source.ExecuteAverage();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="selector"></param>
        /// <param name="variables"></param>
        /// <returns></returns>
        public static object Average(this IQueryable source, string selector, IDictionary<string, dynamic> variables = null)
        {
            Check.NotNull(source, nameof(source));
            Check.NotEmpty(selector, nameof(selector));

            var lambda = ExpressionParser.ParseLambda(source.ElementType, selector, variables);

            return source.ExecuteAverage(lambda);
        }

        #endregion

        #region Count

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static int Count(this IQueryable source)
        {
            Check.NotNull(source, nameof(source));

            return source.Execute<int>(Methods.Count);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="predicate"></param>
        /// <param name="variables"></param>
        /// <returns></returns>
        public static int Count(this IQueryable source, string predicate, IDictionary<string, dynamic> variables = null)
        {
            Check.NotNull(source, nameof(source));
            Check.NotEmpty(predicate, nameof(predicate));

            //var createParameterCtor = source.IsLinqToObjects();
            var lambda = ExpressionParser.ParseLambda(source.ElementType, predicate, variables);

            return source.Execute<int>(Methods.CountWithPredicate, lambda);
        }

        #endregion

        #region Distinct

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static IQueryable Distinct(this IQueryable source)
        {
            Check.NotNull(source, nameof(source));

            return source.CreateQuery(Methods.Distinct);
        }

        #endregion

        #region ElementAt

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static object ElementAt(this IQueryable source, int index)
        {
            Check.NotNull(source, nameof(source));
            Check.Condition(index, _ => _ >= 0, nameof(index));

            return source.Execute(Methods.ElementAtWithIndex, Expression.Constant(index));
        }

        #endregion

        #region ElementAtOrDefault

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static object ElementAtOrDefault(this IQueryable source, int index)
        {
            Check.NotNull(source, nameof(source));
            Check.Condition(index, _ => _ >= 0, nameof(index));

            return source.Execute(Methods.ElementAtOrDefaultWithIndex, Expression.Constant(index));
        }

        #endregion

        #region First

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static object First(this IQueryable source)
        {
            Check.NotNull(source, nameof(source));

            return source.Execute(Methods.First);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="predicate"></param>
        /// <param name="variables"></param>
        /// <returns></returns>
        public static object First(this IQueryable source, string predicate, IDictionary<string, dynamic> variables = null)
        {
            Check.NotNull(source, nameof(source));
            Check.NotEmpty(predicate, nameof(predicate));

            var lambda = ExpressionParser.ParseLambda(source.ElementType, predicate, variables);

            return source.Execute(Methods.FirstWithPredicate, lambda);
        }

        #endregion

        #region FirstOrDefault

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static object FirstOrDefault(this IQueryable source)
        {
            Check.NotNull(source, nameof(source));

            return source.Execute(Methods.FirstOrDefault);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="predicate"></param>
        /// <param name="variables"></param>
        /// <returns></returns>
        public static object FirstOrDefault(this IQueryable source, string predicate, IDictionary<string, dynamic> variables = null)
        {
            Check.NotNull(source, nameof(source));
            Check.NotEmpty(predicate, nameof(predicate));

            var lambda = ExpressionParser.ParseLambda(source.ElementType, predicate, variables);

            return source.Execute(Methods.FirstOrDefaultWithPredicate, lambda);
        }

        #endregion

        #region GroupBy

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="keySelector"></param>
        /// <param name="variables"></param>
        /// <returns></returns>
        public static IQueryable GroupBy(this IQueryable source, string keySelector, IDictionary<string, dynamic> variables = null)
        {
            Check.NotNull(source, nameof(source));
            Check.NotEmpty(keySelector, nameof(keySelector));

            var keyLambda = ExpressionParser.ParseLambda(source.ElementType, keySelector, variables);

            return source.CreateQuery(Methods.GroupByWithKeySelector, keyLambda);
        }

        #endregion

        #region Last

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static object Last(this IQueryable source)
        {
            Check.NotNull(source, nameof(source));

            return source.Execute(Methods.Last);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="predicate"></param>
        /// <param name="variables"></param>
        /// <returns></returns>
        public static object Last(this IQueryable source, string predicate, IDictionary<string, dynamic> variables = null)
        {
            Check.NotNull(source, nameof(source));
            Check.NotEmpty(predicate, nameof(predicate));

            var lambda = ExpressionParser.ParseLambda(source.ElementType, predicate, variables);

            return source.Execute(Methods.LastWithPredicate, lambda);
        }

        #endregion

        #region LastOrDefault

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static object LastOrDefault(this IQueryable source)
        {
            Check.NotNull(source, nameof(source));

            return source.Execute(Methods.LastOrDefault);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="predicate"></param>
        /// <param name="variables"></param>
        /// <returns></returns>
        public static object LastOrDefault(this IQueryable source, string predicate, IDictionary<string, dynamic> variables = null)
        {
            Check.NotNull(source, nameof(source));
            Check.NotEmpty(predicate, nameof(predicate));

            var lambda = ExpressionParser.ParseLambda(source.ElementType, predicate, variables);

            return source.Execute(Methods.LastOrDefaultWithPredicate, lambda);
        }

        #endregion LastOrDefault

        #region Max

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static object Max(this IQueryable source)
        {
            Check.NotNull(source, nameof(source));

            return source.Execute(Methods.Max, source.ElementType);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="selector"></param>
        /// <param name="variables"></param>
        /// <returns></returns>
        public static object Max(this IQueryable source, string selector, IDictionary<string, dynamic> variables = null)
        {
            Check.NotNull(source, nameof(source));
            Check.NotEmpty(selector, nameof(selector));

            var lambda = ExpressionParser.ParseLambda(source.ElementType, selector, variables);

            return source.Execute(Methods.MaxWithSelector, lambda.ReturnType, lambda);
        }

        #endregion

        #region Min

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static object Min(this IQueryable source)
        {
            Check.NotNull(source, nameof(source));

            return source.Execute(Methods.Min, source.ElementType);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="selector"></param>
        /// <param name="variables"></param>
        /// <returns></returns>
        public static object Min(this IQueryable source, string selector, IDictionary<string, dynamic> variables = null)
        {
            Check.NotNull(source, nameof(source));
            Check.NotEmpty(selector, nameof(selector));

            var lambda = ExpressionParser.ParseLambda(source.ElementType, selector, variables);

            return source.Execute(Methods.MinWithSelector, lambda.ReturnType, lambda);
        }

        #endregion

        #region OrderBy

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="selector"></param>
        /// <param name="variables"></param>
        /// <returns></returns>
        public static IOrderedQueryable OrderBy(this IQueryable source, string selector, IDictionary<string, dynamic> variables = null)
        {
            Check.NotNull(source, nameof(source));
            Check.NotEmpty(selector, nameof(selector));

            var lambda = ExpressionParser.ParseLambda(source.ElementType, selector, variables);

            return (IOrderedQueryable)source.CreateQuery(Methods.OrderByWithSelector, lambda);
        }

        #endregion

        #region OrderByDescending

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="selector"></param>
        /// <param name="variables"></param>
        /// <returns></returns>
        public static IOrderedQueryable OrderByDescending(this IQueryable source, string selector, IDictionary<string, dynamic> variables = null)
        {
            Check.NotNull(source, nameof(source));
            Check.NotEmpty(selector, nameof(selector));

            var lambda = ExpressionParser.ParseLambda(source.ElementType, selector, variables);

            return (IOrderedQueryable)source.CreateQuery(Methods.OrderByDescendingWithSelector, lambda);
        }

        #endregion

        #region Reverse

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static IQueryable Reverse(this IQueryable source)
        {
            Check.NotNull(source, nameof(source));

            return source.CreateQuery(Methods.Reverse);
        }

        #endregion

        #region Select

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="selector"></param>
        /// <param name="variables"></param>
        /// <returns></returns>
        public static IQueryable Select(this IQueryable source, string selector, IDictionary<string, dynamic> variables = null)
        {
            Check.NotNull(source, nameof(source));
            Check.NotEmpty(selector, nameof(selector));

            var lambda = ExpressionParser.ParseLambda(source.ElementType, selector, variables);

            return source.CreateQuery(Methods.SelectWithSelector, lambda);
        }

        #endregion

        #region Single

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static object Single(this IQueryable source)
        {
            Check.NotNull(source, nameof(source));

            return source.Execute(Methods.Single);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="predicate"></param>
        /// <param name="variables"></param>
        /// <returns></returns>
        public static object Single(this IQueryable source, string predicate, IDictionary<string, dynamic> variables = null)
        {
            Check.NotNull(source, nameof(source));
            Check.NotEmpty(predicate, nameof(predicate));

            var lambda = ExpressionParser.ParseLambda(source.ElementType, predicate, variables);

            return source.Execute(Methods.SingleWithPredicate, lambda);
        }

        #endregion

        #region SingleOrDefault

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static object SingleOrDefault(this IQueryable source)
        {
            Check.NotNull(source, nameof(source));

            return source.Execute(Methods.SingleOrDefault);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="predicate"></param>
        /// <param name="variables"></param>
        /// <returns></returns>
        public static object SingleOrDefault(this IQueryable source, string predicate, IDictionary<string, dynamic> variables = null)
        {
            Check.NotNull(source, nameof(source));
            Check.NotEmpty(predicate, nameof(predicate));

            var lambda = ExpressionParser.ParseLambda(source.ElementType, predicate, variables);

            return source.Execute(Methods.SingleOrDefaultWithPredicate, lambda);
        }

        #endregion

        #region Skip

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static IQueryable Skip(this IQueryable source, int count)
        {
            Check.NotNull(source, nameof(source));
            Check.Condition(count, x => x >= 0, nameof(count));

            if (count == 0)
                return source;

            return source.CreateQuery(Methods.SkipWithCount, Expression.Constant(count));
        }

        #endregion

        #region SkipWhile

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="predicate"></param>
        /// <param name="variables"></param>
        /// <returns></returns>
        public static IQueryable SkipWhile(this IQueryable source, string predicate, IDictionary<string, dynamic> variables = null)
        {
            Check.NotNull(source, nameof(source));
            Check.NotNull(predicate, nameof(predicate));

            var lambda = ExpressionParser.ParseLambda(source.ElementType, predicate, variables);

            return source.CreateQuery(Methods.SkipWhileWithPredicate, lambda);
        }

        #endregion

        #region Sum

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static object Sum(this IQueryable source)
        {
            Check.NotNull(source, nameof(source));

            return source.ExecuteSum();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="selector"></param>
        /// <param name="variables"></param>
        /// <returns></returns>
        public static object Sum(this IQueryable source, string selector, IDictionary<string, dynamic> variables = null)
        {
            Check.NotNull(source, nameof(source));
            Check.NotEmpty(selector, nameof(selector));

            var lambda = ExpressionParser.ParseLambda(source.ElementType, selector, variables);

            return source.ExecuteSum(lambda);
        }

        #endregion

        #region Take

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static IQueryable Take(this IQueryable source, int count)
        {
            Check.NotNull(source, nameof(source));
            Check.Condition(count, _ => _ >= 0, nameof(count));

            return source.CreateQuery(Methods.TakeWithCount, Expression.Constant(count));
        }

        #endregion

        #region TakeWhile

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="predicate"></param>
        /// <param name="variables"></param>
        /// <returns></returns>
        public static IQueryable TakeWhile(this IQueryable source, string predicate, IDictionary<string, dynamic> variables = null)
        {
            Check.NotNull(source, nameof(source));
            Check.NotNull(predicate, nameof(predicate));

            var expression = ExpressionParser.ParseLambda(source.ElementType, predicate, variables);

            return source.CreateQuery(Methods.TakeWhileWithPredicate, expression);
        }

        #endregion

        #region ThenBy

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="selector"></param>
        /// <param name="variables"></param>
        /// <returns></returns>
        public static IOrderedQueryable ThenBy(this IOrderedQueryable source, string selector, IDictionary<string, dynamic> variables = null)
        {
            Check.NotNull(source, nameof(source));
            Check.NotEmpty(selector, nameof(selector));

            var lambda = ExpressionParser.ParseLambda(source.ElementType, selector, variables);

            return (IOrderedQueryable)source.CreateQuery(Methods.ThenByWithSelector, lambda);
        }

        #endregion

        #region ThenByDescending

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="selector"></param>
        /// <param name="variables"></param>
        /// <returns></returns>
        public static IOrderedQueryable ThenByDescending(this IOrderedQueryable source, string selector, IDictionary<string, dynamic> variables = null)
        {
            Check.NotNull(source, nameof(source));
            Check.NotEmpty(selector, nameof(selector));

            var lambda = ExpressionParser.ParseLambda(source.ElementType, selector, variables);

            return (IOrderedQueryable)source.CreateQuery(Methods.ThenByDescendingWithSelector, lambda);
        }

        #endregion

        #region Where

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="predicate"></param>
        /// <param name="variables"></param>
        /// <returns></returns>
        public static IQueryable Where(this IQueryable source, string predicate, IDictionary<string, dynamic> variables = null)
        {
            Check.NotNull(source, nameof(source));
            Check.NotNull(predicate, nameof(predicate));

            var expression = ExpressionParser.ParseLambda(source.ElementType, predicate, variables);

            return source.CreateQuery(Methods.WhereWithPredicate, expression);
        }

        #endregion
    }
}
