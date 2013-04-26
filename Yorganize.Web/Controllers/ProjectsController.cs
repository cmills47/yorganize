using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using System.Web.Mvc;
using Yorganize.Business.Repository;
using Yorganize.Domain.Models;
using Yorganize.Web.Infrastructure;
using AutoMapper;
using Yorganize.Web.Models;

namespace Yorganize.Web.Controllers
{
    [Authorize]
    public class ProjectsController : Controller
    {
        private readonly IKeyedRepository<Guid, Folder> _folderRepository;
        private readonly IKeyedRepository<Guid, Project> _projectRepository;

        public ProjectsController
        (
            IKeyedRepository<Guid, Folder> folderRepository,
            IKeyedRepository<Guid, Project> projectRepository
        )
        {
            _folderRepository = folderRepository;
            _projectRepository = projectRepository;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetData()
        {
            using (var tr = _folderRepository.BeginTransaction())
            {
                var foldersQuery = from folder in _folderRepository.All()
                        // where folder.Owner.ID
                        select folder;
                
                foldersQuery = foldersQuery.FetchMany(f => f.Projects)
                    .ThenFetch(p => p.Actions)
                    .Distinct();
                
                var folders = foldersQuery.ToList();


                var rootProjectsQuery = from project in _projectRepository.All()
                                        // where project.Owner.ID
                                        where project.Folder == null
                                        select project;

                rootProjectsQuery = rootProjectsQuery.FetchMany(p => p.Actions)
                                                     .Distinct();

                var rootProjects = rootProjectsQuery.ToList();

                _folderRepository.CommitTransaction();

                folders.Add(new Folder()
                    {
                        ID = Guid.Empty,
                        Parent = null,
                        Name = ".",
                        Projects =  rootProjects,
                    });

                var foldersModel = Mapper.Map<IEnumerable<Folder>, IEnumerable<FolderModel>>(folders);

                return new JsonNetResult(foldersModel);
            }
        }

    }
}
