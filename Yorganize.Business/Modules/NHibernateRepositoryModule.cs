using NHibernate;
using Ninject.Modules;
using Ninject.Web.Common;
using Yorganize.Business.Providers.NHibernateSession;
using Yorganize.Business.Repository;

namespace Yorganize.Business.Modules
{
    public class NHibernateRepositoryModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IUnitOfWork>().To<UnitOfWork>().InRequestScope();
            Bind<ISession>().ToProvider(new NHibernateSessionProvider());
            Bind(typeof(IKeyedRepository<,>)).To(typeof(Repository<,>));
        }
    }
}
