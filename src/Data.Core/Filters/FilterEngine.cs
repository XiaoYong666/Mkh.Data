using System.Collections.Generic;
using Mkh.Data.Abstractions.Filters;
using Mkh.Data.Abstractions.Filters.Entity;

namespace Mkh.Data.Core.Filters
{
    /// <summary>
    /// 过滤器引擎
    /// </summary>
    internal class FilterEngine : IFilterEngine
    {
        public IList<IEntityAddFilter> EntityAddFilters { get; } = new List<IEntityAddFilter>();

        public IList<IEntityUpdateFilter> EntityUpdateFilters { get; } = new List<IEntityUpdateFilter>();

        public IList<IEntityDeleteFilter> EntityDeleteFilters { get; } = new List<IEntityDeleteFilter>();
    }
}
