﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Mkh.Data.Abstractions.Entities;
using Mkh.Data.Abstractions.Pagination;
using Mkh.Data.Abstractions.Queryable;
using Mkh.Data.Core.Queryable.Internal;
using IQueryable = Mkh.Data.Abstractions.Queryable.IQueryable;

namespace Mkh.Data.Core.Queryable
{
    internal class Queryable<TEntity, TEntity2, TEntity3, TEntity4, TEntity5> : QueryableAbstract, IQueryable<TEntity, TEntity2, TEntity3, TEntity4, TEntity5> where TEntity : IEntity, new() where TEntity2 : IEntity, new() where TEntity3 : IEntity, new() where TEntity4 : IEntity, new() where TEntity5 : IEntity, new()
    {
        public Queryable(QueryBody queryBody, JoinType joinType, Expression<Func<TEntity, TEntity2, TEntity3, TEntity4, TEntity5, bool>> onExpression, string tableName, bool noLock) : base(queryBody)
        {
            var entityDescriptor = _queryBody.GetEntityDescriptor<TEntity5>();
            var join = new QueryJoin(entityDescriptor, "T5", joinType, onExpression, noLock);
            join.TableName = tableName.NotNull() ? tableName : entityDescriptor.TableName;

            _queryBody.Joins.Add(join);
        }

        private Queryable(QueryBody queryBody) : base(queryBody)
        {

        }

        #region ==Sort==

        public IQueryable<TEntity, TEntity2, TEntity3, TEntity4, TEntity5> OrderBy(string field)
        {
            _queryBody.SetSort(field, SortType.Asc);
            return this;
        }

        public IQueryable<TEntity, TEntity2, TEntity3, TEntity4, TEntity5> OrderByDescending(string field)
        {
            _queryBody.SetSort(field, SortType.Desc);
            return this;
        }

        public IQueryable<TEntity, TEntity2, TEntity3, TEntity4, TEntity5> OrderBy<TKey>(Expression<Func<TEntity, TEntity2, TEntity3, TEntity4, TEntity5, TKey>> expression)
        {
            _queryBody.SetSort(expression, SortType.Asc);
            return this;
        }

        public IQueryable<TEntity, TEntity2, TEntity3, TEntity4, TEntity5> OrderByDescending<TKey>(Expression<Func<TEntity, TEntity2, TEntity3, TEntity4, TEntity5, TKey>> expression)
        {
            _queryBody.SetSort(expression, SortType.Desc);
            return this;
        }

        #endregion

        #region ==Where==

        public IQueryable<TEntity, TEntity2, TEntity3, TEntity4, TEntity5> Where(Expression<Func<TEntity, TEntity2, TEntity3, TEntity4, TEntity5, bool>> expression)
        {
            _queryBody.SetWhere(expression);
            return this;
        }

        public IQueryable<TEntity, TEntity2, TEntity3, TEntity4, TEntity5> Where(string whereSql)
        {
            _queryBody.SetWhere(whereSql);
            return this;
        }

        public IQueryable<TEntity, TEntity2, TEntity3, TEntity4, TEntity5> WhereIf(bool condition, Expression<Func<TEntity, TEntity2, TEntity3, TEntity4, TEntity5, bool>> expression)
        {
            if (condition)
            {
                _queryBody.SetWhere(expression);
            }
            return this;
        }

        public IQueryable<TEntity, TEntity2, TEntity3, TEntity4, TEntity5> WhereIf(bool condition, string whereSql)
        {
            if (condition)
            {
                _queryBody.SetWhere(whereSql);
            }

            return this;
        }

        public IQueryable<TEntity, TEntity2, TEntity3, TEntity4, TEntity5> WhereIfElse(bool condition, Expression<Func<TEntity, TEntity2, TEntity3, TEntity4, TEntity5, bool>> ifExpression, Expression<Func<TEntity, TEntity2, TEntity3, TEntity4, TEntity5, bool>> elseExpression)
        {
            _queryBody.SetWhere(condition ? ifExpression : elseExpression);
            return this;
        }

        public IQueryable<TEntity, TEntity2, TEntity3, TEntity4, TEntity5> WhereIfElse(bool condition, string ifWhereSql, string elseWhereSql)
        {
            _queryBody.SetWhere(condition ? ifWhereSql : elseWhereSql);
            return this;
        }

        public IQueryable<TEntity, TEntity2, TEntity3, TEntity4, TEntity5> WhereNotNull(string condition, Expression<Func<TEntity, TEntity2, TEntity3, TEntity4, TEntity5, bool>> expression)
        {
            if (condition.NotNull())
            {
                _queryBody.SetWhere(expression);
            }

            return this;
        }

        public IQueryable<TEntity, TEntity2, TEntity3, TEntity4, TEntity5> WhereNotNull(string condition, Expression<Func<TEntity, TEntity2, TEntity3, TEntity4, TEntity5, bool>> ifExpression, Expression<Func<TEntity, TEntity2, TEntity3, TEntity4, TEntity5, bool>> elseExpression)
        {
            _queryBody.SetWhere(condition.NotNull() ? ifExpression : elseExpression);
            return this;
        }

        public IQueryable<TEntity, TEntity2, TEntity3, TEntity4, TEntity5> WhereNotNull(string condition, string whereSql)
        {
            if (condition.NotNull())
            {
                _queryBody.SetWhere(whereSql);
            }

            return this;
        }

        public IQueryable<TEntity, TEntity2, TEntity3, TEntity4, TEntity5> WhereNotNull(string condition, string ifWhereSql, string elseWhereSql)
        {
            _queryBody.SetWhere(condition.NotNull() ? ifWhereSql : elseWhereSql);
            return this;
        }

        public IQueryable<TEntity, TEntity2, TEntity3, TEntity4, TEntity5> WhereNotNull(object condition, Expression<Func<TEntity, TEntity2, TEntity3, TEntity4, TEntity5, bool>> expression)
        {
            if (condition != null)
            {
                _queryBody.SetWhere(expression);
            }

            return this;
        }

        public IQueryable<TEntity, TEntity2, TEntity3, TEntity4, TEntity5> WhereNotNull(object condition, Expression<Func<TEntity, TEntity2, TEntity3, TEntity4, TEntity5, bool>> ifExpression, Expression<Func<TEntity, TEntity2, TEntity3, TEntity4, TEntity5, bool>> elseExpression)
        {
            _queryBody.SetWhere(condition != null ? ifExpression : elseExpression);
            return this;
        }

        public IQueryable<TEntity, TEntity2, TEntity3, TEntity4, TEntity5> WhereNotNull(object condition, string whereSql)
        {
            if (condition != null)
            {
                _queryBody.SetWhere(whereSql);
            }

            return this;
        }

        public IQueryable<TEntity, TEntity2, TEntity3, TEntity4, TEntity5> WhereNotNull(object condition, string ifWhereSql, string elseWhereSql)
        {
            _queryBody.SetWhere(condition != null ? ifWhereSql : elseWhereSql);
            return this;
        }

        public IQueryable<TEntity, TEntity2, TEntity3, TEntity4, TEntity5> WhereNotEmpty(Guid condition, Expression<Func<TEntity, TEntity2, TEntity3, TEntity4, TEntity5, bool>> expression)
        {
            if (condition != Guid.Empty)
            {
                _queryBody.SetWhere(expression);
            }

            return this;
        }

        public IQueryable<TEntity, TEntity2, TEntity3, TEntity4, TEntity5> WhereNotEmpty(Guid condition, string whereSql)
        {
            if (condition != Guid.Empty)
            {
                _queryBody.SetWhere(whereSql);
            }

            return this;
        }

        public IQueryable<TEntity, TEntity2, TEntity3, TEntity4, TEntity5> WhereNotEmpty(Guid condition, Expression<Func<TEntity, TEntity2, TEntity3, TEntity4, TEntity5, bool>> ifExpression, Expression<Func<TEntity, TEntity2, TEntity3, TEntity4, TEntity5, bool>> elseExpression)
        {
            _queryBody.SetWhere(condition != Guid.Empty ? ifExpression : elseExpression);
            return this;
        }

        public IQueryable<TEntity, TEntity2, TEntity3, TEntity4, TEntity5> WhereNotEmpty(Guid condition, string ifWhereSql, string elseWhereSql)
        {
            _queryBody.SetWhere(condition != Guid.Empty ? ifWhereSql : elseWhereSql);
            return this;
        }

        #endregion

        #region ==SubQuery==

        public IQueryable<TEntity, TEntity2, TEntity3, TEntity4, TEntity5> SubQueryEqual<TKey>(Expression<Func<TEntity, TEntity2, TEntity3, TEntity4, TEntity5, TKey>> key, IQueryable queryable)
        {
            _queryBody.SetWhere(key, "=", queryable);
            return this;
        }

        public IQueryable<TEntity, TEntity2, TEntity3, TEntity4, TEntity5> SubQueryNotEqual<TKey>(Expression<Func<TEntity, TEntity2, TEntity3, TEntity4, TEntity5, TKey>> key, IQueryable queryable)
        {
            _queryBody.SetWhere(key, "<>", queryable);
            return this;
        }

        public IQueryable<TEntity, TEntity2, TEntity3, TEntity4, TEntity5> SubQueryGreaterThan<TKey>(Expression<Func<TEntity, TEntity2, TEntity3, TEntity4, TEntity5, TKey>> key, IQueryable queryable)
        {
            _queryBody.SetWhere(key, ">", queryable);
            return this;
        }

        public IQueryable<TEntity, TEntity2, TEntity3, TEntity4, TEntity5> SubQueryGreaterThanOrEqual<TKey>(Expression<Func<TEntity, TEntity2, TEntity3, TEntity4, TEntity5, TKey>> key, IQueryable queryable)
        {
            _queryBody.SetWhere(key, ">=", queryable);
            return this;
        }

        public IQueryable<TEntity, TEntity2, TEntity3, TEntity4, TEntity5> SubQueryLessThan<TKey>(Expression<Func<TEntity, TEntity2, TEntity3, TEntity4, TEntity5, TKey>> key, IQueryable queryable)
        {
            _queryBody.SetWhere(key, "<", queryable);
            return this;
        }

        public IQueryable<TEntity, TEntity2, TEntity3, TEntity4, TEntity5> SubQueryLessThanOrEqual<TKey>(Expression<Func<TEntity, TEntity2, TEntity3, TEntity4, TEntity5, TKey>> key, IQueryable queryable)
        {
            _queryBody.SetWhere(key, "<=", queryable);
            return this;
        }

        public IQueryable<TEntity, TEntity2, TEntity3, TEntity4, TEntity5> SubQueryIn<TKey>(Expression<Func<TEntity, TEntity2, TEntity3, TEntity4, TEntity5, TKey>> key, IQueryable queryable)
        {
            _queryBody.SetWhere(key, "IN", queryable);
            return this;
        }

        public IQueryable<TEntity, TEntity2, TEntity3, TEntity4, TEntity5> SubQueryNotIn<TKey>(Expression<Func<TEntity, TEntity2, TEntity3, TEntity4, TEntity5, TKey>> key, IQueryable queryable)
        {
            _queryBody.SetWhere(key, "NOT IN", queryable);
            return this;
        }

        #endregion

        #region ==Limit==

        public IQueryable<TEntity, TEntity2, TEntity3, TEntity4, TEntity5> Limit(int skip, int take)
        {
            _queryBody.SetLimit(skip, take);
            return this;
        }

        #endregion

        #region ==Select==

        public IQueryable<TEntity, TEntity2, TEntity3, TEntity4, TEntity5> Select<TResult>(Expression<Func<TEntity, TEntity2, TEntity3, TEntity4, TEntity5, TResult>> expression)
        {
            _queryBody.SetSelect(expression);
            return this;
        }

        public IQueryable<TEntity, TEntity2, TEntity3, TEntity4, TEntity5> Select<TResult>(string sql)
        {
            _queryBody.SetSelect(sql);
            return this;
        }

        public IQueryable<TEntity, TEntity2, TEntity3, TEntity4, TEntity5> SelectExclude<TResult>(Expression<Func<TEntity, TEntity2, TEntity3, TEntity4, TEntity5, TResult>> expression)
        {
            _queryBody.SetSelectExclude(expression);
            return this;
        }

        #endregion

        #region ==Join==

        public IQueryable<TEntity, TEntity2, TEntity3, TEntity4, TEntity5, TEntity6> LeftJoin<TEntity6>(Expression<Func<TEntity, TEntity2, TEntity3, TEntity4, TEntity5, TEntity6, bool>> onExpression, string tableName = null, bool noLock = true) where TEntity6 : IEntity, new()
        {
            return new Queryable<TEntity, TEntity2, TEntity3, TEntity4, TEntity5, TEntity6>(_queryBody, JoinType.Left, onExpression, tableName, noLock);
        }

        public IQueryable<TEntity, TEntity2, TEntity3, TEntity4, TEntity5, TEntity6> InnerJoin<TEntity6>(Expression<Func<TEntity, TEntity2, TEntity3, TEntity4, TEntity5, TEntity6, bool>> onExpression, string tableName = null, bool noLock = true) where TEntity6 : IEntity, new()
        {
            return new Queryable<TEntity, TEntity2, TEntity3, TEntity4, TEntity5, TEntity6>(_queryBody, JoinType.Inner, onExpression, tableName, noLock);
        }

        public IQueryable<TEntity, TEntity2, TEntity3, TEntity4, TEntity5, TEntity6> RightJoin<TEntity6>(Expression<Func<TEntity, TEntity2, TEntity3, TEntity4, TEntity5, TEntity6, bool>> onExpression, string tableName = null, bool noLock = true) where TEntity6 : IEntity, new()
        {
            return new Queryable<TEntity, TEntity2, TEntity3, TEntity4, TEntity5, TEntity6>(_queryBody, JoinType.Right, onExpression, tableName, noLock);
        }

        #endregion

        #region ==List==

        public Task<IList<TEntity>> List()
        {
            return List<TEntity>();
        }

        #endregion

        #region ==Pagination==

        public Task<IList<TEntity>> Pagination()
        {
            return Pagination<TEntity>();
        }

        public Task<IList<TEntity>> Pagination(Paging paging)
        {
            return Pagination<TEntity>(paging);
        }

        #endregion

        #region ==First==

        public Task<TEntity> First()
        {
            return First<TEntity>();
        }

        #endregion

        #region ==NotFilterSoftDeleted==

        public IQueryable<TEntity, TEntity2, TEntity3, TEntity4, TEntity5> NotFilterSoftDeleted()
        {
            _queryBody.FilterDeleted = false;
            return this;
        }

        #endregion

        #region ==NotFilterTenant==

        public IQueryable<TEntity, TEntity2, TEntity3, TEntity4, TEntity5> NotFilterTenant()
        {
            _queryBody.FilterTenant = false;
            return this;
        }

        #endregion

        #region ==Function==

        public Task<TResult> Max<TResult>(Expression<Func<TEntity, TEntity2, TEntity3, TEntity4, TEntity5, TResult>> expression)
        {
            return base.Max<TResult>(expression);
        }

        public Task<TResult> Min<TResult>(Expression<Func<TEntity, TEntity2, TEntity3, TEntity4, TEntity5, TResult>> expression)
        {
            return base.Min<TResult>(expression);
        }

        public Task<TResult> Sum<TResult>(Expression<Func<TEntity, TEntity2, TEntity3, TEntity4, TEntity5, TResult>> expression)
        {
            return base.Sum<TResult>(expression);
        }

        public Task<TResult> Avg<TResult>(Expression<Func<TEntity, TEntity2, TEntity3, TEntity4, TEntity5, TResult>> expression)
        {
            return base.Avg<TResult>(expression);
        }

        #endregion

        #region ==Copy==

        public IQueryable<TEntity, TEntity2, TEntity3, TEntity4, TEntity5> Copy()
        {
            return new Queryable<TEntity, TEntity2, TEntity3, TEntity4, TEntity5>(_queryBody.Copy());
        }

        #endregion
    }
}