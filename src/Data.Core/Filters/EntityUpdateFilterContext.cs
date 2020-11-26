using Mkh.Data.Abstractions;
using Mkh.Data.Abstractions.Descriptors;
using Mkh.Data.Abstractions.Entities;
using Mkh.Data.Abstractions.Filters.Entity;

namespace Mkh.Data.Core.Filters
{
    /// <summary>
    /// 实体更新过滤器上下文
    /// </summary>
    internal class EntityUpdateFilterContext : IEntityUpdateFilterContext
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
        /// 实体信息
        /// </summary>
        public IEntity Entity { get; }

        /// <summary>
        /// 新增SQL语句
        /// </summary>
        public string Sql { get; }

        public EntityUpdateFilterContext(IDbContext dbContext, IEntityDescriptor entityDescriptor, IEntity entity, string sql)
        {
            DbContext = dbContext;
            EntityDescriptor = entityDescriptor;
            Entity = entity;
            Sql = sql;
        }
    }
}
