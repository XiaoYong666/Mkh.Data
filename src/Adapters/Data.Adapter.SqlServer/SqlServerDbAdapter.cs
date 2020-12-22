using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq.Expressions;
using System.Text;
using Mkh.Data.Abstractions.Adapter;
using Mkh.Data.Abstractions.Descriptors;

namespace Mkh.Data.Adapter.SqlServer
{
    public class SqlServerDbAdapter : DbAdapterAbstract
    {
        public override DbProvider Provider => DbProvider.MySql;

        /// <summary>
        /// 左引号
        /// </summary>
        public override char LeftQuote => '[';

        /// <summary>
        /// 右引号
        /// </summary>
        public override char RightQuote => ']';

        /// <summary>
        /// 获取最后新增ID语句
        /// </summary>
        public override string IdentitySql => "SELECT SCOPE_IDENTITY() ID;";

        public override IDbConnection NewConnection(string connectionString)
        {
            return new SqlConnection(connectionString);
        }

        public override string GenerateFirstSql(string version, string @select, string table, string @where, string sort, string groupBy = null, string having = null)
        {
            return GeneratePagingSql(version, select, table, where, sort, 0, 1, groupBy, having);
        }

        public override string GeneratePagingSql(string version, string select, string table, string where, string sort, int skip, int take, string groupBy = null, string having = null)
        {
            var sqlBuilder = new StringBuilder();

            if (version.IsNull() || version.ToInt() >= 2012)
            {
                #region ==2012+版本==

                sqlBuilder.AppendFormat("SELECT {0} FROM {1}", @select, table);
                if (!string.IsNullOrWhiteSpace(where))
                    sqlBuilder.AppendFormat(" WHERE {0}", @where);

                if (groupBy.NotNull())
                    sqlBuilder.Append(groupBy);

                if (having.NotNull())
                    sqlBuilder.Append(having);

                sqlBuilder.AppendFormat(" ORDER BY {0} OFFSET {1} ROW FETCH NEXT {2} ROW ONLY", sort, skip, take);

                #endregion
            }
            else
            {
                #region ==2018及以下版本==

                sqlBuilder.AppendFormat("SELECT * FROM (SELECT ROW_NUMBER() OVER(ORDER BY {0}) AS RowNum,{1} FROM {2}", sort, @select, table);
                if (!string.IsNullOrWhiteSpace(where))
                    sqlBuilder.AppendFormat(" WHERE {0}", @where);

                sqlBuilder.AppendFormat(") AS T WHERE T.RowNum BETWEEN {0} AND {1}", skip, skip + take);

                #endregion
            }

            return sqlBuilder.ToString();
        }

        public override void ResolveColumn(IColumnDescriptor columnDescriptor)
        {
            var propertyType = columnDescriptor.PropertyInfo.PropertyType;
            var isNullable = propertyType.IsNullable();
            if (isNullable)
            {
                propertyType = Nullable.GetUnderlyingType(propertyType);
                if (propertyType == null)
                    throw new Exception("Property2Column error");
            }

            if (propertyType.IsEnum)
            {
                if (!isNullable)
                {
                    columnDescriptor.DefaultValue = "DEFAULT 0";
                }

                columnDescriptor.TypeName = "SMALLINT(3)";
                return;
            }

            if (propertyType.IsGuid())
            {
                columnDescriptor.TypeName = "CHAR(36)";

                return;
            }

            var typeCode = Type.GetTypeCode(propertyType);
            if (typeCode == TypeCode.Char || typeCode == TypeCode.String)
            {
                if (columnDescriptor.Length < 1)
                    columnDescriptor.TypeName = "TEXT";

                columnDescriptor.TypeName = $"VARCHAR({columnDescriptor.Length})";
                return;
            }

            if (typeCode == TypeCode.Boolean)
            {
                if (!isNullable)
                {
                    columnDescriptor.DefaultValue = "DEFAULT 0";
                }

                columnDescriptor.TypeName = "BIT";

                return;
            }

            if (typeCode == TypeCode.Byte)
            {
                if (!isNullable)
                {
                    columnDescriptor.DefaultValue = "DEFAULT 0";
                }
                columnDescriptor.TypeName = "TINYINT(1)";

                return;
            }

            if (typeCode == TypeCode.Int16 || typeCode == TypeCode.Int32)
            {
                if (!isNullable)
                {
                    columnDescriptor.DefaultValue = "DEFAULT 0";
                }

                columnDescriptor.TypeName = "INT";

                return;
            }

            if (typeCode == TypeCode.Int64)
            {
                if (!isNullable)
                {
                    columnDescriptor.DefaultValue = "DEFAULT 0";
                }

                columnDescriptor.TypeName = "BIGINT";

                return;
            }

            if (typeCode == TypeCode.DateTime)
            {
                if (!isNullable)
                {
                    columnDescriptor.DefaultValue = "DEFAULT CURRENT_TIMESTAMP(0)";
                }
                columnDescriptor.TypeName = "DATETIME(0)";

                return;
            }

            if (typeCode == TypeCode.Decimal)
            {
                if (!isNullable)
                {
                    columnDescriptor.DefaultValue = "DEFAULT 0";
                }

                var m = columnDescriptor.PrecisionM < 1 ? 18 : columnDescriptor.PrecisionM;
                var d = columnDescriptor.PrecisionD < 1 ? 4 : columnDescriptor.PrecisionD;

                columnDescriptor.TypeName = $"DECIMAL({m},{d})";
                return;
            }

            if (typeCode == TypeCode.Double)
            {
                if (!isNullable)
                {
                    columnDescriptor.DefaultValue = "DEFAULT 0";
                }

                var m = columnDescriptor.PrecisionM < 1 ? 18 : columnDescriptor.PrecisionM;
                var d = columnDescriptor.PrecisionD < 1 ? 4 : columnDescriptor.PrecisionD;

                columnDescriptor.TypeName = $"DOUBLE({m},{d})";

                return;
            }

            if (typeCode == TypeCode.Single)
            {
                if (!isNullable)
                {
                    columnDescriptor.DefaultValue = "DEFAULT 0";
                }

                var m = columnDescriptor.PrecisionM < 1 ? 18 : columnDescriptor.PrecisionM;
                var d = columnDescriptor.PrecisionD < 1 ? 4 : columnDescriptor.PrecisionD;

                columnDescriptor.TypeName = $"FLOAT({m},{d})";
            }
        }

        public override string Method2Func(MethodCallExpression methodCallExpression, string columnName)
        {
            throw new NotImplementedException();
        }

        public override string Property2Func(string methodName, string columnName)
        {
            throw new NotImplementedException();
        }
    }
}
