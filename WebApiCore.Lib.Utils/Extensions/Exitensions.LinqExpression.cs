using System;
using System.Linq.Expressions;

namespace WebApiCore.Lib.Utils.Extensions
{
    public static class LinqExpression
    {
        public static Expression<Func<T, bool>> True<T>() { return x => true; }
        public static Expression<Func<T, bool>> False<T>() { return x => false; }

        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> predicateCurrent, Expression<Func<T, bool>> predicateAddition) where T : class
        {
            return Combine(predicateCurrent, predicateAddition, Expression.AndAlso);
        }
        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> predicateCurrent, Expression<Func<T, bool>> predicateAddition) where T : class
        {
            return Combine(predicateCurrent, predicateAddition, Expression.OrElse);
        }

        public static Expression<Func<T, bool>> Not<T>(this Expression<Func<T, bool>> predicateCurrent) where T : class
        {
            ExpressionReplace ex = GenExpressionReplace<T>(out var expressionPara);
            var current = ex.Replace(predicateCurrent);
            var body = Expression.Not(current);

            return Expression.Lambda<Func<T, bool>>(body, expressionPara);
        }

        private static Expression<Func<T,bool>> Combine<T>(Expression<Func<T, bool>> expressionFirst, Expression<Func<T, bool>> expressionSecond, Func<Expression, Expression, Expression> expressionMethod)
        {
            ExpressionReplace ex = GenExpressionReplace<T>(out var expressionPara);

            var left = ex.Replace(expressionFirst.Body);
            var right = ex.Replace(expressionSecond.Body);

            return Expression.Lambda<Func<T, bool>>(expressionMethod(left, right), expressionPara);
        }

        private static ExpressionReplace GenExpressionReplace<T>(out ParameterExpression expressionPara)
        {
            expressionPara = Expression.Parameter(typeof(T), "x");
            return new ExpressionReplace(expressionPara);
        }
    }


    internal class ExpressionReplace : ExpressionVisitor
    {
        public ParameterExpression ParameterExpression { get; private set; }

        public ExpressionReplace(ParameterExpression parameterExpression)
        {
            this.ParameterExpression = parameterExpression;
        }

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
