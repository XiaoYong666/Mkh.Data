using Data.Common.Test.Domain.Article;
using Data.Common.Test.Domain.Category;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;

namespace Data.Adapter.MySql.Test
{
    public class TableJoinTest : BaseTest
    {
        private readonly IArticleRepository _repository;

        public TableJoinTest(ITestOutputHelper output) : base(output)
        {
            _repository = _serviceProvider.GetService<IArticleRepository>();
        }

        [Fact]
        public void Test1()
        {
            var sql = _repository.Find().LeftJoin<CategoryEntity>((t1, t2) => t1.CategoryId == t2.Id).Select((t1, t2) => new { t1 }).ToListSql();
            //SELECT T1.`Id`            AS `Id`,
            //        T1.`CategoryId`    AS `CategoryId`,
            //        T1.`Title`         AS `Title`,
            //        T1.`Content`       AS `Content`,
            //        T1.`IsPublished`   AS `Published`,
            //        T1.`PublishedTime` AS `PublishedTime`,
            //        T1.`Deleted`       AS `Deleted`,
            //        T1.`DeletedBy`     AS `DeletedBy`,
            //        T1.`Deleter`       AS `Deleter`,
            //        T1.`DeletedTime`   AS `DeletedTime`,
            //        T1.`CreatedBy`     AS `CreatedBy`,
            //        T1.`Creator`       AS `Creator`,
            //        T1.`CreatedTime`   AS `CreatedTime`,
            //        T1.`ModifiedBy`    AS `ModifiedBy`,
            //        T1.`Modifier`      AS `Modifier`,
            //        T1.`ModifiedTime`  AS `ModifiedTime`
            //FROM `Article` AS T1
            //            LEFT JOIN `MyCategory` AS T2 ON T1.`CategoryId` = T2.`Id`
            //WHERE `T1`.`Deleted` = 0

            Assert.NotEmpty(sql);
        }
    }
}
