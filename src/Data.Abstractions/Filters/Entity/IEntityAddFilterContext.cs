using Mkh.Data.Abstractions.Descriptors;
using Mkh.Data.Abstractions.Entities;

namespace Mkh.Data.Abstractions.Filters.Entity
{
    /// <summary>
    /// 实体新增过滤器上下文
    /// </summary>
    public interface IEntityAddFilterContext
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
        /// 实体信息
        /// </summary>
        IEntity Entity { get; }

        /// <summary>
        /// 新增SQL语句
        /// </summary>
        string Sql { get; }
    }
}
