using Microsoft.Extensions.Logging;
using Mkh.Data.Abstractions;

namespace Mkh.Data.Core
{
    /// <summary>
    /// 默认日志记录器
    /// </summary>
    internal class DbLogger : IDbLogger
    {
        private readonly ILogger _logger;

        public DbLogger(ILogger<DbLogger> logger)
        {
            _logger = logger;
        }


        public void Write(string action, string sql)
        {
            _logger.LogDebug($"{action}:{sql}");
        }
    }
}
