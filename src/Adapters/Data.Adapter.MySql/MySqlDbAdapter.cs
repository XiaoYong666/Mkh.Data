using System;
using System.Data;
using System.Text;
using Mkh.Data.Abstractions.Adapter;
using Mkh.Data.Abstractions.Adapter.SqlBuildModels;
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

        /// <summary>
        /// 长度函数
        /// </summary>
        public override string FuncLength => "CHAR_LENGTH";

        public override IDbConnection NewConnection(string connectionString)
        {
            return new MySqlConnection(connectionString);
        }

        private string GenerateQuerySql(string select, string table, string where, string sort, int skip, int take, string groupBy = null, string having = null)
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

        public override string GeneratePagingSql(PagingSqlBuildModel model)
        {
            return GenerateQuerySql(model.Select, model.TableName, model.Where, model.Sort, model.Skip, model.Take, model.GroupBy, model.Having);
        }

        public override string GenerateFirstSql(FirstSqlBuildModel model)
        {
            return GenerateQuerySql(model.Select, model.TableName, model.Where, model.Sort, 0, 1, model.GroupBy, model.Having);
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
    }
}
