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
using Yorganize.Business.Providers.Membership.Principal;
using Yorganize.Business.Exceptions;
using System.Transactions;

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
            var ownerID = User.As<ILiveIdPrincipal>().UserId;

            using (var tr = _folderRepository.BeginTransaction())
            {
                var foldersQuery = from folder in _folderRepository.All()
                                   where folder.Owner.ID == ownerID
                                   select folder;

                foldersQuery = foldersQuery.OrderBy(f => f.Position)
                                           .FetchMany(f => f.Projects)
                                           .ThenFetch(p => p.Actions)
                                           .Distinct();

                var folders = foldersQuery.ToList();

                var rootProjectsQuery = from project in _projectRepository.All()
                                        where project.Owner.ID == ownerID && project.Folder == null
                                        select project;

                rootProjectsQuery = rootProjectsQuery
                    .FetchMany(p => p.Actions)
                    .Distinct();

                var rootProjects = rootProjectsQuery.ToList();

                _folderRepository.CommitTransaction(tr);

                folders.Add(new Folder
                {
                    ID = Guid.Empty,
                    Parent = null,
                    Name = "Projects",
                    Projects = rootProjects,
                });

                var foldersModel = Mapper.Map<IEnumerable<Folder>, IEnumerable<FolderModel>>(folders);

                return new JsonNetResult(foldersModel);
            }
        }

        [HttpPost]
        public ActionResult CreateFolder(FolderModel model)
        {
            var owner = new Member { ID = User.As<ILiveIdPrincipal>().UserId };

            using (var ts = new TransactionScope())
            {
                using (var tr = _folderRepository.BeginTransaction())
                {
                    var folderPosition = (from f in _folderRepository.All()
                                          where f.Owner == owner && f.Parent.ID == model.ParentID
                                          orderby f.Position descending
                                          select f.Position).FirstOrDefault() ?? 0;

                    var projectPosition = (from p in _projectRepository.All()
                                           where p.Owner == owner && p.Folder.ID == model.ParentID
                                           orderby p.Position descending
                                           select p.Position).FirstOrDefault() ?? 0;

                    _folderRepository.CommitTransaction(tr);

                    model.Position = folderPosition > projectPosition ? folderPosition + 1 : projectPosition + 1;
                }

                var folder = Mapper.Map<FolderModel, Folder>(model);
                folder.Owner = owner;

                _folderRepository.Save(folder);

                ts.Complete();
                Mapper.Map(folder, model);
            }

            return new JsonNetResult(model);
        }

        [HttpPut]
        public ActionResult UpdateFolder(FolderModel model)
        {
            if (model == null || !model.ID.HasValue || model.ID.Value == default(Guid))
                throw new BusinessException("Failed to update folder. Unknown identifier.");

            var folder = _folderRepository.FindByID(model.ID.Value);
            Mapper.Map(model, folder);
            _folderRepository.Update(folder);

            return new JsonNetResult(model);
        }

        [HttpDelete]
        public ActionResult DeleteFolder(Guid id)
        {
            var folder = _folderRepository.FindByID(id);
            if (folder == null)
                throw new BusinessException("Failed to delete folder. Not found.");

            _folderRepository.Delete(folder);

            return new JsonNetResult("Folder deleted");
        }

        [HttpPost]
        public ActionResult CreateProject(ProjectModel model)
        {
            var owner = new Member { ID = User.As<ILiveIdPrincipal>().UserId };

            using (var ts = new TransactionScope())
            {
                using (var tr = _projectRepository.BeginTransaction())
                {
                    var folderPosition = (from f in _folderRepository.All()
                                          where f.Owner == owner && f.Parent.ID == model.FolderID
                                          orderby f.Position descending
                                          select f.Position).FirstOrDefault() ?? 0;

                    var projectPosition = (from p in _projectRepository.All()
                                           where p.Owner == owner && p.Folder.ID == model.FolderID
                                           orderby p.Position descending
                                           select p.Position).FirstOrDefault() ?? 0;

                    _projectRepository.CommitTransaction(tr);

                    model.Position = folderPosition > projectPosition ? folderPosition + 1 : projectPosition + 1;
                }

                var project = Mapper.Map<ProjectModel, Project>(model);
                project.Owner = owner;

                _projectRepository.Save(project);

                ts.Complete();
                Mapper.Map(project, model);
            }

            return new JsonNetResult(model);
        }

        [HttpPut]
        public ActionResult UpdateProject(ProjectModel model)
        {
            if (model == null || model.ID == default(Guid))
                throw new BusinessException("Failed to update project. Unknown identifier.");

            var project = _projectRepository.FindByID(model.ID);
            Mapper.Map(model, project);
            _projectRepository.Update(project);

            return new JsonNetResult(model);
        }

        [HttpDelete]
        public ActionResult DeleteProject(Guid id)
        {
            var project = _projectRepository.FindByID(id);
            if (project == null)
                throw new BusinessException("Failed to delete project. Not found.");

            _projectRepository.Delete(project);

            return new JsonNetResult("Project deleted.");
        }


    }
}
