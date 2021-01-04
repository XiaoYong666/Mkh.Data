using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Mkh.Data.Abstractions.Entities;
using Mkh.Data.Abstractions.Pagination;

namespace Mkh.Data.Abstractions.Queryable
{
    /// <summary>
    /// 2表连接查询构造器
    /// </summary>
    public interface IQueryable<TEntity> : IQueryable where TEntity : IEntity, new()
    {
        #region ==Sort==

        /// <summary>
        /// 升序
        /// </summary>
        /// <param name="field">排序字段名称</param>
        /// <returns></returns>
        IQueryable<TEntity> OrderBy(string field);

        /// <summary>
        /// 降序
        /// </summary>
        /// <param name="field">排序字段名称</param>
        /// <returns></returns>
        IQueryable<TEntity> OrderByDescending(string field);

        /// <summary>
        /// 升序
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="expression">表达式</param>
        /// <returns></returns>
        IQueryable<TEntity> OrderBy<TKey>(Expression<Func<TEntity, TKey>> expression);

        /// <summary>
        /// 降序
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="expression">表达式</param>
        /// <returns></returns>
        IQueryable<TEntity> OrderByDescending<TKey>(Expression<Func<TEntity, TKey>> expression);

        #endregion

        #region ==Where==

        /// <summary>
        /// 过滤
        /// </summary>
        /// <param name="expression">过滤条件</param>
        /// <returns></returns>
        IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> expression);

        /// <summary>
        /// 附加SQL语句条件
        /// </summary>
        /// <param name="whereSql">查询条件</param>
        /// <returns></returns>
        IQueryable<TEntity> Where(string whereSql);

        /// <summary>
        /// 条件为true时添加过滤
        /// </summary>
        /// <param name="condition">添加条件</param>
        /// <param name="expression">条件</param>
        /// <returns></returns>
        IQueryable<TEntity> WhereIf(bool condition, Expression<Func<TEntity, bool>> expression);

        /// <summary>
        /// 条件为true时添加SQL语句条件
        /// </summary>
        /// <param name="condition">添加条件</param>
        /// <param name="whereSql">查询条件</param>
        /// <returns></returns>
        IQueryable<TEntity> WhereIf(bool condition, string whereSql);

        /// <summary>
        /// 根据条件添加过滤
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="ifExpression"></param>
        /// <param name="elseExpression"></param>
        /// <returns></returns>
        IQueryable<TEntity> WhereIfElse(bool condition, Expression<Func<TEntity, bool>> ifExpression, Expression<Func<TEntity, bool>> elseExpression);

        /// <summary>
        /// 根据条件添加SQL语句条件
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="ifWhereSql"></param>
        /// <param name="elseWhereSql"></param>
        /// <returns></returns>
        IQueryable<TEntity> WhereIfElse(bool condition, string ifWhereSql, string elseWhereSql);

        /// <summary>
        /// 字符串不为Null以及空字符串的时候添加过滤
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        IQueryable<TEntity> WhereNotNull(string condition, Expression<Func<TEntity, bool>> expression);

        /// <summary>
        /// 字符串不为Null以及空字符串的时候添加ifExpression，反之添加elseExpression
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="ifExpression"></param>
        /// <param name="elseExpression"></param>
        /// <returns></returns>
        IQueryable<TEntity> WhereNotNull(string condition, Expression<Func<TEntity, bool>> ifExpression, Expression<Func<TEntity, bool>> elseExpression);

        /// <summary>
        /// 字符串不为Null以及空字符串的时候添加SQL语句条件
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="whereSql"></param>
        /// <returns></returns>
        IQueryable<TEntity> WhereNotNull(string condition, string whereSql);

        /// <summary>
        /// 字符串不为Null以及空字符串的时候添加ifWhereSql，反之添加elseWhereSql
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="ifWhereSql"></param>
        /// <param name="elseWhereSql"></param>
        /// <returns></returns>
        IQueryable<TEntity> WhereNotNull(string condition, string ifWhereSql, string elseWhereSql);

        /// <summary>
        /// 对象不为Null以及空字符串的时候添加SQL语句条件
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        IQueryable<TEntity> WhereNotNull(object condition, Expression<Func<TEntity, bool>> expression);

        /// <summary>
        /// 对象不为Null的时候添加ifExpression，反之添加elseExpression
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="ifExpression"></param>
        /// <param name="elseExpression"></param>
        /// <returns></returns>
        IQueryable<TEntity> WhereNotNull(object condition, Expression<Func<TEntity, bool>> ifExpression, Expression<Func<TEntity, bool>> elseExpression);

        /// <summary>
        /// 对象不为Null以及空字符串的时候添加SQL语句条件
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="whereSql"></param>
        /// <returns></returns>
        IQueryable<TEntity> WhereNotNull(object condition, string whereSql);

        /// <summary>
        /// 对象不为Null的时候添加ifWhereSql，反之添加elseWhereSql
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="ifWhereSql"></param>
        /// <param name="elseWhereSql"></param>
        /// <returns></returns>
        IQueryable<TEntity> WhereNotNull(object condition, string ifWhereSql, string elseWhereSql);

        /// <summary>
        /// GUID不为空的时候添加过滤条件
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        IQueryable<TEntity> WhereNotEmpty(Guid condition, Expression<Func<TEntity, bool>> expression);

        /// <summary>
        /// GUID不为空的时候添加过滤SQL语句条件
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="whereSql"></param>
        /// <returns></returns>
        IQueryable<TEntity> WhereNotEmpty(Guid condition, string whereSql);

        /// <summary>
        /// GUID不为空的时候添加ifExpression，反之添加elseExpression
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="ifExpression"></param>
        /// <param name="elseExpression"></param>
        /// <returns></returns>
        IQueryable<TEntity> WhereNotEmpty(Guid condition, Expression<Func<TEntity, bool>> ifExpression, Expression<Func<TEntity, bool>> elseExpression);

        /// <summary>
        /// GUID不为空的时候添加ifExpression，反之添加elseExpression
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="ifWhereSql"></param>
        /// <param name="elseWhereSql"></param>
        /// <returns></returns>
        IQueryable<TEntity> WhereNotEmpty(Guid condition, string ifWhereSql, string elseWhereSql);

        #endregion

        #region ==SubQuery==

        /// <summary>
        /// 子查询等于
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="key">列</param>
        /// <param name="queryable">子查询的查询构造器</param>
        /// <returns></returns>
        IQueryable<TEntity> SubQueryEqual<TKey>(Expression<Func<TEntity, TKey>> key, IQueryable queryable);

        /// <summary>
        /// 子查询不等于
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="key">列</param>
        /// <param name="queryable">子查询的查询构造器</param>
        /// <returns></returns>
        IQueryable<TEntity> SubQueryNotEqual<TKey>(Expression<Func<TEntity, TKey>> key, IQueryable queryable);

        /// <summary>
        /// 子查询大于
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="key">列</param>
        /// <param name="queryable">子查询的查询构造器</param>
        /// <returns></returns>
        IQueryable<TEntity> SubQueryGreaterThan<TKey>(Expression<Func<TEntity, TKey>> key, IQueryable queryable);

        /// <summary>
        /// 子查询大于等于
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="key">列</param>
        /// <param name="queryable">子查询的查询构造器</param>
        /// <returns></returns>
        IQueryable<TEntity> SubQueryGreaterThanOrEqual<TKey>(Expression<Func<TEntity, TKey>> key, IQueryable queryable);

        /// <summary>
        /// 子查询小于
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="key">列</param>
        /// <param name="queryable">子查询的查询构造器</param>
        /// <returns></returns>
        IQueryable<TEntity> SubQueryLessThan<TKey>(Expression<Func<TEntity, TKey>> key, IQueryable queryable);

        /// <summary>
        /// 子查询小于等于
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="key">列</param>
        /// <param name="queryable">子查询的查询构造器</param>
        /// <returns></returns>
        IQueryable<TEntity> SubQueryLessThanOrEqual<TKey>(Expression<Func<TEntity, TKey>> key, IQueryable queryable);

        /// <summary>
        /// 子查询包含
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="key">列</param>
        /// <param name="queryable">子查询的查询构造器</param>
        /// <returns></returns>
        IQueryable<TEntity> SubQueryIn<TKey>(Expression<Func<TEntity, TKey>> key, IQueryable queryable);

        /// <summary>
        /// 子查询不包含
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="key">列</param>
        /// <param name="queryable">子查询的查询构造器</param>
        /// <returns></returns>
        IQueryable<TEntity> SubQueryNotIn<TKey>(Expression<Func<TEntity, TKey>> key, IQueryable queryable);

        #endregion

        #region ==Select==

        /// <summary>
        /// 查询返回指定列
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="expression">返回的列</param>
        /// <returns></returns>
        IQueryable<TEntity> Select<TResult>(Expression<Func<TEntity, TResult>> expression);

        /// <summary>
        /// 查询返回指定列
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="sql">SELECT后面的SQL语句，一般用于需要自定义的情况</param>
        /// <returns></returns>
        IQueryable<TEntity> Select<TResult>(string sql);

        /// <summary>
        /// 查询排除指定列
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        IQueryable<TEntity> SelectExclude<TResult>(Expression<Func<TEntity, TResult>> expression);

        #endregion

        #region ==Join==

        /// <summary>
        /// 左连接
        /// </summary>
        /// <param name="onExpression">on表达式</param>
        /// <param name="tableName">自定义表名</param>
        /// <param name="noLock">针对SqlServer的NoLock特性，默认开启</param>
        /// <typeparam name="TEntity2"></typeparam>
        /// <returns></returns>
        IQueryable<TEntity, TEntity2> LeftJoin<TEntity2>(Expression<Func<TEntity, TEntity2, bool>> onExpression, string tableName = null, bool noLock = true) where TEntity2 : IEntity, new();

        /// <summary>
        /// 内连接
        /// </summary>
        /// <typeparam name="TEntity2"></typeparam>
        /// <param name="onExpression"></param>
        /// <param name="tableName">自定义表名</param>
        /// <param name="noLock">针对SqlServer的NoLock特性，默认开启</param>
        /// <returns></returns>
        IQueryable<TEntity, TEntity2> InnerJoin<TEntity2>(Expression<Func<TEntity, TEntity2, bool>> onExpression, string tableName = null, bool noLock = true) where TEntity2 : IEntity, new();

        /// <summary>
        /// 右连接
        /// </summary>
        /// <typeparam name="TEntity2"></typeparam>
        /// <param name="onExpression"></param>
        /// <param name="tableName">自定义表名</param>
        /// <param name="noLock">针对SqlServer的NoLock特性，默认开启</param>
        /// <returns></returns>
        IQueryable<TEntity, TEntity2> RightJoin<TEntity2>(Expression<Func<TEntity, TEntity2, bool>> onExpression, string tableName = null, bool noLock = true) where TEntity2 : IEntity, new();

        #endregion

        #region ==NotFilterSoftDeleted==

        /// <summary>
        /// 不过滤软删除的数据
        /// </summary>
        /// <returns></returns>
        IQueryable<TEntity> NotFilterSoftDeleted();

        #endregion

        #region ==NotFilterTenant==

        /// <summary>
        /// 不过滤租户
        /// </summary>
        /// <returns></returns>
        IQueryable<TEntity> NotFilterTenant();

        #endregion

        #region ==Copy==

        /// <summary>
        /// 复制当前查询实例
        /// </summary>
        /// <returns></returns>
        IQueryable<TEntity> Copy();

        #endregion

        #region ==Update==

        /// <summary>
        /// 更新
        /// <para>数据不存在也是返回true</para>
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        Task Update(Expression<Func<TEntity, TEntity>> expression);

        /// <summary>
        /// 更新
        /// <para>数据不存在也是返回true</para>
        /// </summary>
        /// <param name="updateSql">手写更新sql</param>
        /// <param name="parameterObject">参数对象</param>
        /// <returns></returns>
        Task Update(string updateSql, object parameterObject = null);

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
    }
}
