using System;
using System.Collections.Generic;
using System.Data;
using Mkh.Data.Abstractions;
using Mkh.Data.Abstractions.Adapter;
using Mkh.Data.Abstractions.Descriptors;

namespace Mkh.Data.Core
{
    public abstract class DbContext : IDbContext
    {
        #region ==属性==

        public IServiceProvider Services { get; internal set; }

        public DbOptions Options { get; internal set; }

        public IDbLogger Logger { get; internal set; }

        public IDbAdapter Adapter { get; internal set; }

        public IAccountResolver AccountResolver { get; internal set; }

        public IList<IEntityDescriptor> EntityDescriptors { get; } = new List<IEntityDescriptor>();

        public IList<IRepositoryDescriptor> RepositoryDescriptors { get; } = new List<IRepositoryDescriptor>();

        #endregion

        #region ==方法==

        public IDbConnection NewConnection()
        {
            return Adapter.NewConnection(Options.ConnectionString);
        }

        public IUnitOfWork NewUnitOfWork()
        {
            return new UnitOfWorkAbstract(this);
        }

        public IUnitOfWork NewUnitOfWork(IsolationLevel isolationLevel)
        {
            return new UnitOfWorkAbstract(this, isolationLevel);
        }

        #endregion
    }
}
