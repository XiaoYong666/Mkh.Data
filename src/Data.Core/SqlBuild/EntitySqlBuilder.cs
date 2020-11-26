using System.Linq;
using System.Text;
using Mkh.Data.Abstractions.Adapter;
using Mkh.Data.Abstractions.Descriptors;
using Mkh.Data.Core.Descriptors;
using Mkh.Data.Core.Extensions;

namespace Mkh.Data.Core.SqlBuild
{
    /// <summary>
    /// 实体基本CRUD的SQL语句构造器
    /// </summary>
    internal class EntitySqlBuilder
    {
        public static EntitySqlDescriptor Build(IEntityDescriptor entityDescriptor)
        {
            var sqlDescriptor = new EntitySqlDescriptor(entityDescriptor.DbContext.Adapter, entityDescriptor.TableName);

            BuildInsertSql(entityDescriptor, sqlDescriptor);
            BuildDeleteSql(entityDescriptor, sqlDescriptor);
            BuildSoftDeleteSql(entityDescriptor, sqlDescriptor);
            BuildUpdateSql(entityDescriptor, sqlDescriptor);
            BuildQuerySql(entityDescriptor, sqlDescriptor);
            BuildExistsSql(entityDescriptor, sqlDescriptor);

            return sqlDescriptor;
        }

        #region ==私有方法==

        /// <summary>
        /// 生成插入语句
        /// </summary>
        private static void BuildInsertSql(IEntityDescriptor descriptor, EntitySqlDescriptor sql)
        {
            var sb = new StringBuilder();
            sb.Append("INSERT INTO {0} ");
            sb.Append("(");

            var valuesSql = new StringBuilder();
            var primaryKey = descriptor.PrimaryKey;
            var dbAdapter = descriptor.DbContext.Adapter;

            foreach (var col in descriptor.Columns)
            {
                //排除自增主键
                if (col.IsPrimaryKey && (primaryKey.IsInt || primaryKey.IsLong))
                    continue;

                dbAdapter.AppendQuote(sb, col.Name);
                sb.Append(",");

                dbAdapter.AppendParameter(valuesSql, col.PropertyName);

                //针对PostgreSQL数据库的json和jsonb类型字段的处理
                if (dbAdapter.Provider == DbProvider.PostgreSQL)
                {
                    if (col.TypeName.EqualsIgnoreCase("jsonb"))
                    {
                        valuesSql.Append("::jsonb");
                    }
                    else if (col.TypeName.EqualsIgnoreCase("json"))
                    {
                        valuesSql.Append("::json");
                    }
                }

                valuesSql.Append(",");
            }

            //删除最后一个","
            sb.Remove(sb.Length - 1, 1);

            sb.Append(") VALUES");

            //设置批量删除
            sql.SetBatchAdd(sb.ToString());

            sb.Append("(");

            //删除最后一个","
            if (valuesSql.Length > 0)
                valuesSql.Remove(valuesSql.Length - 1, 1);

            sb.Append(valuesSql);
            sb.Append(")");

            if (dbAdapter.Provider != DbProvider.PostgreSQL)
            {
                sb.Append(";");
            }

            sql.SetAdd(sb.ToString());
        }

        /// <summary>
        /// 设置删除语句
        /// </summary>
        private static void BuildDeleteSql(IEntityDescriptor descriptor, EntitySqlDescriptor sql)
        {
            var deleteSql = "DELETE FROM {0} ";
            sql.SetDelete(deleteSql);

            var primaryKey = descriptor.PrimaryKey;
            if (!primaryKey.IsNo)
            {
                var dbAdapter = descriptor.DbContext.Adapter;
                sql.SetDeleteSingle($"{deleteSql} WHERE {dbAdapter.AppendQuote(primaryKey.ColumnName)}={dbAdapter.AppendParameter(primaryKey.PropertyName)};");
            }
        }

        /// <summary>
        /// 设置软删除
        /// </summary>
        private static void BuildSoftDeleteSql(IEntityDescriptor descriptor, EntitySqlDescriptor sql)
        {
            if (!descriptor.IsSoftDelete)
                return;

            var dbAdapter = descriptor.DbContext.Adapter;
            var sb = new StringBuilder("UPDATE {0} SET ");
            sb.AppendFormat("{0}={1},", dbAdapter.AppendQuote(descriptor.GetDeletedColumnName()), dbAdapter.BooleanTrueValue);
            sb.AppendFormat("{0}={1},", dbAdapter.AppendQuote(descriptor.GetDeletedTimeColumnName()), dbAdapter.AppendParameter("DeletedTime"));
            sb.AppendFormat("{0}={1} ", dbAdapter.AppendQuote(descriptor.GetDeletedByColumnName()), dbAdapter.AppendParameter("DeletedBy"));

            sql.SetSoftDelete(sb.ToString());

            var primaryKey = descriptor.PrimaryKey;
            sb.AppendFormat(" WHERE {0}={1};", dbAdapter.AppendQuote(primaryKey.ColumnName), dbAdapter.AppendParameter(primaryKey.PropertyName));
            sql.SetSoftDeleteSingle(sb.ToString());
        }

        /// <summary>
        /// 设置更新语句
        /// </summary>
        private static void BuildUpdateSql(IEntityDescriptor descriptor, EntitySqlDescriptor sql)
        {
            var sb = new StringBuilder();
            sb.Append("UPDATE {0} SET");

            sql.SetUpdate(sb.ToString());

            var dbAdapter = descriptor.DbContext.Adapter;
            var primaryKey = descriptor.PrimaryKey;
            if (!primaryKey.IsNo)
            {
                var columns = descriptor.Columns.Where(m => !m.IsPrimaryKey);

                foreach (var col in columns)
                {
                    sb.AppendFormat("{0}={1}", dbAdapter.AppendQuote(col.Name), dbAdapter.AppendParameter(col.PropertyName));

                    //针对PostgreSQL数据库的json和jsonb类型字段的处理
                    if (dbAdapter.Provider == DbProvider.PostgreSQL)
                    {
                        if (col.TypeName.EqualsIgnoreCase("jsonb"))
                        {
                            sb.Append("::jsonb");
                        }
                        else if (col.TypeName.EqualsIgnoreCase("json"))
                        {
                            sb.Append("::json");
                        }
                    }

                    sb.Append(",");
                }

                sb.Remove(sb.Length - 1, 1);

                sb.AppendFormat(" WHERE {0}={1};", dbAdapter.AppendQuote(primaryKey.ColumnName), dbAdapter.AppendParameter(primaryKey.PropertyName));

                sql.SetUpdateSingle(sb.ToString());
            }
        }

        /// <summary>
        /// 设置查询语句
        /// </summary>
        private static void BuildQuerySql(IEntityDescriptor descriptor, EntitySqlDescriptor sql)
        {
            var dbAdapter = descriptor.DbContext.Adapter;
            var sb = new StringBuilder("SELECT ");
            for (var i = 0; i < descriptor.Columns.Count; i++)
            {
                var col = descriptor.Columns[i];
                sb.AppendFormat("{0} AS {1}", dbAdapter.AppendQuote(col.Name), dbAdapter.AppendQuote(col.PropertyName));

                if (i != descriptor.Columns.Count - 1)
                {
                    sb.Append(",");
                }
            }
            sb.Append(" FROM {0} ");

            var querySql = sb.ToString();
            var getSql = querySql;
            var getAndRowLockSql = querySql;
            var getAndNoLockSql = querySql;
            // SqlServer行锁
            if (dbAdapter.Provider == DbProvider.SqlServer)
            {
                getAndRowLockSql += " WITH (ROWLOCK, UPDLOCK) ";
                getAndNoLockSql += "WITH (NOLOCK) ";
            }

            var primaryKey = descriptor.PrimaryKey;
            if (!primaryKey.IsNo)
            {
                var appendSql = $" WHERE {dbAdapter.AppendQuote(primaryKey.ColumnName)}={dbAdapter.AppendParameter(primaryKey.PropertyName)} ";
                getSql += appendSql;
                getAndRowLockSql += appendSql;
                getAndNoLockSql += appendSql;

                //过滤软删除
                if (descriptor.IsSoftDelete)
                {
                    appendSql = $" AND {dbAdapter.AppendQuote(descriptor.GetDeletedColumnName())}={dbAdapter.BooleanFalseValue} ";
                    getSql += appendSql;
                    getAndRowLockSql += appendSql;
                    getAndNoLockSql += appendSql;
                }

                //MySql和PostgreSQL行锁
                if (dbAdapter.Provider == DbProvider.MySql || dbAdapter.Provider == DbProvider.PostgreSQL)
                {
                    getAndRowLockSql += " FOR UPDATE;";
                }
            }

            sql.SetGet(getSql);
            sql.SetGetAndRowLock(getAndRowLockSql);
            sql.SetGetAndNoLock(getAndNoLockSql);
        }

        /// <summary>
        /// 设置是否存在语句
        /// </summary>
        /// <returns></returns>
        private static void BuildExistsSql(IEntityDescriptor descriptor, EntitySqlDescriptor sqlDescriptor)
        {
            var primaryKey = descriptor.PrimaryKey;
            //没有主键，无法使用该方法
            if (primaryKey.IsNo)
                return;

            var dbAdapter = descriptor.DbContext.Adapter;
            var sql = $"SELECT 1 FROM {{0}} WHERE {dbAdapter.AppendQuote(primaryKey.ColumnName)}={dbAdapter.AppendParameter(primaryKey.PropertyName)}";
            if (descriptor.IsSoftDelete)
            {
                sql += $" AND {dbAdapter.AppendQuote(descriptor.GetDeletedColumnName())}={dbAdapter.BooleanFalseValue} ";
            }

            if (dbAdapter.Provider == DbProvider.MySql)
            {
                sql += " LIMIT 1";
            }

            sqlDescriptor.SetExists(sql);
        }

        #endregion
    }
}
