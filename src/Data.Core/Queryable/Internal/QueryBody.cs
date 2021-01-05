using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
#if DEBUG
using System.Runtime.CompilerServices;
#endif
using Mkh.Data.Abstractions;
using Mkh.Data.Abstractions.Adapter;
using Mkh.Data.Abstractions.Descriptors;
using Mkh.Data.Abstractions.Pagination;

#if DEBUG
[assembly: InternalsVisibleTo("Data.Adapter.MySql.Test")]
#endif
namespace Mkh.Data.Core.Queryable.Internal
{
    /// <summary>
    /// 查询主体信息
    /// </summary>
    internal class QueryBody
    {
        #region ==字段==

        private readonly IDbAdapter _dbAdapter;

        #endregion

        #region ==属性==

        /// <summary>
        /// 仓储
        /// </summary>
        public IRepository Repository { get; set; }

        /// <summary>
        /// 查询的列
        /// </summary>
        public QuerySelect Select { get; } = new QuerySelect();

        /// <summary>
        /// 表连接信息
        /// </summary>
        public List<QueryJoin> Joins { get; } = new List<QueryJoin>();

        /// <summary>
        /// 过滤条件
        /// </summary>
        public List<QueryWhere> Wheres { get; } = new List<QueryWhere>();

        /// <summary>
        /// 排序
        /// </summary>
        public List<QuerySort> Sorts { get; } = new List<QuerySort>();

        /// <summary>
        /// 更新信息
        /// </summary>
        public QueryUpdate Update { get; } = new QueryUpdate();

        /// <summary>
        /// 分组信息
        /// </summary>
        public List<QueryGroupBy> GroupBys { get; set; }

        /// <summary>
        /// 跳过行数
        /// </summary>
        public int Skip { get; set; }

        /// <summary>
        /// 取行数
        /// </summary>
        public int Take { get; set; }

        /// <summary>
        /// 过滤已删除的
        /// </summary>
        public bool FilterDeleted { get; set; } = true;

        /// <summary>
        /// 过滤租户
        /// </summary>
        public bool FilterTenant { get; set; } = true;

        /// <summary>
        /// 是否分组查询
        /// </summary>
        public bool IsGroupBy { get; set; }

        #endregion

        #region ==构造函数

        public QueryBody(IRepository repository)
        {
            Repository = repository;
            _dbAdapter = repository.DbContext.Adapter;
        }

        #endregion

        #region ==方法==

        #region ==设置排序==

        /// <summary>
        /// 设置排序
        /// </summary>
        /// <param name="field"></param>
        /// <param name="sortType"></param>
        public void SetSort(string field, SortType sortType)
        {
            if (field.IsNull())
                return;

            Sorts.Add(new QuerySort { Mode = QuerySortMode.Sql, Sql = field, Type = sortType });
        }


        /// <summary>
        /// 设置排序
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="sortType"></param>
        public void SetSort(LambdaExpression expression, SortType sortType)
        {
            if (expression == null)
                return;

            Sorts.Add(new QuerySort { Mode = QuerySortMode.Lambda, Lambda = expression, Type = sortType });
        }

        #endregion

        #region ==设置条件==

        public void SetWhere(LambdaExpression whereExpression)
        {
            if (whereExpression != null)
            {
                Wheres.Add(new QueryWhere(whereExpression));
            }
        }

        public void SetWhere(string whereSql)
        {
            if (whereSql.NotNull())
            {
                Wheres.Add(new QueryWhere(whereSql));
            }
        }

        public void SetWhere(LambdaExpression expression, string queryOperator, Abstractions.Queryable.IQueryable subQueryable)
        {
            if (subQueryable != null)
            {
                Wheres.Add(new QueryWhere(expression, queryOperator, subQueryable));
            }
        }

        #endregion

        #region ==设置选择列==

        /// <summary>
        /// 设置列
        /// </summary>
        /// <param name="selectExpression"></param>
        public void SetSelect(LambdaExpression selectExpression)
        {
            if (selectExpression != null)
            {
                Select.Mode = QuerySelectMode.Lambda;
                Select.Include = selectExpression;
            }
        }

        /// <summary>
        /// 设置列
        /// </summary>
        /// <param name="sql"></param>
        public void SetSelect(string sql)
        {
            if (sql.NotNull())
            {
                Select.Mode = QuerySelectMode.Sql;
                Select.Sql = sql;
            }
        }

        /// <summary>
        /// 设置函数选择列
        /// </summary>
        /// <param name="functionExpression">函数表达式</param>
        /// <param name="functionName">函数名称</param>
        public void SetFunctionSelect(LambdaExpression functionExpression, string functionName)
        {
            if (functionExpression != null && functionName != null)
            {
                Select.Mode = QuerySelectMode.Function;
                Select.FunctionExpression = functionExpression;
                Select.FunctionName = functionName;
            }
        }

        /// <summary>
        /// 设置排除列
        /// </summary>
        /// <param name="excludeExpression"></param>
        public void SetSelectExclude(LambdaExpression excludeExpression)
        {
            Select.Exclude = excludeExpression;
        }

        #endregion

        #region ==设置分页==

        public void SetLimit(int skip, int take)
        {
            Skip = skip < 0 ? 0 : skip;
            Take = take < 0 ? 0 : take;
        }

        #endregion

        #region ==设置更新==

        public void SetUpdate(LambdaExpression expression)
        {
            Update.Mode = QueryUpdateMode.Lambda;
            Update.Lambda = expression;
        }

        public void SetUpdate(string sql)
        {
            Update.Mode = QueryUpdateMode.Sql;
            Update.Sql = sql;
        }

        #endregion

        #region ==获取列名==

        /// <summary>
        /// 获取列名
        /// </summary>
        public string GetColumnName(string fieldName, QueryJoin join)
        {
            var col = GetColumnDescriptor(fieldName, join);

            //只有一个实体的时候，不需要别名
            if (Joins.Count == 1)
            {
                return _dbAdapter.AppendQuote(col.Name);
            }

            return $"{join.Alias}.{_dbAdapter.AppendQuote(col.Name)}";
        }

        /// <summary>
        /// 从成员表达式中获取列名
        /// </summary>
        /// <param name="exp"></param>
        /// <param name="lambda"></param>
        /// <returns></returns>
        public string GetColumnName(MemberExpression exp, LambdaExpression lambda)
        {
            var join = GetJoin(exp, lambda);
            return GetColumnName(exp.Member.Name, join);
        }

        #endregion

        #region ==获取列信息==

        /// <summary>
        /// 获取列描述信息
        /// </summary>
        /// <param name="name"></param>
        /// <param name="join"></param>
        /// <returns></returns>
        public IColumnDescriptor GetColumnDescriptor(string name, QueryJoin join)
        {
            var col = join.EntityDescriptor.Columns.FirstOrDefault(m => m.PropertyInfo.Name.Equals(name));

            Check.NotNull(col, nameof(col), $"({name})列不存在");

            return col;
        }

        /// <summary>
        /// 获取列描述
        /// </summary>
        /// <param name="exp"></param>
        /// <param name="lambda"></param>
        /// <returns></returns>
        public IColumnDescriptor GetColumnDescriptor(MemberExpression exp, LambdaExpression lambda)
        {
            var join = GetJoin(exp, lambda);
            return GetColumnDescriptor(exp.Member.Name, join);
        }


        #endregion

        #region ==获取表连接信息==

        /// <summary>
        /// 根据指定条件获取表连接信息
        /// </summary>
        /// <param name="exp"></param>
        /// <param name="lambda"></param>
        /// <returns></returns>
        public QueryJoin GetJoin(MemberExpression exp, LambdaExpression lambda)
        {
            var index = 0;
            var memberParameter = exp.Expression as ParameterExpression;
            if (memberParameter == null)
                return null;

            foreach (var parameter in lambda.Parameters)
            {
                if (parameter.Name!.Equals(memberParameter.Name))
                    break;
                index++;
            }
            return Joins[index];
        }

        #endregion

        #endregion
    }
}
