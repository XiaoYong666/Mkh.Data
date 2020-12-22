using System.Threading.Tasks;

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
            var sql = _sql.GetDeleteSingle(tableName);
            var result = await Execute(sql, dynParams) > 0;
            return result;
        }
    }
}
