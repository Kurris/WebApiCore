using System;
using System.Linq.Expressions;

namespace WebApiCore.Lib.Utils.Extensions
{
    public static partial class Extensions
    {
        /// <summary>
        /// 返回一个默认<see cref="true"/>的表达式
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <returns><see cref="Expression{Func{T, bool}}"/></returns>
        public static Expression<Func<T, bool>> True<T>() { return x => true; }

        /// <summary>
        /// 返回一个默认<see cref="false"/>的表达式
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <returns><see cref="Expression{Func{T, bool}}"/></returns>
        public static Expression<Func<T, bool>> False<T>() { return x => false; }

        /// <summary>
        /// 并且
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="predicateCurrent">当前表达式</param>
        /// <param name="predicateAddition">并且的表达式</param>
        /// <returns><see cref="Expression{Func{T, bool}}"/></returns>
        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> predicateCurrent, Expression<Func<T, bool>> predicateAddition) where T : class
        {
            return Combine(predicateCurrent, predicateAddition, Expression.AndAlso);
        }

        /// <summary>
        /// 或者
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="predicateCurrent">当前表达式</param>
        /// <param name="predicateAddition">并且的表达式</param>
        /// <returns><see cref="Expression{Func{T, bool}}"/></returns>
        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> predicateCurrent, Expression<Func<T, bool>> predicateAddition) where T : class
        {
            return Combine(predicateCurrent, predicateAddition, Expression.OrElse);
        }

        /// <summary>
        /// 否
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="predicateCurrent">当前表达式</param>
        /// <returns><see cref="Expression{Func{T, bool}}"/></returns>
        public static Expression<Func<T, bool>> Not<T>(this Expression<Func<T, bool>> predicateCurrent) where T : class
        {
            ExpressionReplace ex = GenExpressionReplace<T>(out var expressionPara);
            var current = ex.Replace(predicateCurrent);
            var body = Expression.Not(current);

            return Expression.Lambda<Func<T, bool>>(body, expressionPara);
        }

        /// <summary>
        /// 合并俩个表达式
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="expressionFirst">第一个表达式</param>
        /// <param name="expressionSecond">第二个表达式</param>
        /// <param name="expressionMethod">合并方法</param>
        /// <returns><see cref="Expression{Func{T, bool}}"/></returns>
        private static Expression<Func<T, bool>> Combine<T>(Expression<Func<T, bool>> expressionFirst, Expression<Func<T, bool>> expressionSecond, Func<Expression, Expression, Expression> expressionMethod)
        {
            ExpressionReplace ex = GenExpressionReplace<T>(out var expressionPara);

            var left = ex.Replace(expressionFirst.Body);
            var right = ex.Replace(expressionSecond.Body);

            return Expression.Lambda<Func<T, bool>>(expressionMethod(left, right), expressionPara);
        }

        /// <summary>
        /// 表达式替换帮助类
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expressionPara">引用相等的表达式参数</param>
        /// <returns><see cref="ExpressionReplace"/></returns>
        private static ExpressionReplace GenExpressionReplace<T>(out ParameterExpression expressionPara)
        {
            expressionPara = Expression.Parameter(typeof(T), "x");
            return new ExpressionReplace(expressionPara);
        }
    }

    /// <summary>
    /// 表达式替换帮助类
    /// </summary>
    internal class ExpressionReplace : ExpressionVisitor
    {
        /// <summary>
        /// 当前的表达式参数
        /// </summary>
        public ParameterExpression ParameterExpression { get; private set; }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="parameterExpression">表达式参数</param>
        public ExpressionReplace(ParameterExpression parameterExpression)
        {
            this.ParameterExpression = parameterExpression;
        }

        /// <summary>
        /// 替换成相同的引用
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public Expression Replace(Expression expression)
        {
            return this.Visit(expression);
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            return this.ParameterExpression;
        }
    }
}
