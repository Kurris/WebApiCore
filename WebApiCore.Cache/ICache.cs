﻿using System;
using System.Collections.Generic;
using System.Text;

namespace WebApiCore.Cache
{
    public interface ICache
    {
        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <typeparam name="T">缓存类型</typeparam>
        /// <param name="key">主键</param>
        /// <param name="value">缓存值</param>
        /// <param name="expireDateTime">失效时间,具体时间点</param>
        /// <returns>添加结果</returns>
        bool SetCache<T>(string key, T value, DateTime? expireDateTime = null);

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <typeparam name="T">缓存类型</typeparam>
        /// <param name="key">主键</param>
        /// <returns>缓存值</returns>
        T GetCache<T>(string key);

        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="key">主键</param>
        /// <returns>移除结果</returns>
        bool RemoveCache(string key);
    }
}
