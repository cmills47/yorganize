using NHibernate;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Yorganize.Business.Repository
{
    public interface IReadOnlyRepository<TEntity> where TEntity : class
    {
        IQueryable<TEntity> All();
        TEntity FindBy(Expression<Func<TEntity, bool>> expression);
        IQueryable<TEntity> FilterBy(Expression<Func<TEntity, bool>> expression);

        ITransaction BeginTransaction();
        void CommitTransaction(ITransaction transaction = null);
        void RollbackTransaction(ITransaction transaction = null);
    }
}
