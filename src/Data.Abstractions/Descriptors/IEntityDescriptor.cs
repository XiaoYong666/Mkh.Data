using System;
using System.Collections.Generic;
using Mkh.Data.Abstractions.Filters;

namespace Mkh.Data.Abstractions.Descriptors
{
    /// <summary>
    /// 实体信息描述符
    /// </summary>
    public interface IEntityDescriptor
    {
        /// <summary>
        /// 数据库上下文
        /// </summary>
        IDbContext DbContext { get; }

        /// <summary>
        /// 表名称
        /// </summary>
        string TableName { get; }

        /// <summary>
        /// 自动创建表
        /// </summary>
        bool AutoCreate { get; set; }

        /// <summary>
        /// 实体类型
        /// </summary>
        Type EntityType { get; }

        /// <summary>
        /// 主键列
        /// </summary>
        IPrimaryKeyDescriptor PrimaryKey { get; }

        /// <summary>
        /// 列集合
        /// </summary>
        IList<IColumnDescriptor> Columns { get; }

        /// <summary>
        /// SQL语句描述符
        /// </summary>
        IEntitySqlDescriptor SqlDescriptor { get; }

        /// <summary>
        /// 操作过滤器引擎
        /// </summary>
        IFilterEngine FilterEngine { get; }

        /// <summary>
        /// 是否使用实体基类
        /// </summary>
        bool IsEntityBase { get; }

        /// <summary>
        /// 是否使用租户
        /// </summary>
        bool IsTenant { get; }

        /// <summary>
        /// 是否使用软删除
        /// </summary>
        bool IsSoftDelete { get; }
    }
}
