using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Mkh.Data.Core.Filters;

namespace Mkh.Data.Core.Repository
{
    /// <summary>
    /// 新增
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public abstract partial class RepositoryAbstract<TEntity>
    {
        public Task Add(TEntity entity)
        {
            return Add(entity, null);
        }

        /// <summary>
        /// 新增，自定义表名称
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        protected async Task Add(TEntity entity, string tableName)
        {
            var sql = _sql.GetAdd(tableName);
            sql = AddBefore(entity, sql);
            _logger?.LogError("Add:{@sql}", sql);

            var primaryKey = EntityDescriptor.PrimaryKey;
            if (primaryKey.IsInt)
            {
                //自增主键
                sql += _adapter.IdentitySql;
                var id = await ExecuteScalar<int>(sql, entity);
                if (id > 0)
                {
                    primaryKey.PropertyInfo.SetValue(entity, id);

                    _logger?.LogDebug("NewID:{@id}", id);
                    AddAfter(entity, sql);
                    return;
                }
            }

            if (primaryKey.IsLong)
            {
                //自增主键
                sql += _adapter.IdentitySql;
                var id = await ExecuteScalar<long>(sql, entity);
                if (id > 0)
                {
                    primaryKey.PropertyInfo.SetValue(entity, id);

                    _logger?.LogDebug("NewID:{@id}", id);
                    AddAfter(entity, sql);

                    return;
                }
            }

            if (primaryKey.IsGuid)
            {
                var id = (Guid)primaryKey.PropertyInfo.GetValue(entity);
                if (id == Guid.Empty)
                    primaryKey.PropertyInfo.SetValue(entity, Guid.NewGuid());

                _logger?.LogDebug("NewID:{@id}", id);

                if (await Execute(sql, entity) > 0)
                {
                    AddAfter(entity, sql);
                    return;
                }
            }

            if (await Execute(sql, entity) > 0)
            {
                AddAfter(entity, sql);
                return;
            }

            throw new ApplicationException("实体新增失败");
        }

        /// <summary>
        /// 新增前
        /// </summary>
        private string AddBefore(TEntity entity, string sql)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity), "entity is null");

            var filters = DbContext.FilterEngine.EntityAddFilters;
            if (filters.NotNullAndEmpty())
            {
                var context = new EntityAddFilterContext(DbContext, EntityDescriptor, entity, sql);
                foreach (var filter in filters)
                {
                    filter.OnBefore(context);
                }

                return context.Sql;
            }



            return sql;
        }

        /// <summary>
        /// 新增后
        /// </summary>
        private void AddAfter(TEntity entity, string sql)
        {
            var filters = DbContext.FilterEngine.EntityAddFilters;
            if (filters.NotNullAndEmpty())
            {
                var context = new EntityAddFilterContext(DbContext, EntityDescriptor, entity, sql);
                foreach (var filter in filters)
                {
                    filter.OnAfter(context);
                }
            }
        }
    }
}
