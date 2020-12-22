using System;
using System.Collections.Generic;
using System.Data;
using Mkh.Data.Abstractions.Adapter;
using Mkh.Data.Abstractions.Descriptors;

namespace Mkh.Data.Abstractions
{
    /// <summary>
    /// 数据库上下文
    /// </summary>
    public interface IDbContext
    {
        #region ==属性==

        /// <summary>
        /// 服务容器
        /// </summary>
        IServiceProvider Services { get; }

        /// <summary>
        /// 数据库配置项
        /// </summary>
        DbOptions Options { get; }

        /// <summary>
        /// 日志记录器
        /// </summary>
        IDbLogger Logger { get; }

        /// <summary>
        /// 数据库适配器
        /// </summary>
        IDbAdapter Adapter { get; }

        /// <summary>
        /// 账户信息解析器
        /// </summary>
        IAccountResolver AccountResolver { get; }

        /// <summary>
        /// 实体描述符列表
        /// </summary>
        IList<IEntityDescriptor> EntityDescriptors { get; }

        /// <summary>
        /// 仓储描述符列表
        /// </summary>
        IList<IRepositoryDescriptor> RepositoryDescriptors { get; }

        #endregion

        #region ==方法==

        /// <summary>
        /// 创建新的连接
        /// </summary>
        IDbConnection NewConnection();

        /// <summary>
        /// 创建工作单元
        /// </summary>
        /// <returns></returns>
        IUnitOfWork NewUnitOfWork();

        /// <summary>
        /// 创建工作单元
        /// </summary>
        /// <param name="isolationLevel">指定锁级别</param>
        /// <returns></returns>
        IUnitOfWork NewUnitOfWork(IsolationLevel isolationLevel);

        #endregion
    }
}
