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
        }

        public IServiceCollection Services { get; }

        public IDbContext DbContext { get; }
    }
}
