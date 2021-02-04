﻿using System;
using System.Linq.Expressions;
using Mkh.Data.Abstractions.Entities;
using Mkh.Data.Abstractions.Logger;
using Mkh.Data.Abstractions.Pagination;
using Mkh.Data.Abstractions.Queryable.Grouping;
using Mkh.Data.Core.SqlBuilder;

namespace Mkh.Data.Core.Queryable.Grouping
{
    internal class GroupingQueryable<TKey, TEntity> : GroupingQueryable, IGroupingQueryable<TKey, TEntity> where TEntity : IEntity
    {
        public GroupingQueryable(QueryableSqlBuilder sqlBuilder, DbLogger logger, Expression expression) : base(sqlBuilder, logger, expression)
        {
        }

        public IGroupingQueryable<TKey, TEntity> Having(Expression<Func<IGrouping<TKey, TEntity>, bool>> expression)
        {
            _queryBody.SetHaving(expression);
            return this;
        }

        public IGroupingQueryable<TKey, TEntity> Having(string havingSql)
        {
            _queryBody.SetHaving(havingSql);
            return this;
        }

        public IGroupingQueryable<TKey, TEntity> OrderBy(string field)
        {
            _queryBody.SetSort(field, SortType.Asc);
            return this;
        }

        public IGroupingQueryable<TKey, TEntity> OrderByDescending(string field)
        {
            _queryBody.SetSort(field, SortType.Desc);
            return this;
        }

        public IGroupingQueryable<TKey, TEntity> OrderBy<TResult>(Expression<Func<IGrouping<TKey, TEntity>, TResult>> expression)
        {
            _queryBody.SetSort(expression, SortType.Asc);
            return this;
        }

        public IGroupingQueryable<TKey, TEntity> OrderByDescending<TResult>(Expression<Func<IGrouping<TKey, TEntity>, TResult>> expression)
        {
            _queryBody.SetSort(expression, SortType.Desc);
            return this;
        }

        public IGroupingQueryable<TKey, TEntity> Select<TResult>(Expression<Func<IGrouping<TKey, TEntity>, TResult>> expression)
        {
            _queryBody.SetSelect(expression);
            return this;
        }
    }
}