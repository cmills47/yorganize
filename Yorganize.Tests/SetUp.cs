using System;
using NUnit.Framework;
using Ninject;

namespace Yorganize.Showcase.Tests
{
    [SetUpFixture]
    public class SetUp
    {
        [TestFixtureSetUp]
        public void RunBeforeAnyTests()
        {
           // var kernel = new StandardKernel(new NinjectSettings() { InjectNonPublic = true });
           // kernel.Load(new Business.Modules.NHibernateRepositoryModule());
           //// kernel.Inject(Membership.Provider);

          
        }

        [TearDown]
        public void RunAfterAnyTests()
        {
            // nothing for now
        }
    }
}
