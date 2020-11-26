namespace Mkh.Data.Abstractions.Filters.Entity
{
    /// <summary>
    /// 实体新增过滤器
    /// </summary>
    public interface IEntityAddFilter : IFilter
    {
        /// <summary>
        /// 新增前调用
        /// </summary>
        /// <param name="context">上下文</param>
        /// <returns></returns>
        void OnBefore(IEntityAddFilterContext context);

        /// <summary>
        /// 新增后调用
        /// </summary>
        /// <param name="context">上下文</param>
        /// <returns></returns>
        void OnAfter(IEntityAddFilterContext context);
    }

    /// <summary>
    /// 实体新增过滤器
    /// </summary>
    public class EntityAddFilter : IEntityAddFilter
    {
        public virtual void OnBefore(IEntityAddFilterContext context)
        {
        }

        public virtual void OnAfter(IEntityAddFilterContext context)
        {
        }
    }
}
