using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace WebApiCore.EF.DataBase
{
    public class DBExtension
    {
        /// <summary>
        /// 分页帮助
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tmpData"></param>
        /// <param name="sort"></param>
        /// <param name="isAsc"></param>
        /// <returns></returns>
        public static IQueryable<T> PaginationSort<T>(IQueryable<T> tmpData, string sort, bool isAsc) where T : class, new()
        {
            string[] sortArr = sort.Split(',');

            MethodCallExpression resultExpression = null;

            for (int index = 0; index < sortArr.Length; index++)
            {
                string[] sortColAndRuleArr = sortArr[index].Trim().Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                string sortField = sortColAndRuleArr.First();
                bool sortAsc = isAsc;

                //排序列带上规则   "Id Asc"
                if (sortColAndRuleArr.Length == 2)
                {
                    sortAsc = string.Equals(sortColAndRuleArr[1], "asc", StringComparison.OrdinalIgnoreCase);
                }

                var parameter = Expression.Parameter(typeof(T), "type");
                var property = typeof(T).GetProperties().First(p => p.Name.Equals(sortField));
                var propertyAccess = Expression.MakeMemberAccess(parameter, property);
                var orderByExpression = Expression.Lambda(propertyAccess, parameter);

                if (index == 0)
                {
                    resultExpression = Expression.Call(
                        typeof(Queryable), //调用的类型
                        sortAsc ? "OrderBy" : "OrderByDescending", //方法名称
                        new[] { typeof(T), property.PropertyType }, tmpData.Expression, Expression.Quote(orderByExpression));
                }
                else
                {
                    resultExpression = Expression.Call(
                        typeof(Queryable),
                        sortAsc ? "ThenBy" : "ThenByDescending",
                        new[] { typeof(T), property.PropertyType }, tmpData.Expression, Expression.Quote(orderByExpression));
                }

                tmpData = tmpData.Provider.CreateQuery<T>(resultExpression);
            }
            return tmpData;
        }
    }
}
