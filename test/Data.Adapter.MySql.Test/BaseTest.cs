using System;
using Data.Common.Test;
using Data.Common.Test.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Mkh.Data.Abstractions;

namespace Data.Adapter.MySql.Test
{
    public class BaseTest
    {
        protected readonly IServiceProvider _serviceProvider;
        protected readonly IDbContext _dbContext;

        public BaseTest()
        {
            var connString = "Database=blog;Data Source=localhost;Port=3306;User Id=root;Password=root;Charset=utf8;SslMode=None;allowPublicKeyRetrieval=true;";
            var services = new ServiceCollection();
            //日志
            services.AddLogging(builder =>
            {
                builder.AddDebug();
                builder.AddTraceSource("MySql");
            });

            //自定义账户信息解析器
            services.AddSingleton<IAccountResolver, CustomAccountResolver>();

            services
                .AddMkhDbWidthMySql<BlogDbContext>(connString)
                .AddRepositories(typeof(BlogDbContext).Assembly);

            _serviceProvider = services.BuildServiceProvider();
            _dbContext = _serviceProvider.GetService<BlogDbContext>();
        }
    }
}
