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

        public string ListSql()
        {
            return _sqlBuilder.BuildListSql(out _);
        }

        public string ListSql(out IQueryParameters parameters)
        {
            return _sqlBuilder.BuildListSql(out parameters);
        }

        public string ListSql(IQueryParameters parameters)
        {
            return _sqlBuilder.BuildListSql(parameters);
        }

        public string ListSqlNotUseParameters()
        {
            return _sqlBuilder.BuildListSqlNotUseParameters();
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

        public string PaginationSql(Paging paging)
        {
            if (paging == null)
                _queryBody.SetLimit(1, 15);
            else
                _queryBody.SetLimit(paging.Skip, paging.Size);

            return _sqlBuilder.BuildPaginationSql(out _);
        }

        public string PaginationSql(Paging paging, out IQueryParameters parameters)
        {
            if (paging == null)
                _queryBody.SetLimit(1, 15);
            else
                _queryBody.SetLimit(paging.Skip, paging.Size);

            return _sqlBuilder.BuildPaginationSql(out parameters);
        }

        public string PaginationSql(Paging paging, IQueryParameters parameters)
        {
            if (paging == null)
                _queryBody.SetLimit(1, 15);
            else
                _queryBody.SetLimit(paging.Skip, paging.Size);

            return _sqlBuilder.BuildPaginationSql(parameters);
        }

        public string PaginationSqlNotUseParameters(Paging paging)
        {
            if (paging == null)
                _queryBody.SetLimit(1, 15);
            else
                _queryBody.SetLimit(paging.Skip, paging.Size);

            return _sqlBuilder.BuildPaginationSqlNotUseParameters();
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

        public string FirstSql()
        {
            return _sqlBuilder.BuildFirstSql(out _);
        }

        public string FirstSql(out IQueryParameters parameters)
        {
            return _sqlBuilder.BuildFirstSql(out parameters);
        }

        public string FirstSql(IQueryParameters parameters)
        {
            return _sqlBuilder.BuildFirstSql(parameters);
        }

        public string FirstSqlNotUseParameters()
        {
            return _sqlBuilder.BuildFirstSqlNotUserParameters();
        }

        #endregion

        #region ==Count==

        public Task<long> Count()
        {
            var sql = _sqlBuilder.BuildCountSql(out IQueryParameters parameters);
            _logger.Write("Count", sql);
            return _repository.ExecuteScalar<long>(sql, parameters.ToDynamicParameters());
        }

        public string CountSql()
        {
            return _sqlBuilder.BuildCountSql(out _);
        }

        public string CountSql(out IQueryParameters parameters)
        {
            return _sqlBuilder.BuildCountSql(out parameters);
        }

        public string CountSql(IQueryParameters parameters)
        {
            return _sqlBuilder.BuildCountSql(parameters);
        }

        public string CountSqlNotUseParameters()
        {
            return _sqlBuilder.BuildCountSqlNotUseParameters();
        }

        #endregion

        #region ==Exists==

        public async Task<bool> Exists()
        {
            var sql = _sqlBuilder.BuildExistsSql(out IQueryParameters parameters);
            _logger.Write("Exists", sql);
            return await _repository.ExecuteScalar<int>(sql, parameters.ToDynamicParameters()) > 0;
        }

        public string ExistsSql()
        {
            return _sqlBuilder.BuildExistsSql(out _);
        }

        public string ExistsSql(out IQueryParameters parameters)
        {
            return _sqlBuilder.BuildExistsSql(out parameters);
        }

        public string ExistsSql(IQueryParameters parameters)
        {
            return _sqlBuilder.BuildExistsSql(parameters);
        }

        public string ExistsSqlNotUseParameters()
        {
            return _sqlBuilder.BuildExistsSqlNotUseParameters();
        }

        #endregion

        #region ==Max==

        protected Task<TResult> Max<TResult>(LambdaExpression expression)
        {
            return ExecuteFunction<TResult>("Max", expression);
        }

        public string MaxSql(LambdaExpression expression)
        {
            _queryBody.SetFunctionSelect(expression, "Max");
            return _sqlBuilder.BuildFunctionSql(out _);
        }

        public string MaxSql(LambdaExpression expression, out IQueryParameters parameters)
        {
            _queryBody.SetFunctionSelect(expression, "Max");
            return _sqlBuilder.BuildFunctionSql(out parameters);
        }

        public string MaxSql(LambdaExpression expression, IQueryParameters parameters)
        {
            _queryBody.SetFunctionSelect(expression, "Max");
            return _sqlBuilder.BuildFunctionSql(parameters);
        }

        public string MaxSqlNotUseParameters(LambdaExpression expression)
        {
            _queryBody.SetFunctionSelect(expression, "Max");
            return _sqlBuilder.BuildFunctionSqlNotUseParameters();
        }

        #endregion

        #region ==Min==
        
        protected Task<TResult> Min<TResult>(LambdaExpression expression)
        {
            return ExecuteFunction<TResult>("Min", expression);
        }

        public string MinSql(LambdaExpression expression)
        {
            _queryBody.SetFunctionSelect(expression, "Min");
            return _sqlBuilder.BuildFunctionSql(out _);
        }

        public string MinSql(LambdaExpression expression, out IQueryParameters parameters)
        {
            _queryBody.SetFunctionSelect(expression, "Min");
            return _sqlBuilder.BuildFunctionSql(out parameters);
        }

        public string MinSql(LambdaExpression expression, IQueryParameters parameters)
        {
            _queryBody.SetFunctionSelect(expression, "Min");
            return _sqlBuilder.BuildFunctionSql(parameters);
        }

        public string MinSqlNotUseParameters(LambdaExpression expression)
        {
            _queryBody.SetFunctionSelect(expression, "Min");
            return _sqlBuilder.BuildFunctionSqlNotUseParameters();
        }

        #endregion

        #region ==Sum==

        protected Task<TResult> Sum<TResult>(LambdaExpression expression)
        {
            return ExecuteFunction<TResult>("Sum", expression);
        }

        public string SumSql(LambdaExpression expression)
        {
            _queryBody.SetFunctionSelect(expression, "Sum");
            return _sqlBuilder.BuildFunctionSql(out _);
        }

        public string SumSql(LambdaExpression expression, out IQueryParameters parameters)
        {
            _queryBody.SetFunctionSelect(expression, "Sum");
            return _sqlBuilder.BuildFunctionSql(out parameters);
        }

        public string SumSql(LambdaExpression expression, IQueryParameters parameters)
        {
            _queryBody.SetFunctionSelect(expression, "Sum");
            return _sqlBuilder.BuildFunctionSql(parameters);
        }

        public string SumSqlNotUseParameters(LambdaExpression expression)
        {
            _queryBody.SetFunctionSelect(expression, "Sum");
            return _sqlBuilder.BuildFunctionSqlNotUseParameters();
        }

        #endregion

        #region ==Avg==

        protected Task<TResult> Avg<TResult>(LambdaExpression expression)
        {
            return ExecuteFunction<TResult>("Avg", expression);
        }

        public string AvgSql(LambdaExpression expression)
        {
            _queryBody.SetFunctionSelect(expression, "Avg");
            return _sqlBuilder.BuildFunctionSql(out _);
        }

        public string AvgSql(LambdaExpression expression, out IQueryParameters parameters)
        {
            _queryBody.SetFunctionSelect(expression, "Avg");
            return _sqlBuilder.BuildFunctionSql(out parameters);
        }

        public string AvgSql(LambdaExpression expression, IQueryParameters parameters)
        {
            _queryBody.SetFunctionSelect(expression, "Avg");
            return _sqlBuilder.BuildFunctionSql(parameters);
        }

        public string AvgSqlNotUseParameters(LambdaExpression expression)
        {
            _queryBody.SetFunctionSelect(expression, "Avg");
            return _sqlBuilder.BuildFunctionSqlNotUseParameters();
        }

        #endregion

        #region ==Function==

        private Task<TResult> ExecuteFunction<TResult>(string functionName, LambdaExpression expression)
        {
            _queryBody.SetFunctionSelect(expression, functionName);
            var sql = _sqlBuilder.BuildFunctionSql(out IQueryParameters parameters);
            _logger.Write(functionName, sql);
            return _repository.ExecuteScalar<TResult>(sql, parameters.ToDynamicParameters());
        }

        #endregion
    }
}
