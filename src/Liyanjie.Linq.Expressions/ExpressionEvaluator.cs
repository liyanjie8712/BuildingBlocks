﻿using System.Collections.Generic;
using System.Linq.Expressions;

namespace Liyanjie.Linq.Expressions
{
    /// <summary>
    /// 
    /// </summary>
    public static class ExpressionEvaluator
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static object Evaluate(string input)
        {
            var parser = new ExpressionParser(null, null);
            var expression = parser.Parse(input);
            var function = Expression.Lambda(expression).Compile();
            var result = function.DynamicInvoke();
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input">表达式字符串</param>
        /// <param name="variables">变量字典</param>
        /// <returns></returns>
        public static object Evaluate(string input, ref IDictionary<string, object> variables)
        {
            var parser = new ExpressionParser(null, variables);
            var expression = parser.Parse(input);
            var function = Expression.Lambda(expression).Compile();
            var result = function.DynamicInvoke();
            variables = parser.Variables;
            return result;
        }
    }
}
