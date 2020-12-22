using System;
using System.Linq;
using System.Linq.Expressions;
using Data.Common.Test.Domain.Article;
using Data.Common.Test.Domain.Category;
using Microsoft.Extensions.DependencyInjection;
using Mkh.Data.Core.Queryable.Internal;
using Xunit;

namespace Data.Adapter.MySql.Test
{
    public class QueryBodyTests : DbContextTests
    {
        [Fact]
        public void GetColumnNameTest()
        {
            var queryBody = new QueryBody(_serviceProvider.GetService<IArticleRepository>());
            queryBody.Joins.Add(new QueryJoin(_dbContext.EntityDescriptors.FirstOrDefault(m => m.TableName == "Article"), "T1"));

            var join = queryBody.Joins[0];
            var columnName = queryBody.GetColumnName("Title", join);

            Assert.Equal("`Title`", columnName);

            Expression<Func<ArticleEntity, int>> exp = m => m.Id;

            columnName = queryBody.GetColumnName(exp.Body as MemberExpression, exp);

            Assert.Equal("`Id`", columnName);
        }

        [Fact]
        public void GetColumnDescriptorTest()
        {
            //单表
            var queryBody = new QueryBody(_serviceProvider.GetService<IArticleRepository>());
            queryBody.Joins.Add(new QueryJoin(_dbContext.EntityDescriptors.FirstOrDefault(m => m.TableName == "Article"), "T1"));
            
            var join = queryBody.Joins[0];
            var descriptor = queryBody.GetColumnDescriptor("Title", join);

            Assert.Equal("Title", descriptor.Name);

            Expression<Func<ArticleEntity, int>> exp = m => m.Id;

            descriptor = queryBody.GetColumnDescriptor(exp.Body as MemberExpression, exp);

            Assert.Equal("Id", descriptor.Name);

            Expression<Func<ArticleEntity, bool>> exp1 = m => m.Published;

            descriptor = queryBody.GetColumnDescriptor(exp1.Body as MemberExpression, exp1);

            Assert.Equal("IsPublished", descriptor.Name);

            //多表连接
            queryBody.Joins.Add(new QueryJoin(_dbContext.EntityDescriptors.FirstOrDefault(m => m.TableName == "MyCategory"), "T2"));

            Expression<Func<ArticleEntity, CategoryEntity, string>> exp2 = (m, n) => n.Name;

            descriptor = queryBody.GetColumnDescriptor(exp2.Body as MemberExpression, exp2);

            Assert.Equal("Name", descriptor.Name);
        }
    }
}
