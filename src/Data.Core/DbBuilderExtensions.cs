using System;
using System.Linq;
using System.Reflection;
using Mkh.Data;
using Mkh.Data.Abstractions;
using Mkh.Data.Abstractions.Filters;
using Mkh.Data.Abstractions.Filters.Entity;
using Mkh.Data.Core.Descriptors;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 数据库生成器扩展
    /// </summary>
    public static class DbBuilderExtensions
    {
        /// <summary>
        /// 添加全局过滤器
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static IDbBuilder AddGlobalFilter(this IDbBuilder builder, IFilter filter)
        {
            var filterEngine = builder.DbContext.FilterEngine;
            var type = filter.GetType();
            if (typeof(IEntityAddFilter) == type)
            {
                filterEngine.EntityAddFilters.Add((IEntityAddFilter)filter);
            }
            else if (typeof(IEntityDeleteFilter) == type)
            {
                filterEngine.EntityDeleteFilters.Add((IEntityDeleteFilter)filter);
            }
            else if (typeof(IEntityUpdateFilter) == type)
            {
                filterEngine.EntityUpdateFilters.Add((IEntityUpdateFilter)filter);
            }

            return builder;
        }

        /// <summary>
        /// 从指定程序集加载仓储
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static IDbBuilder AddRepositories(this IDbBuilder builder, Assembly assembly)
        {
            var repositoryTypes = assembly.GetTypes().Where(m => !m.IsInterface && typeof(IRepository).IsImplementType(m)).ToList();
            if (repositoryTypes.Any())
            {
                foreach (var type in repositoryTypes)
                {
                    //按照框架设计，第二个接口类型就是所需的仓储接口
                    var interfaceType = type.GetInterfaces()[2];
                    var instance = Activator.CreateInstance(type);
                    var initMethod = type.GetMethod("Init", BindingFlags.Instance | BindingFlags.NonPublic);
                    // ReSharper disable once PossibleNullReferenceException
                    initMethod.Invoke(instance, new Object[] { builder.DbContext });

                    // ReSharper disable once AssignNullToNotNullAttribute
                    builder.Services.AddSingleton(interfaceType, instance);

                    //保存仓储描述符
                    builder.DbContext.RepositoryDescriptors.Add(new RepositoryDescriptor(interfaceType, type));
                }
            }

            return builder;
        }
    }
}
