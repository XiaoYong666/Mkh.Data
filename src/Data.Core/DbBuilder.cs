using System;
using Microsoft.Extensions.DependencyInjection;
using Mkh.Data.Abstractions;

namespace Mkh.Data.Core
{
    internal class DbBuilder : IDbBuilder
    {
        public DbBuilder(IServiceCollection services, IDbContext dbContext)
        {
            Services = services;
            DbContext = dbContext;
            DbContextTypeHandle = typeof(DbContext).TypeHandle;
        }

        public IServiceCollection Services { get; }

        public IDbContext DbContext { get; }

        public RuntimeTypeHandle DbContextTypeHandle { get; }
    }
}
