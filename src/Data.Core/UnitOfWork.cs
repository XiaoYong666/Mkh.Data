using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using Mkh.Data.Abstractions;

namespace Mkh.Data.Core
{
    public class UnitOfWorkAbstract : IUnitOfWork
    {
        private readonly IDbContext _dbContext;
        private IDbTransaction _transaction;
        private Dictionary<RuntimeTypeHandle, IRepository> _repositories = new Dictionary<RuntimeTypeHandle, IRepository>();

        internal UnitOfWorkAbstract(IDbContext dbContext)
        {
            _dbContext = dbContext;
            var con = dbContext.NewConnection();
            con.Open();
            _transaction = con.BeginTransaction();
        }

        internal UnitOfWorkAbstract(IDbContext dbContext, IsolationLevel isolationLevel)
        {
            _dbContext = dbContext;
            var con = dbContext.NewConnection();
            con.Open();
            _transaction = con.BeginTransaction(isolationLevel);
        }

        public TRepository Get<TRepository>() where TRepository : IRepository
        {
            var interfaceType = typeof(TRepository);
            var descriptor = _dbContext.RepositoryDescriptors.FirstOrDefault(m => m.InterfaceType == interfaceType);
            if (descriptor == null)
            {
                throw new Exception($"仓储({interfaceType.FullName})不存在");
            }

            var repository = _repositories.FirstOrDefault(m => m.Key.Equals(interfaceType.TypeHandle));
            if (repository.Value == null)
            {
                var implementType = descriptor.ImplementType;
                var instance = (TRepository)Activator.CreateInstance(implementType);
                var initMethod = implementType.GetMethod("InitWidthTransaction", BindingFlags.Instance | BindingFlags.NonPublic);
                initMethod.Invoke(instance, new Object[] { _dbContext, _transaction });

                _repositories.Add(interfaceType.TypeHandle, instance);

                return instance;
            }

            return (TRepository)repository.Value;
        }

        public void SaveChanges()
        {
            if (_transaction != null)
            {
                _transaction.Commit();
                _transaction.Connection?.Close();
                _transaction = null;
            }

            Dispose();
        }

        public void Dispose()
        {
            _transaction?.Rollback();
            _repositories = null;
        }
    }
}
