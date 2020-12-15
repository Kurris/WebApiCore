using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace WebApiCore.EF.DataBase.Extension
{
    /// <summary>
    /// 表达式目录树Vistor
    /// </summary>
    internal class ConditionBuilderVisitor : ExpressionVisitor
    {
        internal ConditionBuilderVisitor()
        {

        }

        private readonly Stack<string> _stringStack = new Stack<string>();

        /// <summary>
        /// 条件组合
        /// </summary>
        /// <param name="addWhere">是否添加WHERE</param>
        /// <returns></returns>
        public string Combine(bool addWhere = false)
        {
            string condition = string.Concat(this._stringStack);
            this._stringStack.Clear();
            if (addWhere)
            {
                return "WHERE " + condition;
            }
            return condition;
        }

        /// <summary>
        /// 二元表达式
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        protected override Expression VisitBinary(BinaryExpression node)
        {
            this._stringStack.Push(")");
            base.Visit(node.Right);
            this._stringStack.Push(" " + ConditionType(node.NodeType) + " ");
            base.Visit(node.Left);
            this._stringStack.Push("(");

            return node;
        }


        /// <summary>
        /// 访问成员
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        protected override Expression VisitMember(MemberExpression node)
        {
            if (node.Expression is ParameterExpression)
            {
                this._stringStack.Push(" [" + node.Member.Name + "] ");
                return node;
            }
            else
            {
                return this.VisitConstant(node.Expression as ConstantExpression);
            }
        }

        /// <summary>
        /// 访问常量
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        protected override Expression VisitConstant(ConstantExpression node)
        {
            if (node.Type.Name.Contains("<>") && node.Type.Name.Contains("DisplayClass"))
            {
                this._stringStack.Push(" '" + node.Type.GetFields()[0].GetValue(node.Value) + "' ");
            }
            else
            {
                this._stringStack.Push(" '" + node.Value + "' ");
            }

            return node;
        }

        /// <summary>
        /// 访问方法
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            string methodName = node.Method.Name;
            string format = methodName switch
            {
                "Contains" => "({0} LIKE '%'+{1}+'%')",
                "StartsWith" => "({0} LIKE {1}+'%')",
                "EndsWith" => "({0} LIKE '%'+{1})",
                "Equals" => "({0} = {1})",
                _ => throw new NotSupportedException(node.Method.Name),
            };
            this.Visit(node.Object);
            this.Visit(node.Arguments[0]);
            string right = this._stringStack.Pop();
            string left = this._stringStack.Pop();
            this._stringStack.Push(string.Format(format, left, right));
            
            return node;
        }


        /// <summary>
        /// 条件类型
        /// </summary>
        /// <param name="expressionType"></param>
        /// <returns></returns>
        public string ConditionType(ExpressionType expressionType)
        {
            switch (expressionType)
            {
                case ExpressionType.Add:
                    return "+";
                case ExpressionType.And:
                case ExpressionType.AndAlso:
                    return "AND";
                case ExpressionType.Equal:
                    return "=";
                case ExpressionType.GreaterThan:
                    return ">";
                case ExpressionType.GreaterThanOrEqual:
                    return ">=";
                case ExpressionType.LessThan:
                    return "<";
                case ExpressionType.LessThanOrEqual:
                    return "<=";
                case ExpressionType.Multiply:
                    return "*";
                case ExpressionType.Not:
                    return "NOT";
                case ExpressionType.NotEqual:
                    return "<>";
                case ExpressionType.Or:
                case ExpressionType.OrElse:
                    return "OR";
                case ExpressionType.Subtract:
                    return "-";
                default:
                    break;
            }

            throw new NotSupportedException();
        }
    }
}
