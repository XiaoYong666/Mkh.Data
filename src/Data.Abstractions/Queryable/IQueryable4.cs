﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Mkh.Data.Abstractions.Entities;
using Mkh.Data.Abstractions.Pagination;

namespace Mkh.Data.Abstractions.Queryable
{
    public interface IQueryable<TEntity, TEntity2, TEntity3, TEntity4> : IQueryable
        where TEntity : IEntity, new()
        where TEntity2 : IEntity, new()
        where TEntity3 : IEntity, new()
        where TEntity4 : IEntity, new()
    {

        #region ==Sort==

        /// <summary>
        /// 升序
        /// </summary>
        /// <param name="colName">列名</param>
        /// <returns></returns>
        IQueryable<TEntity, TEntity2, TEntity3, TEntity4> OrderBy(string colName);

        /// <summary>
        /// 降序
        /// </summary>
        /// <param name="colName">列名</param>
        /// <returns></returns>
        IQueryable<TEntity, TEntity2, TEntity3, TEntity4> OrderByDescending(string colName);

        /// <summary>
        /// 升序
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        IQueryable<TEntity, TEntity2, TEntity3, TEntity4> OrderBy<TKey>(Expression<Func<TEntity, TEntity2, TEntity3, TEntity4, TKey>> expression);

        /// <summary>
        /// 降序
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        IQueryable<TEntity, TEntity2, TEntity3, TEntity4> OrderByDescending<TKey>(Expression<Func<TEntity, TEntity2, TEntity3, TEntity4, TKey>> expression);

        /// <summary>
        /// 排序
        /// </summary>
        /// <param name="sort"></param>
        /// <returns></returns>
        IQueryable<TEntity, TEntity2, TEntity3, TEntity4> OrderBy(Sort sort);

        /// <summary>
        /// 排序
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="expression"></param>
        /// <param name="sortType"></param>
        /// <returns></returns>
        IQueryable<TEntity, TEntity2, TEntity3, TEntity4> OrderBy<TKey>(Expression<Func<TEntity, TEntity2, TEntity3, TEntity4, TKey>> expression, SortType sortType);

        #endregion

        #region ==Where==

        /// <summary>
        /// 过滤
        /// </summary>
        /// <param name="expression">过滤条件</param>
        /// <returns></returns>
        IQueryable<TEntity, TEntity2, TEntity3, TEntity4> Where(Expression<Func<TEntity, TEntity2, TEntity3, TEntity4, bool>> expression);

        /// <summary>
        /// 过滤
        /// </summary>
        /// <param name="whereSql">过滤条件</param>
        /// <returns></returns>
        IQueryable<TEntity, TEntity2, TEntity3, TEntity4> Where(string whereSql);

        /// <summary>
        /// 过滤
        /// </summary>
        /// <param name="condition">添加条件</param>
        /// <param name="expression">条件</param>
        /// <returns></returns>
        IQueryable<TEntity, TEntity2, TEntity3, TEntity4> WhereIf(bool condition, Expression<Func<TEntity, TEntity2, TEntity3, TEntity4, bool>> expression);

        /// <summary>
        /// 过滤
        /// </summary>
        /// <param name="condition">添加条件</param>
        /// <param name="whereSql">条件</param>
        /// <returns></returns>
        IQueryable<TEntity, TEntity2, TEntity3, TEntity4> WhereIf(bool condition, string whereSql);

        /// <summary>
        /// 过滤
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="ifExpression"></param>
        /// <param name="elseExpression"></param>
        /// <returns></returns>
        IQueryable<TEntity, TEntity2, TEntity3, TEntity4> WhereIfElse(bool condition, Expression<Func<TEntity, TEntity2, TEntity3, TEntity4, bool>> ifExpression, Expression<Func<TEntity, TEntity2, TEntity3, TEntity4, bool>> elseExpression);

        /// <summary>
        /// 过滤
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="ifWhereSql"></param>
        /// <param name="elseWhereSql"></param>
        /// <returns></returns>
        IQueryable<TEntity, TEntity2, TEntity3, TEntity4> WhereIfElse(bool condition, string ifWhereSql, string elseWhereSql);

        /// <summary>
        /// 字符串不为Null以及空字符串的时候添加过滤
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        IQueryable<TEntity, TEntity2, TEntity3, TEntity4> WhereNotNull(string condition, Expression<Func<TEntity, TEntity2, TEntity3, TEntity4, bool>> expression);

        /// <summary>
        /// 字符串不为Null以及空字符串的时候添加过滤
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="whereSql"></param>
        /// <returns></returns>
        IQueryable<TEntity, TEntity2, TEntity3, TEntity4> WhereNotNull(string condition, string whereSql);

        /// <summary>
        /// 不为Null以及Empty的时候添加过滤
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        IQueryable<TEntity, TEntity2, TEntity3, TEntity4> WhereNotNull(object condition, Expression<Func<TEntity, TEntity2, TEntity3, TEntity4, bool>> expression);

        /// <summary>
        /// 不为Null以及Empty的时候添加过滤
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="whereSql"></param>
        /// <returns></returns>
        IQueryable<TEntity, TEntity2, TEntity3, TEntity4> WhereNotNull(object condition, string whereSql);

        /// <summary>
        /// GUID不为空的时候添加过滤条件
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        IQueryable<TEntity, TEntity2, TEntity3, TEntity4> WhereNotEmpty(Guid condition, Expression<Func<TEntity, TEntity2, TEntity3, TEntity4, bool>> expression);

        /// <summary>
        /// GUID不为空的时候添加过滤条件
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="whereSql"></param>
        /// <returns></returns>
        IQueryable<TEntity, TEntity2, TEntity3, TEntity4> WhereNotEmpty(Guid condition, string whereSql);

        /// <summary>
        /// NotIn查询
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="key"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        IQueryable<TEntity, TEntity2, TEntity3, TEntity4> WhereNotIn<TKey>(Expression<Func<TEntity, TEntity2, TEntity3, TEntity4, TKey>> key, IEnumerable<TKey> list);

        #endregion

        #region ==SubQuery==

        /// <summary>
        /// 子查询等于
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="key">列</param>
        /// <param name="queryable">子查询的查询构造器</param>
        /// <returns></returns>
        IQueryable<TEntity, TEntity2, TEntity3, TEntity4> SubQueryEqual<TKey>(Expression<Func<TEntity, TKey>> key, IQueryable queryable);

        /// <summary>
        /// 子查询不等于
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="key">列</param>
        /// <param name="queryable">子查询的查询构造器</param>
        /// <returns></returns>
        IQueryable<TEntity, TEntity2, TEntity3, TEntity4> SubQueryNotEqual<TKey>(Expression<Func<TEntity, TKey>> key, IQueryable queryable);

        /// <summary>
        /// 子查询大于
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="key">列</param>
        /// <param name="queryable">子查询的查询构造器</param>
        /// <returns></returns>
        IQueryable<TEntity, TEntity2, TEntity3, TEntity4> SubQueryGreaterThan<TKey>(Expression<Func<TEntity, TKey>> key, IQueryable queryable);

        /// <summary>
        /// 子查询大于等于
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="key">列</param>
        /// <param name="queryable">子查询的查询构造器</param>
        /// <returns></returns>
        IQueryable<TEntity, TEntity2, TEntity3, TEntity4> SubQueryGreaterThanOrEqual<TKey>(Expression<Func<TEntity, TKey>> key, IQueryable queryable);

        /// <summary>
        /// 子查询小于
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="key">列</param>
        /// <param name="queryable">子查询的查询构造器</param>
        /// <returns></returns>
        IQueryable<TEntity, TEntity2, TEntity3, TEntity4> SubQueryLessThan<TKey>(Expression<Func<TEntity, TKey>> key, IQueryable queryable);

        /// <summary>
        /// 子查询小于等于
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="key">列</param>
        /// <param name="queryable">子查询的查询构造器</param>
        /// <returns></returns>
        IQueryable<TEntity, TEntity2, TEntity3, TEntity4> SubQueryLessThanOrEqual<TKey>(Expression<Func<TEntity, TKey>> key, IQueryable queryable);

        /// <summary>
        /// 子查询包含
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="key">列</param>
        /// <param name="queryable">子查询的查询构造器</param>
        /// <returns></returns>
        IQueryable<TEntity, TEntity2, TEntity3, TEntity4> SubQueryIn<TKey>(Expression<Func<TEntity, TKey>> key, IQueryable queryable);

        /// <summary>
        /// 子查询不包含
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="key">列</param>
        /// <param name="queryable">子查询的查询构造器</param>
        /// <returns></returns>
        IQueryable<TEntity, TEntity2, TEntity3, TEntity4> SubQueryNotIn<TKey>(Expression<Func<TEntity, TKey>> key, IQueryable queryable);

        #endregion

        #region ==Limit==

        /// <summary>
        /// 限制
        /// </summary>
        /// <param name="skip">跳过前几条数据</param>
        /// <param name="take">取前几条数据</param>
        /// <returns></returns>
        IQueryable<TEntity, TEntity2, TEntity3, TEntity4> Limit(int skip, int take);

        #endregion

        #region ==Select==

        /// <summary>
        /// 查询指定列
        /// </summary>
        /// <returns></returns>
        IQueryable<TEntity, TEntity2, TEntity3, TEntity4> Select<TResult>(Expression<Func<TEntity, TEntity2, TEntity3, TEntity4, TResult>> selectExpression);

        /// <summary>
        /// 查询排除指定列
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        IQueryable<TEntity, TEntity2, TEntity3, TEntity4> SelectExclude<TResult>(Expression<Func<TEntity, TEntity2, TEntity3, TEntity4, TResult>> expression);

        #endregion

        #region ==Join==

        /// <summary>
        /// 左连接
        /// </summary>
        /// <typeparam name="TEntity5"></typeparam>
        /// <param name="onExpression"></param>
        /// <param name="tableName"></param>
        /// <param name="noLock">针对SqlServer的NoLock特性，默认开启</param>
        /// <returns></returns>
        IQueryable<TEntity, TEntity2, TEntity3, TEntity4, TEntity5> LeftJoin<TEntity5>(Expression<Func<TEntity, TEntity2, TEntity3, TEntity4, TEntity5, bool>> onExpression, string tableName = null, bool noLock = true) where TEntity5 : IEntity, new();

        /// <summary>
        /// 内连接
        /// </summary>
        /// <typeparam name="TEntity5"></typeparam>
        /// <param name="onExpression"></param>
        /// <param name="tableName"></param>
        /// <param name="noLock">针对SqlServer的NoLock特性，默认开启</param>
        /// <returns></returns>
        IQueryable<TEntity, TEntity2, TEntity3, TEntity4, TEntity5> InnerJoin<TEntity5>(Expression<Func<TEntity, TEntity2, TEntity3, TEntity4, TEntity5, bool>> onExpression, string tableName = null, bool noLock = true) where TEntity5 : IEntity, new();

        /// <summary>
        /// 右连接
        /// </summary>
        /// <typeparam name="TEntity5"></typeparam>
        /// <param name="onExpression"></param>
        /// <param name="tableName"></param>
        /// <param name="noLock">针对SqlServer的NoLock特性，默认开启</param>
        /// <returns></returns>
        IQueryable<TEntity, TEntity2, TEntity3, TEntity4, TEntity5> RightJoin<TEntity5>(Expression<Func<TEntity, TEntity2, TEntity3, TEntity4, TEntity5, bool>> onExpression, string tableName = null, bool noLock = true) where TEntity5 : IEntity, new();

        #endregion

        #region ==List==

        /// <summary>
        /// 查询列表，返回指定类型
        /// </summary>
        /// <returns></returns>
        Task<IList<TEntity>> List();

        #endregion

        #region ==Pagination==

        /// <summary>
        /// 分页查询，返回实体类型
        /// </summary>
        /// <returns></returns>
        Task<IList<TEntity>> Pagination();

        /// <summary>
        /// 分页查询，返回实体类型
        /// </summary>
        /// <param name="paging">分页对象</param>
        /// <returns></returns>
        Task<IList<TEntity>> Pagination(Paging paging);

        #endregion

        #region ==First==

        /// <summary>
        /// 查询第一条数据，返回指定类型
        /// </summary>
        /// <returns></returns>
        Task<TEntity> First();

        #endregion

        #region ==Update==

        /// <summary>
        /// 更新
        /// <para>数据不存在也是返回true</para>
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        Task<bool> Update(Expression<Func<TEntity, TEntity>> expression);

        /// <summary>
        /// 更新
        /// <para>数据不存在也是返回true</para>
        /// </summary>
        /// <param name="updateSql">手写更新sql</param>
        /// <param name="parameterObject">参数对象</param>
        /// <returns></returns>
        Task<bool> Update(string updateSql, object parameterObject = null);

        /// <summary>
        /// 更新数据返回影响条数
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="updateSql">更新SQL语句，优于表达式</param>
        /// <param name="parameterObject">参数对象</param>
        /// <returns></returns>
        Task<int> UpdateWithAffectedNum(Expression<Func<TEntity, TEntity>> expression, string updateSql = null, object parameterObject = null);

        #endregion

        #region ==NotFilterSoftDeleted==

        /// <summary>
        /// 不过滤软删除数据
        /// </summary>
        /// <returns></returns>
        IQueryable<TEntity, TEntity2, TEntity3, TEntity4> NotFilterSoftDeleted();

        #endregion

        #region ==NotFilterTenant==

        /// <summary>
        /// 不过滤租户
        /// </summary>
        /// <returns></returns>
        IQueryable<TEntity, TEntity2, TEntity3, TEntity4> NotFilterTenant();

        #endregion

        #region ==Function==

        /// <summary>
        /// 获取最大值
        /// </summary>
        /// <returns></returns>
        Task<TResult> Max<TResult>(Expression<Func<TEntity, TEntity2, TEntity3, TEntity4, TResult>> expression);

        /// <summary>
        /// 获取最小值
        /// </summary>
        /// <returns></returns>
        Task<TResult> Min<TResult>(Expression<Func<TEntity, TEntity2, TEntity3, TEntity4, TResult>> expression);

        /// <summary>
        /// 求和
        /// </summary>
        /// <returns></returns>
        Task<TResult> Sum<TResult>(Expression<Func<TEntity, TEntity2, TEntity3, TEntity4, TResult>> expression);

        /// <summary>
        /// 求平均值
        /// </summary>
        /// <returns></returns>
        Task<TResult> Avg<TResult>(Expression<Func<TEntity, TEntity2, TEntity3, TEntity4, TResult>> expression);

        #endregion

        #region ==Copy==

        /// <summary>
        /// 复制一个新的实例
        /// </summary>
        /// <returns></returns>
        IQueryable<TEntity, TEntity2, TEntity3, TEntity4> Copy();

        #endregion
    }
}
