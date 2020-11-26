namespace Mkh.Data.Abstractions.Filters.Entity
{
    /// <summary>
    /// 实体删除过滤器
    /// </summary>
    public interface IEntityDeleteFilter : IFilter
    {
        /// <summary>
        /// 删除前调用
        /// </summary>
        /// <param name="context">上下文</param>
        /// <returns></returns>
        void OnBefore(IEntityDeleteFilterContext context);

        /// <summary>
        /// 删除后调用
        /// </summary>
        /// <param name="context">上下文</param>
        /// <returns></returns>
        void OnAfter(IEntityDeleteFilterContext context);
    }

    /// <summary>
    /// 实体删除过滤器
    /// </summary>
    public class EntityDeleteFilter : IEntityDeleteFilter
    {
        public virtual void OnBefore(IEntityDeleteFilterContext context)
        {
        }

        public virtual void OnAfter(IEntityDeleteFilterContext context)
        {
        }
    }
}
