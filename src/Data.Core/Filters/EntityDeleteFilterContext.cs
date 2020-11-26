using Mkh.Data.Abstractions;
using Mkh.Data.Abstractions.Descriptors;
using Mkh.Data.Abstractions.Filters.Entity;

namespace Mkh.Data.Core.Filters
{
    /// <summary>
    /// 实体删除过滤器上下文
    /// </summary>
    internal class EntityDeleteFilterContext : IEntityDeleteFilterContext
    {
        /// <summary>
        /// 数据库上下文
        /// </summary>
        public IDbContext DbContext { get; }

        /// <summary>
        /// 实体描述符
        /// </summary>
        public IEntityDescriptor EntityDescriptor { get; }

        /// <summary>
        /// 主键ID
        /// </summary>
        public dynamic Id { get; }

        /// <summary>
        /// 新增SQL语句
        /// </summary>
        public string Sql { get; }

        public EntityDeleteFilterContext(IDbContext dbContext, IEntityDescriptor entityDescriptor, dynamic id, string sql)
        {
            DbContext = dbContext;
            EntityDescriptor = entityDescriptor;
            Id = id;
            Sql = sql;
        }
    }
}
