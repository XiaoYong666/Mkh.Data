using Mkh.Data.Abstractions.Queryable;
using Mkh.Data.Core.Queryable;

namespace Mkh.Data.Core.Repository
{
    public abstract partial class RepositoryAbstract<TEntity>
    {
        public IQueryable<TEntity> Find(bool noLock = true)
        {
            return new Queryable<TEntity>(this, noLock);
        }
    }
}
