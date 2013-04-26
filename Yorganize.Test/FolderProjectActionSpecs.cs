using System.Linq;
using NUnit.Framework;
using System;
using NHibernate.Transform;
using NHibernate.Linq;
using Ninject;
using Yorganize.Business.Repository;
using Yorganize.Domain.Models;

namespace Yorganize.Test
{
    [TestFixture]
    public class FolderProjectActionSpecs
    {
        private IKeyedRepository<Guid, Folder> _folderRepository;
        private IKeyedRepository<Guid, Project> _projectRepository;

        [TestFixtureSetUp]
        public void SetUp()
        {
            var kernel = new StandardKernel(new NinjectSettings() { InjectNonPublic = true });
            kernel.Load(new Business.Modules.NHibernateRepositoryModule());
            _folderRepository = kernel.Get<IKeyedRepository<Guid, Folder>>();
        }

        [Test]
        public void GetChildFolders()
        {
            var q = _folderRepository.All()
                                     .FetchMany(f => f.Projects)
                                     .ThenFetch(p => p.Actions)
                                     .Distinct();
            var result = q.ToList();

            Console.WriteLine("Got {0} folders:", result.Count);
            foreach (var folder in result)
            {
                
                Console.WriteLine("{0} {1} {2}",folder.Name, folder.Parent == null ? "NULL" : folder.Parent.Name, folder.Projects.Count());
            }

        }

    }
}
