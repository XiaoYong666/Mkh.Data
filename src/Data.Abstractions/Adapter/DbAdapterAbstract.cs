using System;
using System.Data;
using System.Reflection;
using System.Text;
using Mkh.Data.Abstractions.Adapter.SqlBuildModels;
using Mkh.Data.Abstractions.Descriptors;

namespace Mkh.Data.Abstractions.Adapter
{
    /// <summary>
    /// 数据库适配器抽象类
    /// </summary>
    public abstract class DbAdapterAbstract : IDbAdapter
    {
        public abstract DbProvider Provider { get; }

        public virtual char LeftQuote => '"';

        public virtual char RightQuote => '"';

        public virtual char ParameterPrefix => '@';

        public virtual string IdentitySql => "";

        public virtual string FuncSubstring => "SUBSTR";

        public virtual string FuncLength => "";

        public virtual string FuncLower => "LOWER";

        public virtual string FuncUpper => "UPPER";

        public virtual bool SqlLowerCase => false;

        public virtual string BooleanTrueValue => "1";

        public virtual string BooleanFalseValue => "0";

        public string AppendQuote(string value)
        {
            var val = value?.Trim();
            if (val != null && SqlLowerCase)
                val = val.ToLower();

            return $"{LeftQuote}{val}{RightQuote}";
        }

        public void AppendQuote(StringBuilder sb, string value)
        {
            sb.Append(AppendQuote(value));
        }

        public string AppendParameter(string parameterName)
        {
            return $"{ParameterPrefix}{parameterName}";
        }

        public void AppendParameter(StringBuilder sb, string parameterName)
        {
            sb.Append(AppendParameter(parameterName));
        }

        public abstract IDbConnection NewConnection(string connectionString);

        public abstract string GeneratePagingSql(PagingSqlBuildModel model);

        public abstract string GenerateFirstSql(FirstSqlBuildModel model);

        public abstract void ResolveColumn(IColumnDescriptor columnDescriptor);
    }
}
