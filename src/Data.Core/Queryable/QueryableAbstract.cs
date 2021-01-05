using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Mkh.Data.Abstractions;
using Mkh.Data.Abstractions.Logger;
using Mkh.Data.Abstractions.Pagination;
using Mkh.Data.Abstractions.Queryable;
using Mkh.Data.Core.Queryable.Internal;
using Mkh.Data.Core.SqlBuilder;
using IQueryable = Mkh.Data.Abstractions.Queryable.IQueryable;

namespace Mkh.Data.Core.Queryable
{
    internal class QueryableAbstract : IQueryable
    {
        protected readonly IRepository _repository;
        protected readonly QueryBody _queryBody;
        protected readonly QueryableSqlBuilder _sqlBuilder;
        protected readonly DbLogger _logger;

        public QueryableAbstract(IRepository repository)
        {
            _repository = repository;
            _logger = repository.DbContext.Logger;
            _queryBody = new QueryBody(repository);
            _sqlBuilder = new QueryableSqlBuilder(_queryBody);
        }

        #region ==List==

        public Task<IList<dynamic>> ListDynamic()
        {
            return List<dynamic>();
        }

        public async Task<IList<TResult>> List<TResult>()
        {
            var sql = _sqlBuilder.BuildListSql(out IQueryParameters parameters);
            _logger.Write("List", sql);
            return (await _repository.Query<TResult>(sql, parameters.ToDynamicParameters())).ToList();
        }

        #endregion

        #region ==Reader==

        public Task<IDataReader> Reader()
        {
            var sql = _sqlBuilder.BuildListSql(out IQueryParameters parameters);
            _logger.Write("Reader", sql);
            return _repository.ExecuteReader(sql, parameters.ToDynamicParameters());
        }

        #endregion

        #region ==Pagination==

        public Task<IList<dynamic>> PaginationDynamic()
        {
            return Pagination<dynamic>(null);
        }

        public Task<IList<dynamic>> PaginationDynamic(Paging paging)
        {
            return Pagination<dynamic>(paging);
        }

        public Task<IList<TResult>> Pagination<TResult>()
        {
            return Pagination<TResult>(null);
        }

        public async Task<IList<TResult>> Pagination<TResult>(Paging paging)
        {
            if (paging == null)
                _queryBody.SetLimit(1, 15);
            else
                _queryBody.SetLimit(paging.Skip, paging.Size);

            var sql = _sqlBuilder.BuildPaginationSql(out IQueryParameters parameters);
            _logger.Write("Pagination", sql);

            var task = _repository.Query<TResult>(sql, parameters.ToDynamicParameters());

            if (paging != null && paging.QueryCount)
            {
                paging.TotalCount = await Count();
            }

            return (await task).ToList();
        }

        #endregion

        #region ==First==

        public Task<dynamic> FirstDynamic()
        {
            return First<dynamic>();
        }

        public Task<TResult> First<TResult>()
        {
            var sql = _sqlBuilder.BuildFirstSql(out IQueryParameters parameters);
            _logger.Write("First", sql);
            return _repository.QueryFirstOrDefault<TResult>(sql, parameters.ToDynamicParameters());
        }

        #endregion

        #region ==Count==

        public Task<long> Count()
        {
            var sql = _sqlBuilder.BuildCountSql(out IQueryParameters parameters);
            _logger.Write("Count", sql);
            return _repository.ExecuteScalar<long>(sql, parameters.ToDynamicParameters());
        }

        #endregion

        #region ==Exists==

        public async Task<bool> Exists()
        {
            var sql = _sqlBuilder.BuildExistsSql(out IQueryParameters parameters);
            _logger.Write("Exists", sql);
            return await _repository.ExecuteScalar<int>(sql, parameters.ToDynamicParameters()) > 0;
        }

        #endregion

        #region ==Delete==

        public Task<bool> Delete()
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteWithAffectedNum()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region ==SoftDelete==

        public Task<bool> SoftDelete()
        {
            throw new NotImplementedException();
        }

        public Task<int> SoftDeleteWithAffectedNum()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region ==Function==

        protected Task<TResult> Max<TResult>(LambdaExpression expression)
        {
            return ExecuteFunction<TResult>("Max", expression);
        }

        protected Task<TResult> Min<TResult>(LambdaExpression expression)
        {
            return ExecuteFunction<TResult>("Min", expression);
        }

        protected Task<TResult> Sum<TResult>(LambdaExpression expression)
        {
            return ExecuteFunction<TResult>("Sum", expression);
        }

        protected Task<TResult> Avg<TResult>(LambdaExpression expression)
        {
            return ExecuteFunction<TResult>("Avg", expression);
        }

        private Task<TResult> ExecuteFunction<TResult>(string functionName, LambdaExpression expression)
        {
            _queryBody.SetFunctionSelect(expression, functionName);
            var sql = _sqlBuilder.BuildFunctionSql(out IQueryParameters parameters);
            _logger.Write(functionName, sql);
            return _repository.ExecuteScalar<TResult>(sql, parameters.ToDynamicParameters());
        }

        #endregion

        #region ==SQL==

        public string Sql()
        {
            throw new NotImplementedException();
        }

        public string Sql(out IQueryParameters parameters)
        {
            throw new NotImplementedException();
        }

        public string Sql(IQueryParameters parameters)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
