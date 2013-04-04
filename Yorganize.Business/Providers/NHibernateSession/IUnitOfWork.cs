using System;
using NHibernate;

namespace Yorganize.Business.Providers.NHibernateSession
{
    public interface IUnitOfWork : IDisposable
    {
        ISession Session { get; }
        void Commit();
        void Rollback();
    }
}
