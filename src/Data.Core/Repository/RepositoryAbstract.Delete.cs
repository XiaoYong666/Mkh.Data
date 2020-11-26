using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Mkh.Data.Core.Filters;

namespace Mkh.Data.Core.Repository
{
    /// <summary>
    /// 删除
    /// </summary>
    public abstract partial class RepositoryAbstract<TEntity>
    {
        public Task<bool> Delete(dynamic id)
        {
            return Delete(id, null);
        }

        /// <summary>
        /// 删除实体，自定义表名称
        /// </summary>
        /// <param name="id">实体ID</param>
        /// <param name="tableName">自定义表名称</param>
        /// <returns></returns>
        protected async Task<bool> Delete(dynamic id, string tableName)
        {
            var dynParams = GetIdParameter(id);
            var sql = DeleteBefore(id, tableName);
            var result = await Execute(sql, dynParams) > 0;
            DeleteAfter(id, sql);
            return result;
        }

        private string DeleteBefore(dynamic id, string tableName)
        {
            var sql = _sql.GetDeleteSingle(tableName);
            var filters = DbContext.FilterEngine.EntityDeleteFilters;
            if (filters.NotNullAndEmpty())
            {
                var context = new EntityDeleteFilterContext(DbContext, EntityDescriptor, id, sql);
                foreach (var filter in filters)
                {
                    filter.OnBefore(context);
                }
            }

            _logger?.LogDebug("Delete:{@sql}", sql);

            return sql;
        }

        private void DeleteAfter(dynamic id, string sql)
        {
            var filters = DbContext.FilterEngine.EntityDeleteFilters;
            if (filters.NotNullAndEmpty())
            {
                var context = new EntityDeleteFilterContext(DbContext, EntityDescriptor, id, sql);
                foreach (var filter in filters)
                {
                    filter.OnAfter(context);
                }
            }
        }
    }
}
