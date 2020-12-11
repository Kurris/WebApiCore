﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WebApiCore.Entity;

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
        public static IQueryable<T> PaginationSort<T>(IQueryable<T> tmpData, string sort, bool isAsc) where T : class
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

        /// <summary>
        /// 递归附加
        /// </summary>
        /// <param name="dbContext">当前上下文</param>
        /// <param name="entity">实例</param>
        public static void RecursionAttach(DbContext dbContext, object entity)
        {

            var entityType = FindTrackingEntity(dbContext, entity);

            if (entityType == null)
                dbContext.Attach(entity);
            else if (entityType.State == EntityState.Modified)
                return;

            foreach (var prop in entity.GetType().GetProperties().Where(x => !x.IsDefined(typeof(NotMappedAttribute), false)))
            {
                if (prop.Name.Equals(entity.GetType().Name + "Id", StringComparison.OrdinalIgnoreCase)) continue;

                object obj = prop.GetValue(entity);
                if (obj == null) continue;

                var subEntityType = FindTrackingEntity(dbContext, obj);

                //List<Entity>
                if (prop.PropertyType.IsGenericType)
                {
                    IEnumerable<object> objs = (IEnumerable<object>)obj;
                    foreach (var item in objs)
                    {
                        RecursionAttach(dbContext, item);
                    }
                }
                //string/int
                else if (subEntityType == null)
                {
                    dbContext.Entry(entity).Property(prop.Name).IsModified = true;
                }
                //Entity
                else if (subEntityType != null && subEntityType.State == EntityState.Unchanged)
                {
                    RecursionAttach(dbContext, obj);
                }
            }
        }

        /// <summary>
        /// 根据ID匹配是否存在
        /// </summary>
        /// <param name="dbContext">当前上下文</param>
        /// <param name="entity">实例</param>
        /// <returns></returns>
        private static EntityEntry FindTrackingEntity(DbContext dbContext, object entity)
        {
            foreach (var item in dbContext.ChangeTracker.Entries())
            {
                string key = entity.GetType().Name + "Id";

                try
                {
                    int tracking = (int)item.Property(key).CurrentValue;
                    int now = (int)entity.GetType().GetProperty(key)?.GetValue(entity);

                    if (tracking == now)
                    {
                        return item;
                    }
                }
                catch
                {

                }
            }
            return null;
        }
    }
}
