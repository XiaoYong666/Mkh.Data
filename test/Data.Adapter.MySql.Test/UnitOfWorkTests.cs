using Data.Common.Test.Domain.Article;
using Data.Common.Test.Domain.Category;
using Xunit;

namespace Data.Adapter.MySql.Test
{
    public class UnitOfWorkTests : BaseTest
    {
        [Fact]
        public async void SaveChangesTest()
        {
            using var uow = _dbContext.NewUnitOfWork();
            var categoryRepository = uow.Get<ICategoryRepository>();
            var articleRepository = uow.Get<IArticleRepository>();

            var category = new CategoryEntity
            {
                Name = ".Net"
            };

            await categoryRepository.Add(category);

            var article = new ArticleEntity
            {
                CategoryId = category.Id,
                Title = "工作单元测试",
                Content = "工作单元测试"
            };

            await articleRepository.Add(article);

            uow.SaveChanges();
        }

        [Fact]
        public async void RollbackTest()
        {
            using var uow = _dbContext.NewUnitOfWork();
            var categoryRepository = uow.Get<ICategoryRepository>();
            var articleRepository = uow.Get<IArticleRepository>();

            var category = new CategoryEntity
            {
                Name = ".Net"
            };

            await categoryRepository.Add(category);

            var article = new ArticleEntity
            {
                CategoryId = category.Id,
                Title = "工作单元测试",
                Content = "工作单元测试"
            };

            await articleRepository.Add(article);

            Assert.True(article.Id > 0);
        }
    }
}
