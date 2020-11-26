using System;
using Data.Common.Test.Domain.Article;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Data.Adapter.MySql.Test
{
    public class RepositoryTest : BaseTest
    {
        private readonly IArticleRepository _repository;

        public RepositoryTest()
        {
            _repository = ServiceProvider.GetService<IArticleRepository>();
        }

        [Fact]
        public async void AddTest()
        {
            var article = new ArticleEntity
            {
                Title = "test",
                Content = "test",
                TimeSpan = new TimeSpan(1, 1, 1, 1)
            };

            await _repository.Add(article);
            /*INSERT INTO `Article` (`CategoryId`,`Title`,`Content`,`Published`,`PublishedTime`,`TimeSpan`,`CreatedBy`,
            `Creator`,`CreatedTime`,`ModifiedBy`,`Modifier`,`ModifiedTime`) VALUES(@CategoryId,@Title,@Content,@Published,
            @PublishedTime,@TimeSpan,@CreatedBy,@Creator,@CreatedTime,@ModifiedBy,@Modifier,@ModifiedTime);*/

            Assert.True(article.Id > 0);

            var article1 = await _repository.Get(article.Id);

            Assert.Equal(article.Title, article1.Title);
            Assert.Equal("OLDLI", article1.Creator);
        }

        [Fact]
        public async void DeleteTest()
        {
            var article = new ArticleEntity
            {
                Title = "test",
                Content = "test"
            };

            await _repository.Add(article);

            Assert.True(article.Id > 0);

            var result = await _repository.Delete(article.Id);
            //DELETE FROM `Article`  WHERE `Id`=@Id;

            Assert.True(result);

            var article1 = await _repository.Get(article.Id);

            Assert.Null(article1);
        }

        [Fact]
        public async void UpdateTest()
        {
            var article = new ArticleEntity
            {
                Title = "test",
                Content = "test"
            };

            await _repository.Add(article);

            var article1 = await _repository.Get(article.Id);
            article1.Title = "修改标题";

            var result = await _repository.Update(article1);
            /*
             * SELECT `Id` AS `Id`,`CategoryId` AS `CategoryId`,`Title` AS `Title`,`Content` AS `Content`,`Published` AS `Published`,
             * `PublishedTime` AS `PublishedTime`,`TimeSpan` AS `TimeSpan`,`CreatedBy` AS `CreatedBy`,`Creator` AS `Creator`,
             * `CreatedTime` AS `CreatedTime`,`ModifiedBy` AS `ModifiedBy`,`Modifier` AS `Modifier`,`ModifiedTime` AS `ModifiedTime`
             * FROM `Article`  WHERE `Id`=@Id 
             */

            Assert.True(result);

            var article2 = await _repository.Get(article1.Id);

            Assert.Equal(article1.Title, article2.Title);
            Assert.Equal("OLDLI", article1.Modifier);
        }

        [Fact]
        public async void SoftDeleteTest()
        {
            var article = new ArticleEntity
            {
                Title = "test",
                Content = "test"
            };

            await _repository.Add(article);

            Assert.True(article.Id > 0);

            var result = await _repository.SoftDelete(article.Id);
            //DELETE FROM `Article`  WHERE `Id`=@Id;

            Assert.True(result);

            var article1 = await _repository.Get(article.Id);

            Assert.Null(article1);
        }

        [Fact]
        public async void ExistsTest()
        {
            var article = new ArticleEntity
            {
                Title = "test",
                Content = "test",
                TimeSpan = new TimeSpan(1, 1, 1, 1)
            };

            await _repository.Add(article);

            var exists = await _repository.Exists(article.Id);
            //SELECT 1 FROM `Article` WHERE `Id`=@Id AND `Deleted`=0  LIMIT 1

            Assert.True(exists);

            exists = await _repository.Exists(100000);
            Assert.False(exists);
        }
    }
}
