﻿using System;
using Microsoft.EntityFrameworkCore;
using WebApiCore.Data.EF.DataBase;
using WebApiCore.Lib.Utils;

namespace WebApiCore.Data.EF
{
    /// <summary>
    /// 初始化数据库上下文
    /// </summary>
    public class EFDB
    {
        /// <summary>
        /// 具体数据库实例
        /// </summary>
        public static IDataBaseOperation Instance
        {
            get
            {
                return GlobalInvariant.SystemConfig.DBConfig.Provider switch
                {
                    "SqlServer" => new SqlServerDB(GlobalInvariant.SystemConfig.DBConfig.Provider,
                           GlobalInvariant.SystemConfig.DBConfig.SqlServerConnectionString).GetIDataBaseOperation(),
                    "MySql" => new MySqlDB(GlobalInvariant.SystemConfig.DBConfig.Provider,
                           GlobalInvariant.SystemConfig.DBConfig.MySqlConnectionString).GetIDataBaseOperation(),
                    _ => throw new NotImplementedException("未知的数据引擎")
                };
            }
        }


        /// <summary>
        /// 创建具体数据库
        /// </summary>
        /// <remarks>
        /// 不建议使用此方法
        /// </remarks>
        /// <param name="provider">数据库引擎</param>
        /// <param name="connStr">数据库连接字符串</param>
        /// <returns>操作对象<see cref="IDataBaseOperation"/></returns>
        public static IDataBaseOperation Create(string provider = null, string connStr = null)
        {
            provider = GlobalInvariant.SystemConfig?.DBConfig?.Provider ?? provider;

            switch (provider)
            {
                case "SqlServer":
                    connStr = GlobalInvariant.SystemConfig?.DBConfig?.SqlServerConnectionString ?? connStr;
                    return new SqlServerDB(provider, connStr).GetIDataBaseOperation();
                case "MySql":
                    connStr = GlobalInvariant.SystemConfig?.DBConfig?.MySqlConnectionString ?? connStr;
                    return new MySqlDB(provider, connStr).GetIDataBaseOperation();


                default:
                    throw new NotImplementedException("未知的数据引擎");
            }
        }
    }

    public static class IDataBaseOperationExtension
    {
        /// <summary>
        /// DbContext 转成无跟踪
        /// </summary>
        /// <param name="operation"></param>
        /// <returns></returns>
        public static IDataBaseOperation AsNoTracking(this IDataBaseOperation operation)
        {
            operation.DbContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            return operation;
        }

        /// <summary>
        /// DbContext 转成跟踪
        /// </summary>
        /// <param name="operation"></param>
        /// <returns></returns>
        public static IDataBaseOperation AsTracking(this IDataBaseOperation operation)
        {
            operation.DbContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll;
            return operation;
        }
    }
}
