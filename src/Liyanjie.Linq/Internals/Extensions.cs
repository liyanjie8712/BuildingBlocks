using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Liyanjie.Linq.Internals
{
    internal static class Extensions
    {

        #region CreateQuery

        public static IQueryable CreateQuery(this IQueryable source, MethodInfo method)
        {
            if (method.IsGenericMethod)
                method = method.MakeGenericMethod(source.ElementType);

            var expression = Expression.Call(method, source.Expression);
            return source.Provider.CreateQuery(expression);
        }

        public static IQueryable CreateQuery(this IQueryable source, MethodInfo method, ConstantExpression constant)
        {
            if (method.IsGenericMethod)
                method = method.MakeGenericMethod(source.ElementType);

            var expression = Expression.Call(method, source.Expression, constant);
            return source.Provider.CreateQuery(expression);
        }

        public static IQueryable CreateQuery(this IQueryable source, MethodInfo method, LambdaExpression lambda)
        {
            method = method.GetGenericArguments().Length == 2
                ? method.MakeGenericMethod(source.ElementType, lambda.Body.Type)
                : method.MakeGenericMethod(source.ElementType);

            var expression = Expression.Call(method, source.Expression, lambda);
            return source.Provider.CreateQuery(expression);
        }

        #endregion

        #region Execute

        /// <summary>
        /// 用于：First、FirstOrDefault、Last、LastOrDefault、Single、SingleOrDefault
        /// </summary>
        /// <param name="source"></param>
        /// <param name="method"></param>
        /// <param name="lambda"></param>
        /// <returns></returns>
        public static object Execute(this IQueryable source, MethodInfo method, LambdaExpression lambda = null)
        {
            if (method.IsGenericMethod)
                method = method.MakeGenericMethod(source.ElementType);

            var expression = lambda == null
                ? Expression.Call(method, source.Expression)
                : Expression.Call(method, source.Expression, lambda);
            return source.Execute(source.ElementType).Invoke(source.Provider, new[] { expression });
        }

        /// <summary>
        /// 用于：ElementAt、ElementAtOrDefault
        /// </summary>
        /// <param name="source"></param>
        /// <param name="method"></param>
        /// <param name="constant"></param>
        /// <returns></returns>
        public static object Execute(this IQueryable source, MethodInfo method, ConstantExpression constant)
        {
            if (method.IsGenericMethod)
                method = method.MakeGenericMethod(source.ElementType);

            var expression = Expression.Call(method, source.Expression, constant);
            return source.Execute(source.ElementType).Invoke(source.Provider, new[] { expression });
        }

        /// <summary>
        /// 用于：Max、Min
        /// </summary>
        /// <param name="source"></param>
        /// <param name="method"></param>
        /// <param name="returnType"></param>
        /// <param name="lambda"></param>
        /// <returns></returns>
        public static object Execute(this IQueryable source, MethodInfo method, Type returnType, LambdaExpression lambda = null)
        {
            method = method.GetGenericArguments().Length == 2
                ? method.MakeGenericMethod(source.ElementType, returnType)
                : method.MakeGenericMethod(source.ElementType);

            var expression = lambda == null
                ? Expression.Call(method, source.Expression)
                : Expression.Call(method, source.Expression, lambda);
            return source.Execute(returnType).Invoke(source.Provider, new[] { expression });
        }

        /// <summary>
        /// 用于：Sum
        /// </summary>
        /// <param name="source"></param>
        /// <param name="lambda"></param>
        /// <returns></returns>
        public static object ExecuteSum(this IQueryable source, LambdaExpression lambda = null)
        {
            var method = nameof(Queryable.Sum);

            var expression = lambda == null
                ? Expression.Call(typeof(Queryable), method, null, source.Expression)
                : Expression.Call(typeof(Queryable), method, new[] { source.ElementType }, source.Expression, lambda);

            return lambda == null
                ? source.Execute(source.ElementType).Invoke(source.Provider, new[] { expression })
                : source.Execute(lambda.ReturnType).Invoke(source.Provider, new[] { expression });
        }

        /// <summary>
        /// 用于：Average
        /// </summary>
        /// <param name="source"></param>
        /// <param name="lambda"></param>
        /// <returns></returns>
        public static object ExecuteAverage(this IQueryable source, LambdaExpression lambda = null)
        {
            var method = nameof(Queryable.Average);

            var expression = lambda == null
                ? Expression.Call(typeof(Queryable), method, null, source.Expression)
                : Expression.Call(typeof(Queryable), method, new[] { source.ElementType }, source.Expression, lambda);
            var elementType = lambda == null
                ? source.ElementType
                : lambda.ReturnType;

            var type = elementType.GetNonNullableType();
            var isNullableType = elementType.IsNullableType();

            if (type == typeof(decimal))
                return isNullableType
                    ? source.Provider.Execute<decimal?>(expression)
                    : source.Provider.Execute<decimal>(expression);
            if (type == typeof(float))
                return isNullableType
                    ? source.Provider.Execute<float?>(expression)
                    : source.Provider.Execute<float>(expression);
            if (type == typeof(double) || type == typeof(long) || type == typeof(int))
                return isNullableType
                    ? source.Provider.Execute<double?>(expression)
                    : source.Provider.Execute<double>(expression);

            throw new Exception("Call Linq Average Error.");
        }

        /// <summary>
        /// 用于：All、Any、Count
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="source"></param>
        /// <param name="method"></param>
        /// <param name="lambda"></param>
        /// <returns></returns>
        public static TResult Execute<TResult>(this IQueryable source, MethodInfo method, LambdaExpression lambda = null)
        {
            if (method.IsGenericMethod)
                method = method.MakeGenericMethod(source.ElementType);

            var expression = lambda == null
                ? Expression.Call(method, source.Expression)
                : Expression.Call(method, source.Expression, lambda);
            return source.Provider.Execute<TResult>(expression);
        }

        static MethodInfo Execute(this IQueryable source, Type returnType)
        {
            return source.Provider.GetType().GetTypeInfo()
                .GetDeclaredMethods(nameof(source.Provider.Execute))
                .First(_ => _.ContainsGenericParameters)
                .MakeGenericMethod(returnType);
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsNullableType(this Type type)
        {
            return type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Type GetNonNullableType(this Type type)
        {
            return IsNullableType(type) ? type.GetTypeInfo().GenericTypeParameters[0] : type;
        }
    }
}