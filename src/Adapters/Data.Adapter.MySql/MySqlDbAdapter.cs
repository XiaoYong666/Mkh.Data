using System;
using System.Data;
using System.Linq.Expressions;
using System.Text;
using Mkh.Data.Abstractions.Adapter;
using Mkh.Data.Abstractions.Descriptors;
using MySql.Data.MySqlClient;

namespace Mkh.Data.Adapter.MySql
{
    public class MySqlDbAdapter : DbAdapterAbstract
    {
        public override DbProvider Provider => DbProvider.MySql;

        /// <summary>
        /// 左引号
        /// </summary>
        public override char LeftQuote => '`';

        /// <summary>
        /// 右引号
        /// </summary>
        public override char RightQuote => '`';

        /// <summary>
        /// 获取最后新增ID语句
        /// </summary>
        public override string IdentitySql => "SELECT LAST_INSERT_ID() ID;";

        public override IDbConnection NewConnection(string connectionString)
        {
            return new MySqlConnection(connectionString);
        }

        public override string GenerateFirstSql(string version, string @select, string table, string @where, string sort, string groupBy = null,
            string having = null)
        {
            return GeneratePagingSql(version, select, table, where, sort, 0, 1, groupBy, having);
        }

        public override string GeneratePagingSql(string version, string select, string table, string where, string sort, int skip, int take, string groupBy = null, string having = null)
        {
            var sqlBuilder = new StringBuilder();
            sqlBuilder.AppendFormat("SELECT {0} FROM {1}", select, table);
            if (where.NotNull())
                sqlBuilder.AppendFormat(" WHERE {0}", where);

            if (groupBy.NotNull())
                sqlBuilder.Append(groupBy);

            if (having.NotNull())
                sqlBuilder.Append(having);

            if (sort.NotNull())
                sqlBuilder.AppendFormat(" ORDER BY {0}", sort);

            sqlBuilder.AppendFormat(" LIMIT {0},{1}", skip, take);
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
            var methodName = methodCallExpression.Method.Name;
            if (methodName == "Substring")
            {
                var start = ((ConstantExpression)methodCallExpression.Arguments[0]).Value.ToInt() + 1;
                if (methodCallExpression.Arguments.Count > 1)
                {
                    var length = ((ConstantExpression)methodCallExpression.Arguments[1]).Value.ToInt();
                    return $"SUBSTR({columnName},{start},{length})";
                }

                return $"SUBSTR({columnName},{start})";
            }

            if (methodName == "ToString")
            {
                if (methodCallExpression.Object!.Type.IsDateTime())
                {
                    var arg = ((ConstantExpression)methodCallExpression.Arguments[0]).Value;
                    if (arg != null)
                    {
                        var format = arg.ToString();
                        format = format!.Replace("YYYY", "%Y")
                            .Replace("YY", "%y")
                            .Replace("MM", "%m")
                            .Replace("DD", "%d")
                            .Replace("HH", "%k")
                            .Replace("mm", "%i")
                            .Replace("ss", "%s");

                        return $"DATE_FORMAT({columnName},'{format}')";
                    }
                }
            }

            if (methodName == "Replace")
            {
                var oldValue = ((ConstantExpression)methodCallExpression.Arguments[0]).Value;
                var newValue = ((ConstantExpression)methodCallExpression.Arguments[1]).Value;

                return $"REPLACE({columnName},'{oldValue}','{newValue}')";
            }

            if (methodName == "ToLower")
            {
                return $"LOWER({columnName})";
            }

            if (methodName == "ToUpper")
            {
                return $"UPPER({columnName})";
            }

            return string.Empty;
        }

        public override string Property2Func(string propertyName, string columnName)
        {
            switch (propertyName)
            {
                case "Length":
                    return $"CHAR_LENGTH({columnName})";
                case "Count":
                    return "COUNT(0)";
                case "Sum":
                case "Avg":
                case "Max":
                case "Min":
                    return $"{propertyName.ToUpper()}({columnName})";
                default:
                    return string.Empty;
            }
        }
    }
}
