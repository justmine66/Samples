using System;

namespace SFtpDownloader
{
    /// <summary>
    /// 数据缓存接口
    /// </summary>
    /// <typeparam name="TSource">数据源类型</typeparam>
    public interface IDataCache<TSource>
    {
        /// <summary>
        /// 根据缓存键获取源数据。
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns>已缓存的数据</returns>
        TSource Get(string key);

        /// <summary>
        /// 根据缓存键获取源数据，没有则添加。
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="createSourceFunctor">添加缓存的函数</param>
        /// <returns>已缓存的数据</returns>
        TSource GetOrAdd(string key, Func<TSource> createSourceFunctor);

        /// <summary>
        /// 尝试添加缓存。
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="source">数据</param>
        /// <returns>是否添加成功</returns>
        bool TryAdd(string key, TSource source);

        /// <summary>
        /// 尝试移除缓存。
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns>是否移除成功</returns>
        bool TryRemove(string key);

        /// <summary>
        /// 清空所有缓存。
        /// </summary>
        void Clear();
    }

}
