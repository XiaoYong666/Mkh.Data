using System;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Mkh.Data.Abstractions;
using Mkh.Data.Abstractions.Adapter;
using Mkh.Data.Abstractions.Logger;
using Mkh.Data.Core;
using Mkh.Data.Core.Internal;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="services"></param>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="dbAdapter">数据库适配器</param>
        /// <param name="configure"></param>
        /// <returns></returns>
        public static IDbBuilder AddMkhDb<TDbContext>(this IServiceCollection services, string connectionString, IDbAdapter dbAdapter, Action<DbOptions> configure) where TDbContext : IDbContext
        {
            var options = new DbOptions { ConnectionString = connectionString };

            configure?.Invoke(options);

            //尝试添加默认账户信息解析器
            services.TryAddSingleton<IAccountResolver, DefaultAccountResolver>();
            //尝试添加默认的数据库操作日志记录器
            services.TryAddSingleton<IDbLoggerProvider, DbLoggerProvider>();

            var sp = services.BuildServiceProvider();

            //创建数据库上下文实例，通过反射设置属性
            var dbContextType = typeof(TDbContext);
            var dbContext = (TDbContext)Activator.CreateInstance(dbContextType);
            dbContextType.GetProperty("Services")?.SetValue(dbContext, sp);
            dbContextType.GetProperty("Options")?.SetValue(dbContext, options);
            dbContextType.GetProperty("Logger")?.SetValue(dbContext, new DbLogger(options, sp.GetService<IDbLoggerProvider>()));
            dbContextType.GetProperty("Adapter")?.SetValue(dbContext, dbAdapter);
            dbContextType.GetProperty("AccountResolver")?.SetValue(dbContext, sp.GetService<IAccountResolver>());

            // ReSharper disable once AssignNullToNotNullAttribute
            services.AddSingleton(dbContextType, dbContext);

            var builder = new DbBuilder(services, dbContext);
            return builder;
        }
    }
}
