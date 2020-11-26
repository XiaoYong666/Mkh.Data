using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Mkh.Data.Core.Filters;

namespace Mkh.Data.Core.Repository
{
    /// <summary>
    /// 实体更新
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public abstract partial class RepositoryAbstract<TEntity>
    {
        public Task<bool> Update(TEntity entity)
        {
            return UpdateAsync(entity, null);
        }

        protected async Task<bool> UpdateAsync(TEntity entity, string tableName)
        {
            var sql = UpdateBefore(entity, tableName);
            var result = await Execute(sql, entity) > 0;
            UpdateAfter(entity, sql);
            return result;
        }

        private string UpdateBefore(TEntity entity, string tableName)
        {
            Check.NotNull(entity, nameof(entity));

            if (EntityDescriptor.PrimaryKey.IsNo)
                throw new ArgumentException("没有主键的实体对象无法使用该方法", nameof(entity));

            var sql = _sql.GetUpdateSingle(tableName);

            var filters = DbContext.FilterEngine.EntityUpdateFilters;
            if (filters.NotNullAndEmpty())
            {
                var context = new EntityUpdateFilterContext(DbContext, EntityDescriptor, entity, sql);
                foreach (var filter in filters)
                {
                    filter.OnBefore(context);
                }
            }

            _logger?.LogDebug("Update:{@sql}", sql);

            return sql;
        }

        /// <summary>
        /// 新增后
        /// </summary>
        private void UpdateAfter(TEntity entity, string sql)
        {
            var filters = DbContext.FilterEngine.EntityUpdateFilters;
            if (filters.NotNullAndEmpty())
            {
                var context = new EntityUpdateFilterContext(DbContext, EntityDescriptor, entity, sql);
                foreach (var filter in filters)
                {
                    filter.OnAfter(context);
                }
            }
        }
    }
}
