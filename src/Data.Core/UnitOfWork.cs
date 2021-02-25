using System.Data;
using Mkh.Data.Abstractions;

namespace Mkh.Data.Core
{
    internal class UnitOfWork : IUnitOfWork
    {
        public IDbTransaction Transaction { get; private set; }

        public UnitOfWork(IDbContext dbContext, IsolationLevel? isolationLevel)
        {
            var con = dbContext.NewConnection();
            con.Open();
            Transaction = isolationLevel != null ? con.BeginTransaction(isolationLevel.Value) : con.BeginTransaction();
        }

        public void SaveChanges()
        {
            if (Transaction != null)
            {
                Transaction.Commit();
                Transaction.Connection?.Close();
                Transaction = null;
            }
        }

        public void Rollback()
        {
            if (Transaction != null)
            {
                Transaction.Rollback();
                Transaction.Connection?.Close();
                Transaction = null;
            }
        }

        public void Dispose()
        {
            Rollback();
        }
    }
}
