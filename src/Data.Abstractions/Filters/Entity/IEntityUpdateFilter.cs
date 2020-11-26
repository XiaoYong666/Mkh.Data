namespace Mkh.Data.Abstractions.Filters.Entity
{
    /// <summary>
    /// 实体更新过滤器
    /// </summary>
    public interface IEntityUpdateFilter : IFilter
    {
        /// <summary>
        /// 更新前调用
        /// </summary>
        /// <param name="context">上下文</param>
        /// <returns></returns>
        void OnBefore(IEntityUpdateFilterContext context);

        /// <summary>
        /// 更新后调用
        /// </summary>
        /// <param name="context">上下文</param>
        /// <returns></returns>
        void OnAfter(IEntityUpdateFilterContext context);
    }

    /// <summary>
    /// 实体更新过滤器
    /// </summary>
    public class EntityUpdateFilter : IEntityUpdateFilter
    {
        public virtual void OnBefore(IEntityUpdateFilterContext context)
        {
        }

        public virtual void OnAfter(IEntityUpdateFilterContext context)
        {
        }
    }
}
