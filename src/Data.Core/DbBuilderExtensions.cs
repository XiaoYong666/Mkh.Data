using System;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Mkh.Data;
using Mkh.Data.Abstractions;
using Mkh.Data.Core.Descriptors;
using Mkh.Data.Core.Internal;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 数据库生成器扩展
    /// </summary>
    public static class DbBuilderExtensions
    {
        /// <summary>
        /// 从指定程序集加载仓储
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static IDbBuilder AddRepositories(this IDbBuilder builder, Assembly assembly)
        {
            var serviceLifetime = builder.DbContext.Options.RepositoryServiceLifetime;
            if (serviceLifetime == ServiceLifetime.Transient)
            {
                throw new ArgumentException("仓储暂不支持瞬时模式(Transient)注入");
            }

            var services = builder.Services;
            if (serviceLifetime == ServiceLifetime.Scoped)
            {
                //尝试添加仓储实例管理器
                services.TryAddScoped<IRepositoryManager, RepositoryManager>();
            }

            var repositoryTypes = assembly.GetTypes().Where(m => !m.IsInterface && typeof(IRepository).IsImplementType(m)).ToList();
            if (repositoryTypes.Any())
            {
                foreach (var type in repositoryTypes)
                {
                    //按照框架约定，仓储的第二个接口类型就是所需的仓储接口
                    var interfaceType = type.GetInterfaces()[2];

                    if (serviceLifetime == ServiceLifetime.Scoped)
                    {
                        builder.Services.AddScoped(interfaceType, sp =>
                        {
                            var instance = Activator.CreateInstance(type);
                            var initMethod = type.GetMethod("Init", BindingFlags.Instance | BindingFlags.NonPublic);
                            initMethod!.Invoke(instance, new Object[] { builder.DbContext });

                            //保存仓储实例
                            var manager = sp.GetService<IRepositoryManager>();
                            manager?.Add((IRepository)instance);

                            return instance;
                        });
                    }
                    else
                    {
                        var instance = Activator.CreateInstance(type);
                        var initMethod = type.GetMethod("Init", BindingFlags.Instance | BindingFlags.NonPublic);
                        initMethod!.Invoke(instance, new Object[] { builder.DbContext });
                        builder.Services.AddSingleton(interfaceType, instance!);
                    }
                    //保存仓储描述符
                    builder.DbContext.RepositoryDescriptors.Add(new RepositoryDescriptor(interfaceType, type));
                }
            }

            return builder;
        }
    }
}
