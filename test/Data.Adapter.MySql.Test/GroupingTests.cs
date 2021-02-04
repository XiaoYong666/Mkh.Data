using Data.Common.Test.Domain.Article;
using Data.Common.Test.Domain.Category;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;

namespace Data.Adapter.MySql.Test
{
    public class GroupingTests : BaseTest
    {
        private readonly IArticleRepository _repository;

        public GroupingTests(ITestOutputHelper output) : base(output)
        {
            _repository = _serviceProvider.GetService<IArticleRepository>();
        }

        [Fact]
        public void Test1()
        {
            var query = _repository.Find().GroupBy(m => new { m.Title, m.Deleted })
                .Select(m => new
                {
                    Sum = m.Sum(n => n.Id),
                    m.Key.Title
                });

            var sql = query.ToListSql();
            Assert.Equal("SELECT SUM(`Id`) AS `Sum`,`Title` AS `Title` FROM `Article` WHERE `Deleted` = 0 GROUP BY `Title`, `Deleted`", sql);
        }

        [Fact]
        public void Test2()
        {
            var sql = _repository.Find().LeftJoin<CategoryEntity>((x, y) => x.CategoryId == y.Id)
                .GroupBy((x, y) => new { name = y.Name.Substring(1, 3) })
                .Select(m => new
                {
                    Sum = m.Sum((x, y) => x.Id),
                    Count = m.Count(),
                    name1 = m.Key.name
                })
                .ToListSql();

            Assert.Equal("SELECT SUM(T1.`Id`) AS `Sum`,COUNT(0) AS `Count`,SUBSTR(T2.`Name`,2,3) AS `name1` FROM `Article` AS T1 LEFT JOIN `MyCategory` AS T2 ON T1.`CategoryId` = T2.`Id` WHERE `T1`.`Deleted` = 0 GROUP BY SUBSTR(T2.`Name`,2,3)", sql);
        }

        [Fact]
        public void Test3()
        {
            var query = _repository.Find()
                .GroupBy(m => new { m.Deleted })
                .Having(m => m.Sum(x => x.Id) > 3)
                .Select(m => new { Sum = m.Sum(n => n.Id) });

            var sql = query.ToListSql();
            Assert.Equal("SELECT SUM(`Id`) AS `Sum` FROM `Article` WHERE `Deleted` = 0 GROUP BY `Deleted` HAVING SUM(`Id`) > @P1", sql);

            sql = query.ToListSqlNotUseParameters();
            Assert.Equal("SELECT SUM(`Id`) AS `Sum` FROM `Article` WHERE `Deleted` = 0 GROUP BY `Deleted` HAVING SUM(`Id`) > 3", sql);
        }

        [Fact]
        public void Test4()
        {
            var query = _repository.Find()
                .Where(m => m.Id > 5)
                .GroupBy(m => new { m.Deleted })
                .Having(m => m.Sum(x => x.Id) > 3)
                .OrderBy(m => m.Sum(x => x.Id))
                .Select(m => new
                {
                    Sum = m.Sum(n => n.Id),
                });

            var sql = query.ToListSql();
            Assert.Equal("SELECT SUM(`Id`) AS `Sum` FROM `Article` WHERE `Id` > @P1 AND `Deleted` = 0 GROUP BY `Deleted` HAVING SUM(`Id`) > @P2 ORDER BY SUM(`Id`) ASC", sql);

            sql = query.ToListSqlNotUseParameters();
            Assert.Equal("SELECT SUM(`Id`) AS `Sum` FROM `Article` WHERE `Id` > 5 AND `Deleted` = 0 GROUP BY `Deleted` HAVING SUM(`Id`) > 3 ORDER BY SUM(`Id`) ASC", sql);
        }

        [Fact]
        public void Test5()
        {
            var sql = _repository.Find().GroupBy(m => new { m.Deleted })
                .Having(m => m.Sum(x => x.Id) > 3)
                .OrderBy(m => m.Key.Deleted)
                .Select(m => new
                {
                    Sum = m.Sum(n => n.Id),
                })
                .ToListSql();

            Assert.Equal("SELECT SUM(`Id`) AS `Sum` FROM `Article` WHERE `Deleted` = 0 GROUP BY `Deleted` HAVING SUM(`Id`) > @P1 ORDER BY `Deleted` ASC", sql);
        }

        [Fact]
        public void Test6()
        {
            var sql = _repository.Find().GroupBy(m => new { m.Title })
                .Having(m => m.Sum(x => x.Id) > 3)
                .OrderBy(m => m.Key.Title.Substring(3))
                .Select(m => new
                {
                    Sum = m.Sum(n => n.Id),
                })
                .ToListSql();

            Assert.Equal("SELECT SUM(`Id`) AS `Sum` FROM `Article` WHERE `Deleted` = 0 GROUP BY `Title` HAVING SUM(`Id`) > @P1 ORDER BY SUBSTR(`Title`,4) ASC", sql);
        }

        [Fact]
        public void Test7()
        {
            var sql = _repository.Find().LeftJoin<CategoryEntity>((x, y) => x.CategoryId == y.Id)
                .GroupBy((x, y) => new
                {
                    y.Name
                })
                .Having(m => m.Sum((x, y) => x.Id) > 5)
                .OrderBy(m => m.Sum((x, y) => x.Id))
                .OrderByDescending(m => m.Key.Name.Substring(2))
                .Select(m => new
                {
                    Sum = m.Sum((x, y) => x.Id),
                    Name = m.Key.Name.Substring(5)
                })
                .ToListSql();

            Assert.Equal("SELECT SUM(T1.`Id`) AS `Sum`,SUBSTR(T2.`Name`,6) AS `Name` FROM `Article` AS T1 LEFT JOIN `MyCategory` AS T2 ON T1.`CategoryId` = T2.`Id` WHERE `T1`.`Deleted` = 0 GROUP BY T2.`Name` HAVING SUM(T1.`Id`) > @P1 ORDER BY SUM(T1.`Id`) ASC, SUBSTR(T2.`Name`,3) DESC", sql);
        }
    }
}
