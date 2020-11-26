using Mkh.Data.Abstractions.Descriptors;

namespace Mkh.Data.Abstractions.Filters.Entity
{
    /// <summary>
    /// 实体删除过滤器上下文
    /// </summary>
    public interface IEntityDeleteFilterContext
    {
        /// <summary>
        /// 数据库上下文
        /// </summary>
        IDbContext DbContext { get; }

        /// <summary>
        /// 实体描述符
        /// </summary>
        IEntityDescriptor EntityDescriptor { get; }

        /// <summary>
        /// 主键ID
        /// </summary>
        dynamic Id { get; }

        /// <summary>
        /// 新增SQL语句
        /// </summary>
        string Sql { get; }
    }
}
