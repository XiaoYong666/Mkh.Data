using System;

namespace Mkh.Data.Abstractions
{
    /// <summary>
    /// 工作单元接口
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// 获取指定类型仓储实例
        /// </summary>
        /// <typeparam name="TRepository"></typeparam>
        /// <returns></returns>
        TRepository Get<TRepository>() where TRepository : IRepository;

        /// <summary>
        /// 保存变更
        /// </summary>
        void SaveChanges();
    }
}
