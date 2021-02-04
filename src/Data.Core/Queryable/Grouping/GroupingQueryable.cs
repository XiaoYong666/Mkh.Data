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
using Mkh.Data.Abstractions.Queryable.Grouping;
using Mkh.Data.Core.Internal.QueryStructure;
using Mkh.Data.Core.SqlBuilder;

namespace Mkh.Data.Core.Queryable.Grouping
{
    internal abstract class GroupingQueryable : IGroupingQueryable
    {
        protected readonly QueryBody _queryBody;
        protected readonly GroupBySqlBuilder _sqlBuilder;
        protected readonly DbLogger _logger;
        protected readonly IRepository _repository;

        protected GroupingQueryable(QueryableSqlBuilder sqlBuilder, DbLogger logger, Expression expression)
        {
            _queryBody = sqlBuilder.QueryBody.Copy();
            _queryBody.IsGroupBy = true;
            _queryBody.SetGroupBy(expression);
            _sqlBuilder = new GroupBySqlBuilder(_queryBody);
            _logger = logger;
            _repository = _queryBody.Repository;
        }

        #region ==List==

        public Task<IList<dynamic>> ToListDynamic()
        {
            return ToList<dynamic>();
        }

        public async Task<IList<TResult>> ToList<TResult>()
        {
            var sql = _sqlBuilder.BuildListSql(out IQueryParameters parameters);
            _logger.Write("GroupByList", sql);
            return (await _repository.Query<TResult>(sql, parameters.ToDynamicParameters())).ToList();
        }

        public string ToListSql()
        {
            return _sqlBuilder.BuildListSql(out _);
        }

        public string ToListSql(out IQueryParameters parameters)
        {
            return _sqlBuilder.BuildListSql(out parameters);
        }

        public string ToListSql(IQueryParameters parameters)
        {
            return _sqlBuilder.BuildListSql(parameters);
        }

        public string ToListSqlNotUseParameters()
        {
            return _sqlBuilder.BuildListSqlNotUseParameters();
        }

        #endregion

        #region ==Reader==

        public Task<IDataReader> ToReader()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region ==Pagination==


        public Task<IList<dynamic>> ToPaginationDynamic()
        {
            throw new NotImplementedException();
        }

        public Task<IList<dynamic>> ToPaginationDynamic(Paging paging)
        {
            throw new NotImplementedException();
        }

        public Task<IList<TResult>> ToPagination<TResult>()
        {
            throw new NotImplementedException();
        }

        public Task<IList<TResult>> ToPagination<TResult>(Paging paging)
        {
            throw new NotImplementedException();
        }

        public string ToPaginationSql(Paging paging)
        {
            throw new NotImplementedException();
        }

        public string ToPaginationSql(Paging paging, out IQueryParameters parameters)
        {
            throw new NotImplementedException();
        }

        public string ToPaginationSql(Paging paging, IQueryParameters parameters)
        {
            throw new NotImplementedException();
        }

        public string ToPaginationSqlNotUseParameters(Paging paging)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region ==First==


        public Task<dynamic> ToFirstDynamic()
        {
            throw new NotImplementedException();
        }

        public Task<TResult> ToFirst<TResult>()
        {
            throw new NotImplementedException();
        }

        public string ToFirstSql()
        {
            throw new NotImplementedException();
        }

        public string ToFirstSql(out IQueryParameters parameters)
        {
            throw new NotImplementedException();
        }

        public string ToFirstSql(IQueryParameters parameters)
        {
            throw new NotImplementedException();
        }

        public string ToFirstSqlNotUseParameters()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
