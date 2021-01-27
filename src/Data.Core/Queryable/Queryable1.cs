using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Mkh.Data.Abstractions;
using Mkh.Data.Abstractions.Descriptors;
using Mkh.Data.Abstractions.Entities;
using Mkh.Data.Abstractions.Pagination;
using Mkh.Data.Abstractions.Queryable;
using Mkh.Data.Core.Queryable.Internal;

namespace Mkh.Data.Core.Queryable
{
    internal class Queryable<TEntity> : QueryableAbstract, IQueryable<TEntity> where TEntity : IEntity, new()
    {
        public Queryable(IRepository repository, Expression<Func<TEntity, bool>> expression, bool noLock) : base(repository)
        {
            _queryBody.Joins.Add(new QueryJoin(repository.EntityDescriptor, "T1", JoinType.UnKnown, null, noLock));
            _queryBody.SetWhere(expression);
        }

        #region ==排序==

        public IQueryable<TEntity> OrderBy(string field)
        {
            _queryBody.SetSort(field, SortType.Asc);
            return this;
        }

        public IQueryable<TEntity> OrderByDescending(string field)
        {
            _queryBody.SetSort(field, SortType.Desc);
            return this;
        }

        public IQueryable<TEntity> OrderBy<TKey>(Expression<Func<TEntity, TKey>> expression)
        {
            _queryBody.SetSort(expression, SortType.Asc);
            return this;
        }

        public IQueryable<TEntity> OrderByDescending<TKey>(Expression<Func<TEntity, TKey>> expression)
        {
            _queryBody.SetSort(expression, SortType.Desc);
            return this;
        }

        #endregion

        #region ==过滤条件=

        public IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> expression)
        {
            _queryBody.SetWhere(expression);
            return this;
        }

        public IQueryable<TEntity> Where(string whereSql)
        {
            _queryBody.SetWhere(whereSql);
            return this;
        }

        public IQueryable<TEntity> WhereIf(bool condition, Expression<Func<TEntity, bool>> expression)
        {
            if (condition)
            {
                _queryBody.SetWhere(expression);
            }
            return this;
        }

        public IQueryable<TEntity> WhereIf(bool condition, string whereSql)
        {
            if (condition)
            {
                _queryBody.SetWhere(whereSql);
            }

            return this;
        }

        public IQueryable<TEntity> WhereIfElse(bool condition, Expression<Func<TEntity, bool>> ifExpression, Expression<Func<TEntity, bool>> elseExpression)
        {
            _queryBody.SetWhere(condition ? ifExpression : elseExpression);
            return this;
        }

        public IQueryable<TEntity> WhereIfElse(bool condition, string ifWhereSql, string elseWhereSql)
        {
            _queryBody.SetWhere(condition ? ifWhereSql : elseWhereSql);
            return this;
        }

        public IQueryable<TEntity> WhereNotNull(string condition, Expression<Func<TEntity, bool>> expression)
        {
            if (condition.NotNull())
            {
                _queryBody.SetWhere(expression);
            }

            return this;
        }

        public IQueryable<TEntity> WhereNotNull(string condition, Expression<Func<TEntity, bool>> ifExpression, Expression<Func<TEntity, bool>> elseExpression)
        {
            _queryBody.SetWhere(condition.NotNull() ? ifExpression : elseExpression);
            return this;
        }

        public IQueryable<TEntity> WhereNotNull(string condition, string whereSql)
        {
            if (condition.NotNull())
            {
                _queryBody.SetWhere(whereSql);
            }

            return this;
        }

        public IQueryable<TEntity> WhereNotNull(string condition, string ifWhereSql, string elseWhereSql)
        {
            _queryBody.SetWhere(condition.NotNull() ? ifWhereSql : elseWhereSql);
            return this;
        }

        public IQueryable<TEntity> WhereNotNull(object condition, Expression<Func<TEntity, bool>> expression)
        {
            if (condition != null)
            {
                _queryBody.SetWhere(expression);
            }

            return this;
        }

        public IQueryable<TEntity> WhereNotNull(object condition, Expression<Func<TEntity, bool>> ifExpression, Expression<Func<TEntity, bool>> elseExpression)
        {
            _queryBody.SetWhere(condition != null ? ifExpression : elseExpression);
            return this;
        }

        public IQueryable<TEntity> WhereNotNull(object condition, string whereSql)
        {
            if (condition != null)
            {
                _queryBody.SetWhere(whereSql);
            }

            return this;
        }

        public IQueryable<TEntity> WhereNotNull(object condition, string ifWhereSql, string elseWhereSql)
        {
            _queryBody.SetWhere(condition != null ? ifWhereSql : elseWhereSql);
            return this;
        }

        public IQueryable<TEntity> WhereNotEmpty(Guid condition, Expression<Func<TEntity, bool>> expression)
        {
            if (condition != Guid.Empty)
            {
                _queryBody.SetWhere(expression);
            }

            return this;
        }

        public IQueryable<TEntity> WhereNotEmpty(Guid condition, string whereSql)
        {
            if (condition != Guid.Empty)
            {
                _queryBody.SetWhere(whereSql);
            }

            return this;
        }

        public IQueryable<TEntity> WhereNotEmpty(Guid condition, Expression<Func<TEntity, bool>> ifExpression, Expression<Func<TEntity, bool>> elseExpression)
        {
            _queryBody.SetWhere(condition != Guid.Empty ? ifExpression : elseExpression);
            return this;
        }

        public IQueryable<TEntity> WhereNotEmpty(Guid condition, string ifWhereSql, string elseWhereSql)
        {
            _queryBody.SetWhere(condition != Guid.Empty ? ifWhereSql : elseWhereSql);
            return this;
        }

        #endregion

        #region ==子查询==

        public IQueryable<TEntity> SubQueryEqual<TKey>(Expression<Func<TEntity, TKey>> key, IQueryable queryable)
        {
            _queryBody.SetWhere(key, "=", queryable);
            return this;
        }

        public IQueryable<TEntity> SubQueryNotEqual<TKey>(Expression<Func<TEntity, TKey>> key, IQueryable queryable)
        {
            _queryBody.SetWhere(key, "<>", queryable);
            return this;
        }

        public IQueryable<TEntity> SubQueryGreaterThan<TKey>(Expression<Func<TEntity, TKey>> key, IQueryable queryable)
        {
            _queryBody.SetWhere(key, ">", queryable);
            return this;
        }

        public IQueryable<TEntity> SubQueryGreaterThanOrEqual<TKey>(Expression<Func<TEntity, TKey>> key, IQueryable queryable)
        {
            _queryBody.SetWhere(key, ">=", queryable);
            return this;
        }

        public IQueryable<TEntity> SubQueryLessThan<TKey>(Expression<Func<TEntity, TKey>> key, IQueryable queryable)
        {
            _queryBody.SetWhere(key, "<", queryable);
            return this;
        }

        public IQueryable<TEntity> SubQueryLessThanOrEqual<TKey>(Expression<Func<TEntity, TKey>> key, IQueryable queryable)
        {
            _queryBody.SetWhere(key, "<=", queryable);
            return this;
        }

        public IQueryable<TEntity> SubQueryIn<TKey>(Expression<Func<TEntity, TKey>> key, IQueryable queryable)
        {
            _queryBody.SetWhere(key, "IN", queryable);
            return this;
        }

        public IQueryable<TEntity> SubQueryNotIn<TKey>(Expression<Func<TEntity, TKey>> key, IQueryable queryable)
        {
            _queryBody.SetWhere(key, "NOT IN", queryable);
            return this;
        }

        #endregion

        #region ==查询列==

        public IQueryable<TEntity> Select<TResult>(Expression<Func<TEntity, TResult>> expression)
        {
            _queryBody.SetSelect(expression);
            return this;
        }

        public IQueryable<TEntity> Select<TResult>(string sql)
        {
            _queryBody.SetSelect(sql);
            return this;
        }

        public IQueryable<TEntity> SelectExclude<TResult>(Expression<Func<TEntity, TResult>> expression)
        {
            _queryBody.SetSelectExclude(expression);
            return this;
        }

        #endregion

        #region ==表连接==

        public IQueryable<TEntity, TEntity2> LeftJoin<TEntity2>(Expression<Func<TEntity, TEntity2, bool>> onExpression, string tableName = null, bool noLock = true) where TEntity2 : IEntity, new()
        {
            throw new NotImplementedException();
        }

        public IQueryable<TEntity, TEntity2> InnerJoin<TEntity2>(Expression<Func<TEntity, TEntity2, bool>> onExpression, string tableName = null, bool noLock = true) where TEntity2 : IEntity, new()
        {
            throw new NotImplementedException();
        }

        public IQueryable<TEntity, TEntity2> RightJoin<TEntity2>(Expression<Func<TEntity, TEntity2, bool>> onExpression, string tableName = null, bool noLock = true) where TEntity2 : IEntity, new()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region ==不过滤软删除数据==

        public IQueryable<TEntity> NotFilterSoftDeleted()
        {
            _queryBody.FilterDeleted = false;
            return this;
        }

        #endregion

        #region ==不过滤租户==

        public IQueryable<TEntity> NotFilterTenant()
        {
            _queryBody.FilterTenant = false;
            return this;
        }

        #endregion

        #region ==更新==

        public async Task<bool> Update(Expression<Func<TEntity, TEntity>> expression)
        {
            return await UpdateWithAffectedRowsNumber(expression) > 0;
        }

        public Task<int> UpdateWithAffectedRowsNumber(Expression<Func<TEntity, TEntity>> expression)
        {
            _queryBody.SetUpdate(expression);

            var sql = _sqlBuilder.BuildUpdateSql(out IQueryParameters parameters);
            _logger.Write("Update", sql);
            return _repository.Execute(sql, parameters.ToDynamicParameters());
        }

        public async Task<bool> Update(string updateSql, Dictionary<string, object> parameters = null)
        {
            return await UpdateWithAffectedRowsNumber(updateSql, parameters) > 0;
        }

        public Task<int> UpdateWithAffectedRowsNumber(string updateSql, Dictionary<string, object> parameters = null)
        {
            _queryBody.SetUpdate(updateSql);
            var sql = _sqlBuilder.BuildUpdateSql(out IQueryParameters p_);
            _logger.Write("Update", sql);
            var dynamicParameters = p_.ToDynamicParameters();
            if (parameters != null)
            {
                foreach (var p in parameters)
                {
                    dynamicParameters.Add(p.Key, p.Value);
                }
            }

            return _repository.Execute(sql, dynamicParameters);
        }

        public string UpdateSql(Expression<Func<TEntity, TEntity>> expression)
        {
            _queryBody.SetUpdate(expression);
            return _sqlBuilder.BuildUpdateSql(out _);
        }

        public string UpdateSql(Expression<Func<TEntity, TEntity>> expression, out IQueryParameters parameters)
        {
            _queryBody.SetUpdate(expression);
            return _sqlBuilder.BuildUpdateSql(out parameters);
        }

        public string UpdateSql(Expression<Func<TEntity, TEntity>> expression, IQueryParameters parameters)
        {
            _queryBody.SetUpdate(expression);
            return _sqlBuilder.BuildUpdateSql(parameters);
        }

        public string UpdateNotUseParameters(Expression<Func<TEntity, TEntity>> expression)
        {
            _queryBody.SetUpdate(expression);
            return _sqlBuilder.BuildUpdateSqlNotUseParameters();
        }

        public string UpdateSql(string updateSql)
        {
            _queryBody.SetUpdate(updateSql);
            return _sqlBuilder.BuildUpdateSql(out _);
        }

        public string UpdateSql(string updateSql, out IQueryParameters parameters)
        {
            _queryBody.SetUpdate(updateSql);
            return _sqlBuilder.BuildUpdateSql(out parameters);
        }

        public string UpdateSql(string updateSql, IQueryParameters parameters)
        {
            _queryBody.SetUpdate(updateSql);
            return _sqlBuilder.BuildUpdateSql(parameters);
        }

        public string UpdateNotUseParameters(string updateSql)
        {
            _queryBody.SetUpdate(updateSql);
            return _sqlBuilder.BuildUpdateSqlNotUseParameters();
        }

        #endregion

        #region ==删除==

        public Task Delete()
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteWithAffectedRowsNumber()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region ==软删除==

        public Task<bool> SoftDelete()
        {
            throw new NotImplementedException();
        }

        public Task<int> SoftDeleteWithAffectedRowsNumber()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region ==列表==

        public Task<IList<TEntity>> List()
        {
            return List<TEntity>();
        }

        #endregion

        #region ==分页==

        public Task<IList<TEntity>> Pagination()
        {
            return Pagination<TEntity>();
        }

        public Task<IList<TEntity>> Pagination(Paging paging)
        {
            return Pagination<TEntity>(paging);
        }

        #endregion

        #region ==查询第一条==

        public Task<TEntity> First()
        {
            return First<TEntity>();
        }

        #endregion

        #region ==Function==

        public Task<TResult> Max<TResult>(Expression<Func<TEntity, TResult>> expression)
        {
            return base.Max<TResult>(expression);
        }

        public Task<TResult> Min<TResult>(Expression<Func<TEntity, TResult>> expression)
        {
            return base.Min<TResult>(expression);
        }

        public Task<TResult> Sum<TResult>(Expression<Func<TEntity, TResult>> expression)
        {
            return base.Sum<TResult>(expression);
        }

        public Task<TResult> Avg<TResult>(Expression<Func<TEntity, TResult>> expression)
        {
            return base.Avg<TResult>(expression);
        }

        #endregion

        #region ==复制==

        public IQueryable<TEntity> Copy()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
