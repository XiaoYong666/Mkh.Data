using System.Collections.Generic;
using Mkh.Data.Abstractions.Filters.Entity;

namespace Mkh.Data.Abstractions.Filters
{
    /// <summary>
    /// 过滤器引擎
    /// </summary>
    public interface IFilterEngine
    {
        /// <summary>
        /// 实体新增过滤器集合
        /// </summary>
        IList<IEntityAddFilter> EntityAddFilters { get; }

        /// <summary>
        /// 实体更新过滤器集合
        /// </summary>
        IList<IEntityUpdateFilter> EntityUpdateFilters { get; }

        /// <summary>
        /// 实体删除过滤器集合
        /// </summary>
        IList<IEntityDeleteFilter> EntityDeleteFilters { get; }
    }
}
