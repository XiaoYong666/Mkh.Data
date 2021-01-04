using System;
using Mkh.Data.Abstractions;
using Mkh.Data.Adapter.MySql;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IDbBuilder AddMkhDbWidthMySql<TDbContext>(this IServiceCollection services, string connectionString) where TDbContext : IDbContext
        {
            return services.AddMkhDb<TDbContext>(connectionString, new MySqlDbAdapter(), null);
        }

        public static IDbBuilder AddMkhDbWidthMySql<TDbContext>(this IServiceCollection services, string connectionString, Action<DbOptions> configure) where TDbContext : IDbContext
        {
            return services.AddMkhDb<TDbContext>(connectionString, new MySqlDbAdapter(), configure);
        }
    }
}
