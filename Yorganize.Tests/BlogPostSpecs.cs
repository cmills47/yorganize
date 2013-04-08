using System;
using NUnit.Framework;
using Yorganize.Business.Repository;
using Yorganize.Showcase.Domain.Models;
using Ninject;
using System.Linq;

namespace Yorganize.Showcase.Tests
{
    [TestFixture]
    public class BlogPostSpecs
    {
        private StandardKernel kernel;

        [TestFixtureSetUp]
        public void SetUp()
        {
            kernel = new StandardKernel(new NinjectSettings() { InjectNonPublic = true });
            kernel.Load(new Business.Modules.NHibernateRepositoryModule());
        }

        [Test]
        public void CanGetArchive()
        {
            IKeyedRepository<Guid, BlogPost> _blogPostRepository = kernel.Get<IKeyedRepository<Guid, BlogPost>>();


            var q = from post in _blogPostRepository.All()
                    group post by new
                                      {
                                          post.Created.Year,
                                          post.Created.Month
                                      }
                    into g
                    select new { g.Key, c=g.Count()};

            var result = q.ToList();

            

            foreach (var item in result)
                Console.WriteLine("{0} {1} {2}",
                    System.Globalization.DateTimeFormatInfo.CurrentInfo.GetMonthName(item.Key.Month)
                    ,item.Key.Year, item.c);
        }
    }
}
