using System;
using System.Linq.Expressions;
using Mkh.Data.Abstractions.Entities;

namespace Mkh.Data.Abstractions.Queryable.Grouping
{
    /// <summary>
    /// 单表分组查询对象
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TEntity2"></typeparam>
    public interface IGroupingQueryable<TKey, TEntity, TEntity2> : IGroupingQueryable where TEntity : IEntity where TEntity2 : IEntity
    {
        /// <summary>
        /// 聚合过滤
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        IGroupingQueryable<TKey, TEntity, TEntity2> Having(Expression<Func<IGrouping<TKey, TEntity, TEntity2>, bool>> expression);

        /// <summary>
        /// 聚合过滤
        /// </summary>
        /// <param name="havingSql">SQL语句</param>
        /// <returns></returns>
        IGroupingQueryable<TKey, TEntity, TEntity2> Having(string havingSql);

        /// <summary>
        /// 排序
        /// </summary>
        /// <param name="field">排序字段名称</param>
        /// <returns></returns>
        IGroupingQueryable<TKey, TEntity, TEntity2> OrderBy(string field);

        /// <summary>
        /// 排序
        /// </summary>
        /// <param name="field">排序字段名称</param>
        /// <returns></returns>
        IGroupingQueryable<TKey, TEntity, TEntity2> OrderByDescending(string field);

        /// <summary>
        /// 排序
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        IGroupingQueryable<TKey, TEntity, TEntity2> OrderBy<TResult>(Expression<Func<IGrouping<TKey, TEntity, TEntity2>, TResult>> expression);

        /// <summary>
        /// 倒序排序
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        IGroupingQueryable<TKey, TEntity, TEntity2> OrderByDescending<TResult>(Expression<Func<IGrouping<TKey, TEntity, TEntity2>, TResult>> expression);

        /// <summary>
        /// 查询列
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        IGroupingQueryable<TKey, TEntity, TEntity2> Select<TResult>(Expression<Func<IGrouping<TKey, TEntity, TEntity2>, TResult>> expression);
    }

    public interface IGrouping<out TKey, TEntity, TEntity2> : IGrouping<TKey> where TEntity : IEntity where TEntity2 : IEntity
    {
        TResult Max<TResult>(Expression<Func<TEntity, TEntity2, TResult>> where);

        TResult Min<TResult>(Expression<Func<TEntity, TEntity2, TResult>> where);

        TResult Sum<TResult>(Expression<Func<TEntity, TEntity2, TResult>> where);

        TResult Avg<TResult>(Expression<Func<TEntity, TEntity2, TResult>> where);
    }
}
