using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;

namespace Yorganize.Business.Repository
{
    public class Repository<TKey, T> : IKeyedRepository<TKey, T> where T : class, IEntity<TKey>
    {
        protected readonly ISession _session;

        public Repository(ISession session)
        {
            _session = session;
        }

        public T FindByID(TKey id)
        {
            return _session.Get<T>(id);
        }

        public TKey Add(T entity)
        {
            TKey ID = (TKey)_session.Save(entity);

            return ID;
        }

        public void Add(IEnumerable<T> items)
        {
            foreach (T item in items)
                _session.Save(item);
        }

        public TKey Insert(T entity)
        {
            BeginTransaction();
            TKey ID = (TKey)_session.Save(entity);
            CommitTransaction();

            return ID;
        }

        public void Insert(IEnumerable<T> items)
        {
            BeginTransaction();
            foreach (T item in items)
                _session.Save(item);
            CommitTransaction();
        }

        public void Update(T entity)
        {
            BeginTransaction();
            _session.Update(entity);
            CommitTransaction();
        }

        public void Update(IEnumerable<T> items)
        {
            BeginTransaction();
            foreach (T item in items)
                _session.Update(item);
            CommitTransaction();
        }

        public void Save(T entity)
        {
            BeginTransaction();
            try
            {
                _session.SaveOrUpdate(entity);
                CommitTransaction();
            }
            catch (Exception)
            {
                RollbackTransaction();
                throw;
            }
        }

        public void Delete(T entity)
        {
            BeginTransaction();
            _session.Delete(entity);
            CommitTransaction();
        }

        public void Delete(IEnumerable<T> entities)
        {
            BeginTransaction();
            foreach (T entity in entities)
                _session.Delete(entity);
            CommitTransaction();
        }

        public IQueryable<T> All()
        {
            return _session.Query<T>();
        }

        public T FindBy(System.Linq.Expressions.Expression<Func<T, bool>> expression)
        {
            return FilterBy(expression).SingleOrDefault();
        }

        public IQueryable<T> FilterBy(System.Linq.Expressions.Expression<Func<T, bool>> expression)
        {
            return All().Where(expression).AsQueryable();
        }

        public ITransaction BeginTransaction()
        {
            if (_session.Transaction != null && _session.Transaction.IsActive)
                return _session.Transaction; // no need to start a new transaction

           return _session.BeginTransaction();
        }

        public void CommitTransaction(ITransaction transaction = null)
        {
            if (transaction == null)
                transaction = _session.Transaction;

            if (transaction!= null && transaction.IsActive)
                transaction.Commit();
            else
                throw new InvalidOperationException("There is no active transaction to commit.");
        }

        public void RollbackTransaction(ITransaction transaction = null)
        {
            if (transaction == null)
                transaction = _session.Transaction;

            if (transaction!=null && transaction.IsActive)
                transaction.Rollback();
            else
                throw new InvalidOperationException("There is no active transaction to rollback.");
        }

    }
}
