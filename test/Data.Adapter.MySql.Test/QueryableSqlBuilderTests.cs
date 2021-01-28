using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Data.Common.Test.Domain.Article;
using Data.Common.Test.Domain.Category;
using Microsoft.Extensions.DependencyInjection;
using Mkh.Data.Abstractions.Extensions;
using Mkh.Data.Abstractions.Pagination;
using Mkh.Data.Core.Queryable.Internal;
using Mkh.Data.Core.SqlBuilder;
using Xunit;
using Xunit.Abstractions;

namespace Data.Adapter.MySql.Test
{
    public class QueryableSqlBuilderTests : DbContextTests
    {
        public QueryableSqlBuilderTests(ITestOutputHelper output) : base(output)
        {
        }

        internal QueryableSqlBuilder CreateBuilder()
        {
            var queryBody = new QueryBody(_serviceProvider.GetService<IArticleRepository>());
            queryBody.Joins.Add(new QueryJoin(_dbContext.EntityDescriptors.FirstOrDefault(m => m.TableName == "Article"), "T1"));
            return new QueryableSqlBuilder(queryBody);
        }

        #region ==解析排序==

        //解析排序测试
        [Fact]
        public void ResolveSortTest()
        {
            var builder = CreateBuilder();

            Expression<Func<ArticleEntity, string>> exp = m => m.Title;
            builder.QueryBody.SetSort(exp, SortType.Asc);
            Assert.Equal(" `Title` ASC", builder.ResolveSort());
        }

        //解析排序测试
        [Fact]
        public void ResolveSortForSqlTest()
        {
            var builder = CreateBuilder();

            builder.QueryBody.SetSort("Title", SortType.Asc);
            Assert.Equal(" Title ASC", builder.ResolveSort());
        }

        //解析排序，针对字符串的Substring函数
        [Fact]
        public void ResolveSortForSubstringTest()
        {
            var builder = CreateBuilder();

            Expression<Func<ArticleEntity, string>> exp = m => m.Title.Substring(1);
            builder.QueryBody.SetSort(exp, SortType.Asc);
            Assert.Equal(" SUBSTR(`Title`,2) ASC", builder.ResolveSort());
        }

        //解析排序，针对字符串的Length属性
        [Fact]
        public void ResolveSortForLengthTest()
        {
            var builder = CreateBuilder();

            Expression<Func<ArticleEntity, int>> exp = m => m.Title.Length;
            builder.QueryBody.SetSort(exp, SortType.Asc);
            Assert.Equal(" CHAR_LENGTH(`Title`) ASC", builder.ResolveSort());
        }

        //解析排序，针对日期格式化函数
        [Fact]
        public void ResolveSortForDatetimeFormatTest()
        {
            var builder = CreateBuilder();
            Expression<Func<ArticleEntity, string>> exp = m => m.CreatedTime.ToString("YYYY-MM-DD");
            builder.QueryBody.SetSort(exp, SortType.Asc);
            Assert.Equal(" DATE_FORMAT(`CreatedTime`,'%Y-%m-%d') ASC", builder.ResolveSort());

            builder = CreateBuilder();
            Expression<Func<ArticleEntity, string>> exp1 = m => m.CreatedTime.ToString("YY-MM-DD");
            builder.QueryBody.SetSort(exp1, SortType.Asc);
            Assert.Equal(" DATE_FORMAT(`CreatedTime`,'%y-%m-%d') ASC", builder.ResolveSort());
        }

        //解析排序，针对字符串的Replace函数
        [Fact]
        public void ResolveSortForReplaceTest()
        {
            var builder = CreateBuilder();
            Expression<Func<ArticleEntity, string>> exp = m => m.Title.Replace("a", "c");
            builder.QueryBody.SetSort(exp, SortType.Asc);
            Assert.Equal(" REPLACE(`Title`,'a','c') ASC", builder.ResolveSort());
        }

        //解析排序，针对字符串的ToLower函数
        [Fact]
        public void ResolveSortForToLowerTest()
        {
            var builder = CreateBuilder();
            Expression<Func<ArticleEntity, string>> exp = m => m.Title.ToLower();
            builder.QueryBody.SetSort(exp, SortType.Asc);
            Assert.Equal(" LOWER(`Title`) ASC", builder.ResolveSort());
        }

        //解析排序，针对字符串的ToUpper函数
        [Fact]
        public void ResolveSortForToUpperTest()
        {
            var builder = CreateBuilder();
            Expression<Func<ArticleEntity, string>> exp = m => m.Title.ToUpper();
            builder.QueryBody.SetSort(exp, SortType.Asc);
            Assert.Equal(" UPPER(`Title`) ASC", builder.ResolveSort());
        }

        //解析排序，针对匿名函数
        [Fact]
        public void ResolveSortForNewTest()
        {
            var builder = CreateBuilder();
            Expression<Func<ArticleEntity, dynamic>> exp = m => new { m.Id, m.Title };
            builder.QueryBody.SetSort(exp, SortType.Asc);
            Assert.Equal(" `Id` ASC, `Title` ASC", builder.ResolveSort());

            builder = CreateBuilder();
            Expression<Func<ArticleEntity, dynamic>> exp1 = m => new { m.Title.Length, Title = m.Title.Substring(2, 2) };
            builder.QueryBody.SetSort(exp1, SortType.Desc);
            Assert.Equal(" CHAR_LENGTH(`Title`) DESC, SUBSTR(`Title`,3,2) DESC", builder.ResolveSort());
        }

        #endregion

        #region ==解析排除列==

        //解析排除列
        [Fact]
        public void ResolveSelectExcludeColumnsTest()
        {
            var builder = CreateBuilder();
            Expression<Func<ArticleEntity, string>> exp = m => m.Content;
            builder.QueryBody.SetSelectExclude(exp);
            var columns = builder.ResolveSelectExcludeColumns();

            Assert.Single(columns);


            Expression<Func<ArticleEntity, dynamic>> exp1 = m => new { m.Id, m.Content };
            builder.QueryBody.SetSelectExclude(exp1);
            columns = builder.ResolveSelectExcludeColumns();

            Assert.Equal(2, columns.Count);
        }

        #endregion

        #region ==解析选择列==

        //解析单个完整实体
        [Fact]
        public void ResolveSelectForEntityTest()
        {
            var builder = CreateBuilder();
            var sb = new StringBuilder();
            builder.ResolveSelectForEntity(sb);

            var sql = sb.ToString();

            Assert.Equal("`Id` AS `Id`,`CategoryId` AS `CategoryId`,`Title` AS `Title`,`Content` AS `Content`,`IsPublished` AS `Published`,`PublishedTime` AS `PublishedTime`,`Deleted` AS `Deleted`,`DeletedBy` AS `DeletedBy`,`Deleter` AS `Deleter`,`DeletedTime` AS `DeletedTime`,`CreatedBy` AS `CreatedBy`,`Creator` AS `Creator`,`CreatedTime` AS `CreatedTime`,`ModifiedBy` AS `ModifiedBy`,`Modifier` AS `Modifier`,`ModifiedTime` AS `ModifiedTime`,", sql);
        }

        //解析单个实体并排除指定列
        [Fact]
        public void ResolveSelectForEntityAndExcludeColumnsTest()
        {
            //排除单个列
            var builder = CreateBuilder();
            var sb = new StringBuilder();
            Expression<Func<ArticleEntity, string>> exp = m => m.Content;
            builder.QueryBody.SetSelectExclude(exp);
            var columns = builder.ResolveSelectExcludeColumns();
            builder.ResolveSelectForEntity(sb, 0, columns);

            var sql = sb.ToString();

            Assert.Equal("`Id` AS `Id`,`CategoryId` AS `CategoryId`,`Title` AS `Title`,`IsPublished` AS `Published`,`PublishedTime` AS `PublishedTime`,`Deleted` AS `Deleted`,`DeletedBy` AS `DeletedBy`,`Deleter` AS `Deleter`,`DeletedTime` AS `DeletedTime`,`CreatedBy` AS `CreatedBy`,`Creator` AS `Creator`,`CreatedTime` AS `CreatedTime`,`ModifiedBy` AS `ModifiedBy`,`Modifier` AS `Modifier`,`ModifiedTime` AS `ModifiedTime`,", sql);

            //排除多个列
            sb.Clear();
            Expression<Func<ArticleEntity, dynamic>> exp1 = m => new { m.CategoryId, m.Content };
            builder.QueryBody.SetSelectExclude(exp1);
            columns = builder.ResolveSelectExcludeColumns();
            builder.ResolveSelectForEntity(sb, 0, columns);

            sql = sb.ToString();

            Assert.Equal("`Id` AS `Id`,`Title` AS `Title`,`IsPublished` AS `Published`,`PublishedTime` AS `PublishedTime`,`Deleted` AS `Deleted`,`DeletedBy` AS `DeletedBy`,`Deleter` AS `Deleter`,`DeletedTime` AS `DeletedTime`,`CreatedBy` AS `CreatedBy`,`Creator` AS `Creator`,`CreatedTime` AS `CreatedTime`,`ModifiedBy` AS `ModifiedBy`,`Modifier` AS `Modifier`,`ModifiedTime` AS `ModifiedTime`,", sql);
        }

        //解析自定SQL语句类型选择列
        [Fact]
        public void ResolveSelectForSqlTest()
        {
            var builder = CreateBuilder();
            builder.QueryBody.SetSelect("Title AS 'Title1'");
            var sql = builder.ResolveSelect();

            Assert.Equal("Title AS 'Title1'", sql);
        }

        //解析自定类型的选择列
        [Fact]
        public void ResolveSelectTest()
        {
            var builder = CreateBuilder();
            Expression<Func<ArticleEntity, string>> exp = m => m.Content;
            builder.QueryBody.SetSelect(exp);
            var sql = builder.ResolveSelect();

            Assert.Equal("`Content` AS `Content`", sql);

            Expression<Func<ArticleEntity, dynamic>> exp1 = m => new { m.Id, Len = m.Title.Length };
            builder.QueryBody.SetSelect(exp1);
            sql = builder.ResolveSelect();

            Assert.Equal("`Id` AS `Id`,CHAR_LENGTH(`Title`) AS `Len`", sql);

            Expression<Func<ArticleEntity, dynamic>> exp2 = m => new { m.Id, Title = m.Title.Substring(2), Content = m.Content.Replace("a", "b") };
            builder.QueryBody.SetSelect(exp2);
            sql = builder.ResolveSelect();

            Assert.Equal("`Id` AS `Id`,SUBSTR(`Title`,3) AS `Title`,REPLACE(`Content`,'a','b') AS `Content`", sql);


            Expression<Func<ArticleEntity, dynamic>> exp3 = m => new { Title = m.Title.ToLower(), Content = m.Content.ToUpper() };
            builder.QueryBody.SetSelect(exp3);
            sql = builder.ResolveSelect();

            Assert.Equal("LOWER(`Title`) AS `Title`,UPPER(`Content`) AS `Content`", sql);

            //多表
            builder.QueryBody.Joins.Add(new QueryJoin(_dbContext.EntityDescriptors.FirstOrDefault(m => m.TableName == "MyCategory"), "T2"));

            Expression<Func<ArticleEntity, CategoryEntity, dynamic>> exp4 = (m, n) => new { m.Title, n.Name };

            builder.QueryBody.SetSelect(exp4);
            sql = builder.ResolveSelect();

            Assert.Equal("T1.`Title` AS `Title`,T2.`Name` AS `Name`", sql);

            Expression<Func<ArticleEntity, CategoryEntity, dynamic>> exp5 = (m, n) => new { m.Title, n };

            builder.QueryBody.SetSelect(exp5);
            sql = builder.ResolveSelect();

            Assert.Equal("T1.`Title` AS `Title`,T2.`Id` AS `Id`,T2.`Name` AS `Name`,T2.`CreatedBy` AS `CreatedBy`,T2.`Creator` AS `Creator`,T2.`CreatedTime` AS `CreatedTime`,T2.`ModifiedBy` AS `ModifiedBy`,T2.`Modifier` AS `Modifier`,T2.`ModifiedTime` AS `ModifiedTime`", sql);

            Expression<Func<ArticleEntity, CategoryEntity, dynamic>> exp6 = (m, n) => new { m.Title, Name = n.Name.Substring(3, 2) };

            builder.QueryBody.SetSelect(exp6);
            sql = builder.ResolveSelect();
            Assert.Equal("T1.`Title` AS `Title`,SUBSTR(T2.`Name`,4,2) AS `Name`", sql);


            //排除列
            Expression<Func<ArticleEntity, CategoryEntity, dynamic>> exp7 = (m, n) => new { m.Id, m.Title, n };
            Expression<Func<ArticleEntity, CategoryEntity, dynamic>> exp8 = (m, n) => new { m.Title, n.Name };

            builder.QueryBody.SetSelect(exp7);
            builder.QueryBody.SetSelectExclude(exp8);
            sql = builder.ResolveSelect();

            Assert.Equal("T1.`Id` AS `Id`,T2.`Id` AS `Id`,T2.`CreatedBy` AS `CreatedBy`,T2.`Creator` AS `Creator`,T2.`CreatedTime` AS `CreatedTime`,T2.`ModifiedBy` AS `ModifiedBy`,T2.`Modifier` AS `Modifier`,T2.`ModifiedTime` AS `ModifiedTime`", sql);
        }

        #endregion

        #region ==解析表==

        //解析表
        [Fact]
        public void ResolveFormTest()
        {
            //单表
            var builder = CreateBuilder();

            var parameters = new QueryParameters();
            var sqlBuilder = new StringBuilder();
            builder.ResolveFrom(sqlBuilder, parameters);
            var sql = sqlBuilder.ToString();

            Assert.Equal("`Article`", sql);
        }

        //解析表，多表左连接
        [Fact]
        public void ResolveFormTest1()
        {
            var builder = CreateBuilder();
            Expression<Func<ArticleEntity, CategoryEntity, bool>> exp = (m, n) => m.CategoryId == n.Id;
            builder.QueryBody.Joins.Add(new QueryJoin(_dbContext.EntityDescriptors.FirstOrDefault(m => m.TableName == "MyCategory"), "T2", JoinType.Left, exp));

            var parameters = new QueryParameters();
            var sqlBuilder = new StringBuilder();

            builder.ResolveFrom(sqlBuilder, parameters);
            var sql = sqlBuilder.ToString();

            Assert.Equal("`Article` AS T1 LEFT JOIN `MyCategory` AS T2 ON T1.`CategoryId` = T2.`Id`", sql);
        }

        //解析表，多表右连接
        [Fact]
        public void ResolveFormTest2()
        {
            var builder = CreateBuilder();
            Expression<Func<ArticleEntity, CategoryEntity, bool>> exp = (m, n) => m.CategoryId == n.Id;
            builder.QueryBody.Joins.Add(new QueryJoin(_dbContext.EntityDescriptors.FirstOrDefault(m => m.TableName == "MyCategory"), "T2", JoinType.Right, exp));

            var parameters = new QueryParameters();
            var sqlBuilder = new StringBuilder();

            builder.ResolveFrom(sqlBuilder, parameters);
            var sql = sqlBuilder.ToString();

            Assert.Equal("`Article` AS T1 RIGHT JOIN `MyCategory` AS T2 ON T1.`CategoryId` = T2.`Id`", sql);
        }

        //解析表，多表内连接
        [Fact]
        public void ResolveFormTest3()
        {
            var builder = CreateBuilder();
            Expression<Func<ArticleEntity, CategoryEntity, bool>> exp = (m, n) => m.CategoryId == n.Id;
            builder.QueryBody.Joins.Add(new QueryJoin(_dbContext.EntityDescriptors.FirstOrDefault(m => m.TableName == "MyCategory"), "T2", JoinType.Inner, exp));

            var parameters = new QueryParameters();
            var sqlBuilder = new StringBuilder();

            builder.ResolveFrom(sqlBuilder, parameters);
            var sql = sqlBuilder.ToString();

            Assert.Equal("`Article` AS T1 INNER JOIN `MyCategory` AS T2 ON T1.`CategoryId` = T2.`Id`", sql);
        }

        #endregion

        #region ==解析过滤条件==

        [Fact]
        public void ResolveWhereTest()
        {
            var builder = CreateBuilder();

            var parameters = new QueryParameters();
            var sqlBuilder = new StringBuilder();
            builder.ResolveWhere(sqlBuilder, parameters);
            var sql = sqlBuilder.ToString();

            Assert.Equal("WHERE `Deleted` = 0", sql);
        }

        [Fact]
        public void ResolveWhereTest1()
        {
            var builder = CreateBuilder();
            Expression<Func<ArticleEntity, bool>> exp = m => m.Id > 10;

            var parameters = new QueryParameters();
            var sqlBuilder = new StringBuilder();
            builder.QueryBody.SetWhere(exp);
            builder.ResolveWhere(sqlBuilder, parameters);
            var sql = sqlBuilder.ToString();

            Assert.Equal("WHERE `Id` > @P1 AND `Deleted` = 0", sql);
            Assert.Equal(1, parameters.Count);
        }

        #endregion

        #region ==解析表达式==

        /// <summary>
        /// 解析表达式
        /// </summary>
        [Fact]
        public void ResolveExpressionTest()
        {
            var builder = CreateBuilder();
            Expression<Func<ArticleEntity, bool>> exp = m => m.Id > 10;

            var parameters = new QueryParameters();
            var sqlBuilder = new StringBuilder();
            builder.ResolveExpression(exp.Body, exp, sqlBuilder, parameters);
            var sql = sqlBuilder.ToString();

            Assert.Equal("`Id` > @P1", sql);
            Assert.Equal(1, parameters.Count);


            //多表
            builder.QueryBody.Joins.Add(new QueryJoin(_dbContext.EntityDescriptors.FirstOrDefault(m => m.TableName == "MyCategory"), "T2"));
            parameters.Clear();
            sqlBuilder.Clear();
            Expression<Func<ArticleEntity, CategoryEntity, bool>> exp1 = (m, n) => m.Id > 10 && n.Name == "mkh";
            builder.ResolveExpression(exp1.Body, exp1, sqlBuilder, parameters);
            sql = sqlBuilder.ToString();

            Assert.Equal("T1.`Id` > @P1 AND T2.`Name` = @P2", sql);
            Assert.Equal(2, parameters.Count);
            Assert.Equal("mkh", parameters[1].Value);
        }

        /// <summary>
        /// 解析表达式
        /// </summary>
        [Fact]
        public void ResolveExpressionTest1()
        {
            var builder = CreateBuilder();
            Expression<Func<ArticleEntity, bool>> exp = m => m.Id > 10 && m.Id < 20 && m.Title == "123";

            var parameters = new QueryParameters();
            var sqlBuilder = new StringBuilder();
            builder.ResolveExpression(exp.Body, exp, sqlBuilder, parameters);
            var sql = sqlBuilder.ToString();

            Assert.Equal("`Id` > @P1 AND `Id` < @P2 AND `Title` = @P3", sql);
            Assert.Equal(3, parameters.Count);
        }

        /// <summary>
        /// 解析测试字符串的Length属性
        /// </summary>
        [Fact]
        public void ResolveExpressionTest3()
        {
            var builder = CreateBuilder();
            Expression<Func<ArticleEntity, bool>> exp = m => m.Title.Length >= 10;

            var parameters = new QueryParameters();
            var sqlBuilder = new StringBuilder();
            builder.ResolveExpression(exp.Body, exp, sqlBuilder, parameters);
            var sql = sqlBuilder.ToString();

            Assert.Equal("CHAR_LENGTH(`Title`) >= @P1", sql);
            Assert.Equal(1, parameters.Count);
        }

        /// <summary>
        /// 解析测试Substring函数
        /// </summary>
        [Fact]
        public void ResolveExpressionTest4()
        {
            var builder = CreateBuilder();
            Expression<Func<ArticleEntity, bool>> exp = m => m.Title.Substring(3, 3) == "10";

            var parameters = new QueryParameters();
            var sqlBuilder = new StringBuilder();
            builder.ResolveExpression(exp.Body, exp, sqlBuilder, parameters);
            var sql = sqlBuilder.ToString();

            Assert.Equal("SUBSTR(`Title`,4,3) = @P1", sql);
            Assert.Equal(1, parameters.Count);
        }

        /// <summary>
        /// 解析测试Replace函数
        /// </summary>
        [Fact]
        public void ResolveExpressionTest5()
        {
            var builder = CreateBuilder();
            Expression<Func<ArticleEntity, bool>> exp = m => m.Title.Replace("a", "b") == "10";

            var parameters = new QueryParameters();
            var sqlBuilder = new StringBuilder();
            builder.ResolveExpression(exp.Body, exp, sqlBuilder, parameters);
            var sql = sqlBuilder.ToString();

            Assert.Equal("REPLACE(`Title`,'a','b') = @P1", sql);
            Assert.Equal(1, parameters.Count);
        }

        /// <summary>
        /// 解析测试ToUpper和ToLower函数
        /// </summary>
        [Fact]
        public void ResolveExpressionTest6()
        {
            var builder = CreateBuilder();
            Expression<Func<ArticleEntity, bool>> exp = m => m.Title.ToUpper() == "10" || m.Title.ToLower() == "10";

            var parameters = new QueryParameters();
            var sqlBuilder = new StringBuilder();
            builder.ResolveExpression(exp.Body, exp, sqlBuilder, parameters);
            var sql = sqlBuilder.ToString();

            Assert.Equal("UPPER(`Title`) = @P1 OR LOWER(`Title`) = @P2", sql);
            Assert.Equal(2, parameters.Count);
        }

        /// <summary>
        /// 解析测试常量参数
        /// </summary>
        [Fact]
        public void ResolveExpressionTest7()
        {
            var builder = CreateBuilder();

            var id = 10;
            Expression<Func<ArticleEntity, bool>> exp = m => m.Id > id;

            var parameters = new QueryParameters();
            var sqlBuilder = new StringBuilder();
            builder.ResolveExpression(exp.Body, exp, sqlBuilder, parameters);
            var sql = sqlBuilder.ToString();

            Assert.Equal("`Id` > @P1", sql);
            Assert.Equal(1, parameters.Count);
            Assert.Equal(10, parameters[0].Value);
        }

        /// <summary>
        /// 解析测试函数调用
        /// </summary>
        [Fact]
        public void ResolveExpressionTest8()
        {
            var builder = CreateBuilder();

            Expression<Func<ArticleEntity, bool>> exp = m => m.Id > GetId();

            var parameters = new QueryParameters();
            var sqlBuilder = new StringBuilder();
            builder.ResolveExpression(exp.Body, exp, sqlBuilder, parameters);
            var sql = sqlBuilder.ToString();

            Assert.Equal("`Id` > @P1", sql);
            Assert.Equal(1, parameters.Count);
            Assert.Equal(10, parameters[0].Value);
        }

        private int GetId()
        {
            return 10;
        }

        /// <summary>
        /// 解析测试布尔类型
        /// </summary>
        [Fact]
        public void ResolveExpressionTest9()
        {
            var builder = CreateBuilder();

            Expression<Func<ArticleEntity, bool>> exp = m => m.Published == true;

            var parameters = new QueryParameters();
            var sqlBuilder = new StringBuilder();
            builder.ResolveExpression(exp.Body, exp, sqlBuilder, parameters);
            var sql = sqlBuilder.ToString();

            Assert.Equal("`IsPublished` = @P1", sql);
            Assert.Equal(1, parameters.Count);
            Assert.Equal(true, parameters[0].Value);
        }

        /// <summary>
        /// 解析测试布尔类型
        /// </summary>
        [Fact]
        public void ResolveExpressionTest10()
        {
            var builder = CreateBuilder();

            Expression<Func<ArticleEntity, bool>> exp = m => m.Published;

            var parameters = new QueryParameters();
            var sqlBuilder = new StringBuilder();
            builder.ResolveExpression(exp.Body, exp, sqlBuilder, parameters);
            var sql = sqlBuilder.ToString();

            Assert.Equal("`IsPublished` = @P1", sql);
            Assert.Equal(1, parameters.Count);
            Assert.Equal("1", parameters[0].Value);
        }

        /// <summary>
        /// 解析测试布尔类型
        /// </summary>
        [Fact]
        public void ResolveExpressionTest11()
        {
            var builder = CreateBuilder();

            Expression<Func<ArticleEntity, bool>> exp = m => m.Published && m.Id > 10;

            var parameters = new QueryParameters();
            var sqlBuilder = new StringBuilder();
            builder.ResolveExpression(exp.Body, exp, sqlBuilder, parameters);
            var sql = sqlBuilder.ToString();

            Assert.Equal("`IsPublished` = @P1 AND `Id` > @P2", sql);
            Assert.Equal(2, parameters.Count);
            Assert.Equal(10, parameters[1].Value);
        }

        /// <summary>
        /// 解析测试布尔类型
        /// </summary>
        [Fact]
        public void ResolveExpressionTest12()
        {
            var builder = CreateBuilder();

            Expression<Func<ArticleEntity, bool>> exp = m => !m.Published && m.Id > 10;

            var parameters = new QueryParameters();
            var sqlBuilder = new StringBuilder();
            builder.ResolveExpression(exp.Body, exp, sqlBuilder, parameters);
            var sql = sqlBuilder.ToString();

            Assert.Equal("`IsPublished` = @P1 AND `Id` > @P2", sql);
            Assert.Equal(2, parameters.Count);
            Assert.Equal(10, parameters[1].Value);
        }

        /// <summary>
        /// 解析集合类型的Contains函数
        /// </summary>
        [Fact]
        public void ResolveExpressionTest13()
        {
            var builder = CreateBuilder();

            var ids = new List<int> { 10, 15 };

            Expression<Func<ArticleEntity, bool>> exp = m => ids.Contains(m.Id);

            var parameters = new QueryParameters();
            var sqlBuilder = new StringBuilder();
            builder.ResolveExpression(exp.Body, exp, sqlBuilder, parameters);
            var sql = sqlBuilder.ToString();

            Assert.Equal("`Id` IN (10,15)", sql);
        }

        /// <summary>
        /// 解析数组类型的Contains函数
        /// </summary>
        [Fact]
        public void ResolveExpressionTest14()
        {
            var builder = CreateBuilder();

            var ids = new[] { 10, 15 };

            Expression<Func<ArticleEntity, bool>> exp = m => ids.Contains(m.Id);

            var parameters = new QueryParameters();
            var sqlBuilder = new StringBuilder();
            builder.ResolveExpression(exp.Body, exp, sqlBuilder, parameters);
            var sql = sqlBuilder.ToString();

            Assert.Equal("`Id` IN (10,15)", sql);
        }

        /// <summary>
        /// 解析字符串类型的Contains函数
        /// </summary>
        [Fact]
        public void ResolveExpressionTest15()
        {
            var builder = CreateBuilder();

            Expression<Func<ArticleEntity, bool>> exp = m => m.Title.Contains("mkh");

            var parameters = new QueryParameters();
            var sqlBuilder = new StringBuilder();
            builder.ResolveExpression(exp.Body, exp, sqlBuilder, parameters);
            var sql = sqlBuilder.ToString();

            Assert.Equal("`Title` LIKE @P1", sql);
            Assert.Equal("%mkh%", parameters[0].Value);
        }

        /// <summary>
        /// 解析StartsWith函数
        /// </summary>
        [Fact]
        public void ResolveExpressionTest16()
        {
            var builder = CreateBuilder();

            Expression<Func<ArticleEntity, bool>> exp = m => m.Title.StartsWith("mkh");

            var parameters = new QueryParameters();
            var sqlBuilder = new StringBuilder();
            builder.ResolveExpression(exp.Body, exp, sqlBuilder, parameters);
            var sql = sqlBuilder.ToString();

            Assert.Equal("`Title` LIKE @P1", sql);
            Assert.Equal("mkh%", parameters[0].Value);
        }

        /// <summary>
        /// 解析StartsWith函数
        /// </summary>
        [Fact]
        public void ResolveExpressionTest17()
        {
            var builder = CreateBuilder();

            Expression<Func<ArticleEntity, bool>> exp = m => m.Title.EndsWith("mkh");

            var parameters = new QueryParameters();
            var sqlBuilder = new StringBuilder();
            builder.ResolveExpression(exp.Body, exp, sqlBuilder, parameters);
            var sql = sqlBuilder.ToString();

            Assert.Equal("`Title` LIKE @P1", sql);
            Assert.Equal("%mkh", parameters[0].Value);
        }

        /// <summary>
        /// 解析Equals函数
        /// </summary>
        [Fact]
        public void ResolveExpressionTest18()
        {
            var builder = CreateBuilder();

            Expression<Func<ArticleEntity, bool>> exp = m => m.Title.Equals("mkh");

            var parameters = new QueryParameters();
            var sqlBuilder = new StringBuilder();
            builder.ResolveExpression(exp.Body, exp, sqlBuilder, parameters);
            var sql = sqlBuilder.ToString();

            Assert.Equal("`Title` = @P1", sql);
            Assert.Equal("mkh", parameters[0].Value);
        }

        /// <summary>
        /// 解析数组类型的NotContains函数
        /// </summary>
        [Fact]
        public void ResolveExpressionTest19()
        {
            var builder = CreateBuilder();

            var ids = new[] { 10, 15 };

            Expression<Func<ArticleEntity, bool>> exp = m => ids.NotContains(m.Id);

            var parameters = new QueryParameters();
            var sqlBuilder = new StringBuilder();
            builder.ResolveExpression(exp.Body, exp, sqlBuilder, parameters);
            var sql = sqlBuilder.ToString();

            Assert.Equal("`Id` NOT IN (10,15)", sql);
        }

        /// <summary>
        /// 解析集合类型的NotContains函数
        /// </summary>
        [Fact]
        public void ResolveExpressionTest20()
        {
            var builder = CreateBuilder();

            var ids = new List<int> { 10, 15 };

            Expression<Func<ArticleEntity, bool>> exp = m => ids.NotContains(m.Id);

            var parameters = new QueryParameters();
            var sqlBuilder = new StringBuilder();
            builder.ResolveExpression(exp.Body, exp, sqlBuilder, parameters);
            var sql = sqlBuilder.ToString();

            Assert.Equal("`Id` NOT IN (10,15)", sql);
        }

        #endregion

    }
}
