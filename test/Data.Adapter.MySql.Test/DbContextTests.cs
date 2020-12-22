using System.Data;
using Xunit;

namespace Data.Adapter.MySql.Test
{
    public class DbContextTests : BaseTest
    {
        /// <summary>
        /// 数据库上下文状态测试
        /// </summary>
        [Fact]
        public void NewConnectionTest()
        {
            using var con = _dbContext.NewConnection();

            Assert.NotNull(con);
            Assert.Equal(ConnectionState.Closed, con.State);

            con.Open();

            Assert.Equal(ConnectionState.Open, con.State);
        }
    }
}
