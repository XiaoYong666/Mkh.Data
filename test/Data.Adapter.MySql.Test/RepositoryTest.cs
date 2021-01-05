using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Common.Test.Domain.Article;
using Data.Common.Test.Domain.Category;
using Microsoft.Extensions.DependencyInjection;
using Mkh.Data.Abstractions.Extensions;
using Mkh.Data.Abstractions.Pagination;
using Xunit;
using Xunit.Abstractions;

namespace Data.Adapter.MySql.Test
{
    public class RepositoryTest : BaseTest
    {
        private readonly IArticleRepository _repository;

        public RepositoryTest(ITestOutputHelper output) : base(output)
        {
            _repository = _serviceProvider.GetService<IArticleRepository>();
        }

        private Task ClearTable()
        {
            return _repository.Execute("truncate article;");
        }

        [Fact]
        public async void AddTest()
        {
            await ClearTable();

            var article = new ArticleEntity
            {
                Title = "test",
                Content = "test"
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
                Content = "test"
            };

            await _repository.Add(article);

            var exists = await _repository.Exists(article.Id);
            //SELECT 1 FROM `Article` WHERE `Id`=@Id AND `Deleted`=0  LIMIT 1

            Assert.True(exists);

            exists = await _repository.Exists(100000);
            Assert.False(exists);

            exists = await _repository.Find(m => m.Id == 1).Exists();
            Assert.True(exists);

            exists = await _repository.Find(m => m.Id == 1000).Exists();
            Assert.False(exists);
        }

        private async Task ClearAndAdd(int count = 10)
        {
            await ClearTable();

            for (int i = 1; i <= count; i++)
            {
                var article = new ArticleEntity
                {
                    Title = i < count / 2 ? "test" + i : "mkh" + i,
                    Content = "test"
                };

                await _repository.Add(article);
            }
        }

        [Fact]
        public async void ListTest()
        {
            await ClearAndAdd();

            var list = await _repository.Find().List();

            Assert.Equal(10, list.Count);

            list = await _repository.Find(m => m.Id > 5).List();
            Assert.Equal("mkh10", list[4].Title);

            list = await _repository.Find(m => m.Id == 7).List();
            Assert.Single(list);
            Assert.Equal("mkh7", list.First().Title);

            list = await _repository.Find(m => m.Title.Contains("9")).List();
            Assert.Single(list);

            list = await _repository.Find(m => m.Title.StartsWith("mkh")).List();
            Assert.Equal(6, list.Count);

            list = await _repository.Find(m => m.Title.EndsWith("9") || m.Title.EndsWith("1")).List();
            Assert.Equal(2, list.Count);

            var ids = new List<int> { 3, 5, 9 };
            list = await _repository.Find(m => ids.Contains(m.Id)).List();
            Assert.Equal(3, list.Count);
            Assert.Equal("mkh5", list[1].Title);

            list = await _repository.Find(m => ids.NotContains(m.Id)).List();
            Assert.Equal(7, list.Count);
            Assert.Equal("test1", list[0].Title);
        }

        [Fact]
        public async void PaginationTest()
        {
            await ClearAndAdd(20);

            var list = await _repository.Find().Pagination();

            Assert.Equal(15, list.Count);

            var paging = new Paging(2, 10);
            list = await _repository.Find().Pagination(paging);

            Assert.Equal(20, paging.TotalCount);
            Assert.Equal("mkh11", list[0].Title);

            list = await _repository.Find(m => m.Id > 5).Pagination(new Paging(2, 3));
            Assert.Equal("test9", list[0].Title);
        }

        [Fact]
        public async void FirstTest()
        {
            await ClearAndAdd();

            var first = await _repository.Find(m => m.Title == "test3").First();

            Assert.NotNull(first);
            Assert.Equal(3, first.Id);
        }

        [Fact]
        public async void NotContainsTest()
        {
            await ClearAndAdd();

            var ids = new List<int>();
            var list = await _repository.Find(m => ids.NotContains(m.Id)).List();

            Assert.Equal(10, list.Count);
        }

        [Fact]
        public async void FunctionTest()
        {
            await ClearAndAdd();

            var maxId = await _repository.Find().Max(m => m.Id);

            Assert.Equal(10, maxId);

            maxId = await _repository.Find(m => m.Id < 8).Max(m => m.Id);

            Assert.Equal(7, maxId);

            var minId = await _repository.Find(m => m.Id > 5).Min(m => m.Id);

            Assert.Equal(6, minId);

            var avg = await _repository.Find(m => m.Id > 5 && m.Id < 10).Avg<decimal>(m => m.Id);

            Assert.Equal(7.5M, avg);

            var sum = await _repository.Find(m => m.Id > 5 && m.Id < 10).Sum(m => m.Id);

            Assert.Equal(30, sum);
        }
    }
}
