using System;
using System.Data;
using System.Reflection;
using Mkh.Data.Abstractions.Adapter;
using Mkh.Data.Abstractions.Adapter.SqlBuildModels;
using Mkh.Data.Abstractions.Descriptors;
using Mkh.Data.Core.Descriptors;

namespace Mkh.Data.PostgreSQL
{
    public class PostgreSQLAdapter : DbAdapterAbstract
    {
        public override DbProvider Provider => DbProvider.PostgreSQL;

        public override string BooleanTrueValue => "TRUE";

        public override string BooleanFalseValue => "FALSE";

        public override IDbConnection NewConnection(string connectionString)
        {
            throw new NotImplementedException();
        }

        public override string GeneratePagingSql(PagingSqlBuildModel model)
        {
            throw new NotImplementedException();
        }

        public override string GenerateFirstSql(FirstSqlBuildModel model)
        {
            throw new NotImplementedException();
        }

        public override void ResolveColumn(IColumnDescriptor columnDescriptor)
        {
            throw new NotImplementedException();
        }
    }
}
