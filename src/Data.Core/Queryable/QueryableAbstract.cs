using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Mkh.Data.Abstractions;
using Mkh.Data.Abstractions.Pagination;
using Mkh.Data.Abstractions.Queryable;
using Mkh.Data.Core.Queryable.Internal;
using Mkh.Data.Core.SqlBuilder;

namespace Mkh.Data.Core.Queryable
{
    internal class QueryableAbstract : IQueryable
    {
        protected readonly IRepository _repository;
        protected readonly QueryBody _queryBody;
        protected readonly QueryableSqlBuilder _sqlBuilder;

        public QueryableAbstract(IRepository repository)
        {
            _repository = repository;
            _queryBody = new QueryBody(repository);
            _sqlBuilder = new QueryableSqlBuilder(_queryBody);
        }

        #region ==List==

        public Task<IEnumerable<dynamic>> ListDynamic()
        {
            var sql = _sqlBuilder.BuildListSql(out IQueryParameters parameters);
            return _repository.Query(sql, parameters.ToDynamicParameters());
        }

        public Task<IEnumerable<TResult>> List<TResult>()
        {
            var sql = _sqlBuilder.BuildListSql(out IQueryParameters parameters);
            return _repository.Query<TResult>(sql, parameters.ToDynamicParameters());
        }

        #endregion

        #region ==Reader==

        public Task<IDataReader> Reader()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region ==Pagination==

        public Task<IEnumerable<dynamic>> PaginationDynamic()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<dynamic>> PaginationDynamic(Paging paging)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TResult>> Pagination<TResult>()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TResult>> Pagination<TResult>(Paging paging)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region ==First==

        public Task<dynamic> FirstDynamic()
        {
            throw new NotImplementedException();
        }

        public Task<TResult> First<TResult>()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region ==Count==

        public Task<long> Count()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region ==Exists==

        public Task<bool> Exists()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region ==Function==

        public Task<TResult> Max<TResult>()
        {
            throw new NotImplementedException();
        }

        public Task<TResult> Min<TResult>()
        {
            throw new NotImplementedException();
        }

        public Task<TResult> Sum<TResult>()
        {
            throw new NotImplementedException();
        }

        public Task<TResult> Avg<TResult>()
        {
            throw new NotImplementedException();
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
