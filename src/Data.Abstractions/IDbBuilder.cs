using System;
using Microsoft.Extensions.DependencyInjection;

namespace Mkh.Data.Abstractions
{
    public interface IDbBuilder
    {
        /// <summary>
        /// 服务集合
        /// </summary>
        IServiceCollection Services { get; }

        /// <summary>
        /// 数据库上下文
        /// </summary>
        IDbContext DbContext { get; }

        /// <summary>
        /// 数据库上下文类型
        /// </summary>
        RuntimeTypeHandle DbContextTypeHandle { get; }
    }
}
