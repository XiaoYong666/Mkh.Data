using Xunit;

namespace Data.Core.Test
{
    public class FilterTests : DbContextTests
    {
        /// <summary>
        /// 全局实体操作过滤器测试
        /// </summary>
        [Fact]
        public void GlobalEntityActionFilterTest()
        {
            Assert.Equal(1, _context.FilterEngine.EntityAddFilters.Count);
            Assert.Equal(1, _context.FilterEngine.EntityUpdateFilters.Count);
        }

        /// <summary>
        /// 自定义实体操作过滤器
        /// </summary>
        [Fact]
        public void CustomEntityActionFilterTest()
        {
            Assert.Equal(1, _articleRepository.EntityDescriptor.FilterEngine.EntityAddFilters.Count);
        }
    }
}
