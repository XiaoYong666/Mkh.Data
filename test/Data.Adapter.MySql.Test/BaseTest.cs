using System;
using Data.Common.Test;
using Data.Common.Test.Infrastructure;
using Divergic.Logging.Xunit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Mkh.Data.Abstractions;
using Xunit.Abstractions;

namespace Data.Adapter.MySql.Test
{
    public class BaseTest
    {
        protected readonly IServiceProvider _serviceProvider;
        protected readonly IDbContext _dbContext;

        public BaseTest(ITestOutputHelper output)
        {
            var connString = "Database=blog;Data Source=localhost;Port=3306;User Id=root;Password=root;Charset=utf8;SslMode=None;allowPublicKeyRetrieval=true;";
            var services = new ServiceCollection();
            //��־
            services.AddLogging(builder =>
            {
                builder.AddXunit(output, new LoggingConfig
                {
                    LogLevel = LogLevel.Trace
                });
            });

            //�Զ����˻���Ϣ������
            services.AddSingleton<IAccountResolver, CustomAccountResolver>();

            services
                .AddMkhDbWidthMySql<BlogDbContext>(connString, options =>
                {
                    options.Log = true;//������־
                })
                .AddRepositories(typeof(BlogDbContext).Assembly);

            _serviceProvider = services.BuildServiceProvider();
            _dbContext = _serviceProvider.GetService<BlogDbContext>();
        }
    }
}
